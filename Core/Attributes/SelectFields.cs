using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Linq;
using Shin_UnityLibrary;
using UniRx;

#if UNITY_EDITOR
using UnityEditor.Callbacks;
#endif

public class SelectFieldsStatic
{
    /// <summary>
    /// Type������S�Ă�Field��Ԃ�
    /// </summary>
    /// <param name="baseType"></param>
    /// <returns></returns>
    public static List<FieldInfo> GetFieldsInheritedFrom(Type baseType)
    {
        //�S�Ă̌p������Ă���^�C�v���擾
        var types = Utils.GetInheriedType(baseType, true);

        return GetFields(types);
    }
    public static List<FieldInfo> GetFieldsInheritedFrom<T>()
    {
        return GetFieldsInheritedFrom(typeof(T));
    }
    public static List<FieldInfo> GetFields(List<Type> types)
    {
        List<FieldInfo> fieldNames = new();
        foreach (var t in types)
        {
            // �t�B�[���h�����擾
            FieldInfo[] fields = t.GetFields(BindingFlags.Public | BindingFlags.Instance);

            fieldNames.AddRange(fields);
        }
        return fieldNames;
    }

    /// <summary>
    /// �e�N���X�Ǝq�N���X����t�B�[���h���擾
    /// </summary>
    /// <param name="baseType"></param>
    /// <returns></returns>
    public static FieldInfo GetFieldTypeInherited(Type baseType, string fieldName)
    {
        var list = Utils.GetInheriedType(baseType, true);

        foreach (var t in list)
        {
            // �t�B�[���h�����擾
            FieldInfo[] fields = t.GetFields(BindingFlags.Public | BindingFlags.Instance);

            // �t�B�[���h�������X�g�ɒǉ�
            foreach (FieldInfo field in fields)
            {
                if (field.Name == fieldName) return field;
            }
        }
        return null;
    }

    /// <summary>
    /// target��field��value��K�p����
    /// </summary>
    /// <param name="targetObject"></param>
    /// <param name="fieldName"></param>
    /// <param name="value"></param>
    public static bool SetFieldValue(object targetObject, FieldInfo fieldInfo, AnyObjectValue value)
    {
        if (fieldInfo != null)
        {
            foreach (var t in ObjectValueType.types)
            {
                if (ObjectValueField.IsTypeIValuable(fieldInfo.FieldType, t))
                {
                    if (fieldInfo.FieldType == t) fieldInfo.SetValue(targetObject, value.GetValue(t));

                    Type valueable = typeof(IValueable<>).MakeGenericType(t);
                    MethodInfo mi = valueable.GetMethod("Get");
                    if (mi != null) mi.Invoke(t, new object[] { value.GetValue(t) });
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// �e�N���X����q�N���X���擾
    /// </summary>
    /// <param name="baseClass"></param>
    /// <param name="typeName"></param>
    /// <returns></returns>
    public static Type GetTypeInheritedFromString(Type baseClass, string typeName)
    {
        var list = Utils.GetInheriedType(baseClass, true);
        foreach (var l in list)
        {
            if (l.Name == typeName) return l;
        }
        return null;
    }

    public static List<string> MakeStringsFromFields(List<FieldInfo> fields, Predicate<FieldInfo> isAddField = null, bool isAddTypeText = true)
    {
        var list = new List<string>();
        foreach (FieldInfo field in fields)
        {
            if (isAddField != null && isAddField(field) == false) continue; 
            string text = field.Name;
            if (isAddTypeText) text += " (" + field.FieldType.Name + ")";
            list.Add(text);
        }
        return list;
    }

    public static List<string> MakeStringsFromTypes(List<Type> fields)
    {
        var list = new List<string>();
        foreach (Type field in fields)
        {
            list.Add(field.Name);
        }
        return list;
    }
}

[System.Serializable]
public class GetField : IObservable<object>
{
    public GetField(Type baseType)
    {
        this.baseType = baseType;
        baseTypeName = baseType.Name;
    }

    public int selectedClass;
    public int selectedField;

    public Type baseType;//�e�N���X
    public string baseTypeName;


    public string typeName;//�q�N���X�̖��O
    public Type type { get { if (m_type == null) m_type = Utils.GetTypeFromString(typeName); return m_type; } }
    protected Type m_type; //�q�N���X

    public string fieldName { get { if (m_fieldName == null) { m_fieldName = m_fieldInfo.Name; } return m_fieldName; } }
    public string m_fieldName; //�q�N���X�̎w��̃t�B�[���h�̖��O

    public FieldInfo fieldInfo
    {
        get
        {
            if (m_fieldInfo == null)
            {
                if (m_type == null) m_type = Utils.GetTypeFromString(typeName);

                m_fieldInfo = type.GetField(m_fieldName);
                if (m_fieldInfo != null) return m_fieldInfo;
                else
                {
                    m_fieldInfo = SelectFieldsStatic.GetFieldTypeInherited(baseType, m_fieldName);
                    return m_fieldInfo;
                }
            }
            return m_fieldInfo;
        }
    }
    private FieldInfo m_fieldInfo;

    IObservable<object> observable;

    public virtual object GetValue(object targetObject)
    {
        /*Debug.Log(selectedField);
        Debug.Log(targetObject.GetType());
        Debug.Log(fieldInfo);
        Debug.Log(fieldInfo.Name);
        Debug.Log(fieldInfo.GetValue(targetObject));*/

        return fieldInfo.GetValue(targetObject);
    }
    public object GetValueFromList(IEnumerable<object> targetObject)
    {
        object k = null;
        foreach (object t in targetObject)
        {
            try
            {
                k = GetValue(t);
                if (k != null) return k;
            }
            catch { }
        }
        return null;
    }

    public T GetValue<T>(object targetObject) where T : class
    {
        return GetValue(targetObject) as T;
    }
    public T GetValueFromList<T>(IEnumerable<object> targetObject) where T : class
    {
        object k = null;
        //Debug.Log(fieldInfo.Name);
        foreach (object t in targetObject)
        {

            if (t.GetType() == type)
            {
                k = GetValue(t);
                if (k != null) return k as T;
            }

        }
        return null;
    }

    public IDisposable Subscribe(IObserver<object> observer)
    {
        if(observable == null)
        {
            observable = GetValue(this).ObserveEveryValueChanged(o => o);
            Debug.Log(observable);
            /*Type genericType = typeof(ReactiveProperty<>);

            // intをジェネリック型引数として具体的な型を作成
            reactiveType = genericType.MakeGenericType(fieldInfo.GetType());

            // その型の新しいインスタンスを作成
            observable = Activator.CreateInstance(reactiveType);*/
        }
        observable.Subscribe(observer);
        /*
        var subscribeMethod = reactiveType.GetMethod("Subscribe", new[] { fieldInfo.GetType() });

        if(fieldInfo.GetType() == typeof(S))
        {
            subscribeMethod.Invoke(observable, new object[] { observer });
        }
        else
        {
            Debug.LogWarning("Subscribe failed. To Subscribe, you need equal fieldType and inputType");
        }*/

        return observable as IDisposable;
    }
}

[System.Serializable]
public class SetField : GetField
{
    public AnyObjectValue fieldValue;

    public SetField(Type baseType) : base(baseType)
    {
        this.baseType = baseType;
        baseTypeName = baseType.Name;
    }

    public void SetValue(object targetObject)
    {
        SelectFieldsStatic.SetFieldValue(targetObject, fieldInfo, fieldValue);
    }
}

[System.Serializable]
public class GetFieldObject : GetField
{
    public Component behaviour;

    public GetFieldObject(Type baseType) : base(baseType)
    {
        this.baseType = baseType;
        baseTypeName = baseType.Name;
    }

    public override object GetValue(object targetObject)
    {
        return base.GetValue(behaviour);
    }
}


public class SelectFieldsAttribute : PropertyAttribute
{
    public Predicate<FieldInfo> isAddList;
    public SelectFieldsAttribute()
    {
        this.isAddList = c => true;
    }
}

#if UNITY_EDITOR

/// <summary>
/// SelectFields, SetField��p�����A����N���X�̒�����t�B�[���h��I���o����B
/// </summary>
[CustomPropertyDrawer(typeof(SelectFieldsAttribute))]
public class SelectFieldsDrawer : PropertyDrawer
{
    SelectFieldsAttribute fields { get { return (SelectFieldsAttribute)attribute; } }
    bool inited = false;

    Type baseType;
    List<Type> classList;
    List<string> classListString;
    List<FieldInfo> fieldslist;
    List<string> list;
    int preClassSelect = -1;
    string preBaseType = null;

    public int fieldSelect;

   

    public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
    {
        #region FindProperty
        var baseTypeNameProp = prop.FindPropertyRelative("baseTypeName"); //�I������Ă���t�B�[���h�̃^�C�v�̖��O
        var typeNameProp = prop.FindPropertyRelative("typeName"); //�I������Ă���t�B�[���h�̃^�C�v�̖��O
        var fieldNameProp = prop.FindPropertyRelative("m_fieldName"); //�I������Ă���t�B�[���h�̖��O
        var valueProp = prop.FindPropertyRelative("fieldValue"); //�Z�b�g����l
        var selectFieldProp = prop.FindPropertyRelative("selectedField"); //�Z�b�g����l
        var selectClassProp = prop.FindPropertyRelative("selectedClass"); //�Z�b�g����l 
        var behaviourProp = prop.FindPropertyRelative("behaviour"); //�Z�b�g����l 
        #endregion

        if (baseTypeNameProp.stringValue == "") return;

        #region Init

        void Init()
        {
            baseType = Utils.GetTypeFromString(baseTypeNameProp.stringValue);
            if (baseType == null) return;
            classList = Utils.GetInheriedType(baseType, true);
            classListString = SelectFieldsStatic.MakeStringsFromTypes(classList);
        }

        if (preBaseType != baseTypeNameProp.stringValue) inited = false;

        if (inited == false)
        {
            Init();
            inited = true;
        } 
        #endregion

        EditorGUI.BeginProperty(position, label, prop);
        EditorGUILayout.BeginVertical(GUI.skin.box);

        EditorGUILayout.LabelField("Fields  " + baseTypeNameProp.stringValue);

        if(behaviourProp != null)
        {
            EditorGUILayout.ObjectField(behaviourProp);
            if(behaviourProp.objectReferenceValue != null)
            {
                baseTypeNameProp.stringValue = behaviourProp.objectReferenceValue.GetType().Name;
                Init();
            }
        }
        
        if (classList != null && classList.Count != 0)  //クラスを選択
        {
            selectClassProp.intValue = EditorGUILayout.Popup("SelectClass", selectClassProp.intValue, classListString.ToArray());
            typeNameProp.stringValue = classListString[selectClassProp.intValue];
        }

        if(preBaseType != baseTypeNameProp.stringValue || selectClassProp.intValue != preClassSelect || list.Count <= selectFieldProp.intValue) //フィールドリストを更新
        {
            fieldslist = SelectFieldsStatic.GetFieldsInheritedFrom(classList[selectClassProp.intValue]);
            list = SelectFieldsStatic.MakeStringsFromFields(fieldslist , fields.isAddList);
        }

        if (list != null && list.Count != 0) //フィールドを選択
        {

            fieldSelect = selectFieldProp.intValue;
            fieldSelect = EditorGUILayout.Popup("SelectField", fieldSelect, list.ToArray());
            selectFieldProp.intValue = fieldSelect;
            fieldNameProp.stringValue = fieldslist[selectFieldProp.intValue].Name;
        }

        if (valueProp != null) //SetValueの場合、Setする値を代入
        {
            var selectedFieldType = fieldslist[selectFieldProp.intValue].FieldType; //�^�C�v���擾
            ObjectValueField.MultiObjectField(valueProp, selectedFieldType, "SetObject"); //�t�B�[���h�ɓ����l����
        }

        preBaseType = baseTypeNameProp.stringValue;
        preClassSelect = selectClassProp.intValue;

        EditorGUILayout.EndHorizontal();
        EditorGUI.EndProperty();
        EditorGUILayout.Space(10);
    }
}
#endif