using Shin_UnityLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public static class AttributeGetParam
{
    /// <summary>
    /// ある変数の属性が含むパラメータを取得する
    /// </summary>
    /// <typeparam name="T">取得する型</typeparam>
    /// <param name="value">属性はIGetParamAttribute<T>を継承する必要がある</param>
    /// <returns></returns>
    public static T GetAttrParam<T>(Type type, string fieldName)
    {
        //フィールドを取得
        FieldInfo field = type.GetField(fieldName);

        //フィールドにRemarkAttributeが定義されていれば
        if (field.IsDefined(typeof(IGetParamAttribute<T>), true))
        {
            var attribute =
                field.GetCustomAttributes(typeof(System.Attribute), true);
            foreach (var attr in attribute)
            {
                if (attr is IGetParamAttribute<T>) return (attr as IGetParamAttribute<T>).Get();
            }
            return default;
        }
        else
        {
            return default;
        }
    }

    /// <summary>
    /// ある変数の属性が含むパラメータを取得する
    /// </summary>
    /// <typeparam name="T">取得する型</typeparam>
    /// <param name="value">属性はIGetParamAttribute<T>を継承する必要がある</param>
    /// <returns></returns>
    public static List<T> GetAttrParamEnum<T>(this Enum value)
    {
        List<T> output = new List<T>();
        Type type = value.GetType(); //valueのタイプを取得
        var list = Utils.GetEnumList(type); //全要素取得
        foreach ( var attr in list)
        {
            if (value.HasFlag(attr)) output.Add(GetAttrParam<T>(type, attr.ToString()));
        }
        
        return output;
    }
}

public interface IGetParamAttribute<T>
{
    T Get();
}

