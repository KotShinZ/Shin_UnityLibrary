using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;
using System.IO;
using System.Text;


public class ObjectValueType
{
    public static List<Type> types = new List<Type>() {
        typeof(int),
        typeof(float),
        typeof(string),
        typeof(Vector3),
        typeof(Vector2),
        typeof(UnityEngine.Object),
    };
}

public class ObjectValueField
{
#if UNITY_EDITOR
    /// <summary>
    /// AnyObjectValueÇTypeå^Ç≈ï\é¶Ç∑ÇÈ
    /// </summary>
    /// <param name="t"></param>
    /// <param name="propertyType"></param>
    /// <param name="label"></param>
    /// <returns></returns>
    public static void MultiObjectField(SerializedProperty t, Type propertyType, string label)
    {
        foreach(var type in ObjectValueType.types)
        {
            if(IsTypeIValuable(propertyType, type))
            {
                var prop = t.FindPropertyRelative(type.Name + "Value");
                EditorGUILayout.PropertyField(prop);
            }
        }
    }
#endif

    public static bool IsTypeIValuable(Type target, Type type)
    {
        if (target == type) return true;
        foreach (var i in target.GetInterfaces())
        {
            if (i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IValueable<>) &&
                i.GetGenericArguments()[0] == type)
            {
                return true;
            }
        }
        if(type == typeof(UnityEngine.Object) && target.IsSubclassOf(typeof(UnityEngine.Object))) { return  true; }
        return false; // false Çï‘Ç∑èÍçáÇí«â¡
    }
}
