using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Linq;
using Shin_UnityLibrary;

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

    public static List<string> MakeStringsFromFields(List<FieldInfo> fields)
    {
        var list = new List<string>();
        foreach (FieldInfo field in fields)
        {
            list.Add(field.Name);
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
public class GetField
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
    public Type type { get { if (m_type == null) m_type = SelectFieldsStatic.GetTypeInheritedFromString(baseType, typeName); return m_type; } }
    private Type m_type; //�q�N���X

    public string fieldName { get { if (m_fieldName == null) { m_fieldName = m_fieldInfo.Name; } return m_fieldName; } }
    public string m_fieldName; //�q�N���X�̎w��̃t�B�[���h�̖��O

    public FieldInfo fieldInfo
    {
        get
        {
            if (m_fieldInfo == null)
            {
                if (m_type == null) m_type = SelectFieldsStatic.GetTypeInheritedFromString(baseType, typeName);
                if (m_type == null)
                {
                    m_fieldInfo = SelectFieldsStatic.GetFieldTypeInherited(baseType, m_fieldName);
                    return m_fieldInfo;
                }
                else
                {
                    m_fieldInfo = type.GetField(m_fieldName);
                }
                return m_fieldInfo;
            }
            return null;
        }
    }
    private FieldInfo m_fieldInfo;

    public object GetValue(object targetObject)
    {
        /*Debug.Log(targetObject.GetType());
        Debug.Log(fieldInfo);
        Debug.Log(fieldInfo.GetValue(targetObject));*/
        return fieldInfo.GetValue(targetObject);
    }
    public object GetValueFromList(IEnumerable<object> targetObject)
    {
        object k = null;
        foreach (object t in targetObject)
        {
            if (t.GetType() == type)
            {
                k = GetValue(t);
                if (k != null) return k;
            }
               
        }
        return null;
    }

    public T GetValue<T>(object targetObject) where T : class
    {
        return fieldInfo.GetValue(targetObject) as T;
    }
    public T GetValueFromList<T>(IEnumerable<object> targetObject) where T : class
    {
        object k = null;
        Debug.Log(fieldInfo.Name);
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
}

[System.Serializable]
public class SetField : GetField
{
    public AnyObjectValue fieldValue;

    public SetField(Type baseType) : base(baseType)
    {

    }

    public void SetValue(object targetObject)
    {
        SelectFieldsStatic.SetFieldValue(targetObject, fieldInfo, fieldValue);
    }
}

public class SelectFieldsAttribute : PropertyAttribute
{

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

    public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
    {
        var baseTypeNameProp = prop.FindPropertyRelative("baseTypeName"); //�I������Ă���t�B�[���h�̃^�C�v�̖��O
        var typeNameProp = prop.FindPropertyRelative("typeName"); //�I������Ă���t�B�[���h�̃^�C�v�̖��O
        var fieldNameProp = prop.FindPropertyRelative("m_fieldName"); //�I������Ă���t�B�[���h�̖��O
        var valueProp = prop.FindPropertyRelative("fieldValue"); //�Z�b�g����l
        var selectFieldProp = prop.FindPropertyRelative("selectedField"); //�Z�b�g����l
        var selectClassProp = prop.FindPropertyRelative("selectedClass"); //�Z�b�g����l

        if(inited == false)
        {
            baseType = Utils.GetTypeFromString(baseTypeNameProp.stringValue);
            classList = Utils.GetInheriedType(baseType, true);
            classListString = SelectFieldsStatic.MakeStringsFromTypes(classList);
            inited = true;
        }

        EditorGUI.BeginProperty(position, label, prop);
        EditorGUILayout.BeginVertical(GUI.skin.box);

        EditorGUILayout.LabelField("Fields  " + baseTypeNameProp.stringValue);
        
        if (classList != null && classList.Count != 0) 
        {
            selectClassProp.intValue = EditorGUILayout.Popup("�N���X��I��", selectClassProp.intValue, classListString.ToArray()); //�N���X��I��
            typeNameProp.stringValue = classListString[selectClassProp.intValue];
        }

        if(selectClassProp.intValue != preClassSelect || list.Count <= selectFieldProp.intValue)
        {
            fieldslist = SelectFieldsStatic.GetFieldsInheritedFrom(classList[selectClassProp.intValue]);//�t�B�[���h���X�g���擾
            list = SelectFieldsStatic.MakeStringsFromFields(fieldslist); //�t�B�[���h�����񃊃X�g���擾
        }

        if (list != null && list.Count != 0)
        {
            selectFieldProp.intValue = EditorGUILayout.Popup("�t�B�[���h��I��", selectFieldProp.intValue, list.ToArray()); //�t�B�[���h��I��
            fieldNameProp.stringValue = list[selectFieldProp.intValue];
        }

        if (valueProp != null) //SetValue�Ȃ�
        {
            var selectedFieldType = fieldslist[selectFieldProp.intValue].FieldType; //�^�C�v���擾
            ObjectValueField.MultiObjectField(valueProp, selectedFieldType, valueProp.name); //�t�B�[���h�ɓ����l����
        }

        preClassSelect = selectClassProp.intValue;

        EditorGUILayout.EndHorizontal();
        EditorGUI.EndProperty();
        EditorGUILayout.Space(10);
    }
}
#endif