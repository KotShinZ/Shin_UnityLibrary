// --------------------------------------------------------------------------------------------------------------------
// <author>
//   HiddenMonk
//   http://answers.unity3d.com/users/496850/hiddenmonk.html
//   
//   Johannes Deml
//   send@johannesdeml.com
// </author>
// --------------------------------------------------------------------------------------------------------------------


using System;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;

public static class SerializedPropertyExtensions
{
    /// <summary>
    /// Get the object the serialized property holds by using reflection
    /// </summary>
    /// <typeparam name="T">The object type that the property contains</typeparam>
    /// <param name="property"></param>
    /// <returns>Returns the object type T if it is the type the property actually contains</returns>
    public static T GetValue<T>(this SerializedProperty property)
    {
        return GetNestedObject<T>(property.propertyPath, GetSerializedPropertyRootComponent(property), true); //The "true" means we will also check all base classes
    }

    /// <summary>
    /// Set the value of a field of the property with the type T
    /// </summary>
    /// <typeparam name="T">The type of the field that is set</typeparam>
    /// <param name="property">The serialized property that should be set</param>
    /// <param name="value">The new value for the specified property</param>
    /// <returns>Returns if the operation was successful or failed</returns>
    public static void SetValue<T>(this SerializedProperty property, T value)
    {

        object obj = GetSerializedPropertyRootComponent(property);
        //Iterate to parent object of the value, necessary if it is a nested object
        string[] fieldStructure = property.propertyPath.Split('.');
        for (int i = 0; i < fieldStructure.Length - 1; i++)
        {
            obj = GetFieldOrPropertyValue<object>(fieldStructure[i], obj);
        }
        string fieldName = fieldStructure.Last();

        SetFieldOrPropertyValue<T>(fieldName, obj, value);

    }

    public static Component GetSerializedPropertyRootComponent(SerializedProperty property)
    {
        return (Component)property.serializedObject.targetObject;
    }

    public static T GetNestedObject<T>(string path, object obj, bool includeAllBases = false)
    {
        foreach (string part in path.Split('.'))
        {
            obj = GetFieldOrPropertyValue<object>(part, obj, includeAllBases);
        }
        return (T)obj;
    }

    public static T GetFieldOrPropertyValue<T>(string fieldName, object obj, bool includeAllBases = false, BindingFlags bindings = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
    {
        FieldInfo field = obj.GetType().GetField(fieldName, bindings);
        if (field != null) return (T)field.GetValue(obj);

        PropertyInfo property = obj.GetType().GetProperty(fieldName, bindings);
        if (property != null) return (T)property.GetValue(obj, null);

        if (includeAllBases)
        {

            foreach (Type type in GetBaseClassesAndInterfaces(obj.GetType()))
            {
                field = type.GetField(fieldName, bindings);
                if (field != null) return (T)field.GetValue(obj);

                property = type.GetProperty(fieldName, bindings);
                if (property != null) return (T)property.GetValue(obj, null);
            }
        }

        return default(T);
    }

    public static void SetFieldOrPropertyValue<T>(string fieldName, object obj, object value, bool includeAllBases = false, BindingFlags bindings = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
    {
        FieldInfo field = obj.GetType().GetField(fieldName, bindings);
        if (field != null)
        {
            field.SetValue(obj, value);
            return;
        }

        PropertyInfo property = obj.GetType().GetProperty(fieldName, bindings);
        if (property != null)
        {
            property.SetValue(obj, value, null);
            return;
        }

        if (includeAllBases)
        {
            foreach (Type type in GetBaseClassesAndInterfaces(obj.GetType()))
            {
                field = type.GetField(fieldName, bindings);
                if (field != null)
                {
                    field.SetValue(obj, value);
                    return;
                }

                property = type.GetProperty(fieldName, bindings);
                if (property != null)
                {
                    property.SetValue(obj, value, null);
                    return;
                }
            }
        }
    }

    public static IEnumerable<Type> GetBaseClassesAndInterfaces(this Type type, bool includeSelf = false)
    {
        List<Type> allTypes = new List<Type>();

        if (includeSelf) allTypes.Add(type);

        if (type.BaseType == typeof(object))
        {
            allTypes.AddRange(type.GetInterfaces());
        }
        else
        {
            allTypes.AddRange(
                    Enumerable
                    .Repeat(type.BaseType, 1)
                    .Concat(type.GetInterfaces())
                    .Concat(type.BaseType.GetBaseClassesAndInterfaces())
                    .Distinct());
            //I found this on stackoverflow
        }

        return allTypes;
    }
    
}
