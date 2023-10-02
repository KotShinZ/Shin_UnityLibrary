using Shin_UnityLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public static class AttributeGetParam
{
    /// <summary>
    /// ����ϐ��̑������܂ރp�����[�^���擾����
    /// </summary>
    /// <typeparam name="T">�擾����^</typeparam>
    /// <param name="value">������IGetParamAttribute<T>���p������K�v������</param>
    /// <returns></returns>
    public static T GetAttrParam<T>(this object value)
    {
        //value�̃^�C�v���擾
        Type type = value.GetType();

        //�t�B�[���h���擾
        FieldInfo field = type.GetField(value.ToString());

        //�t�B�[���h��RemarkAttribute����`����Ă����
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
    /// ����ϐ��̑������܂ރp�����[�^���擾����
    /// </summary>
    /// <typeparam name="T">�擾����^</typeparam>
    /// <param name="value">������IGetParamAttribute<T>���p������K�v������</param>
    /// <returns></returns>
    public static List<T> GetAttrParamEnum<T>(this Enum value)
    {
        List<T> output = new List<T>();

       
        Type type = value.GetType(); //value�̃^�C�v���擾
        var list = Utils.GetEnumList(type); //�S�v�f�擾
        foreach ( var attr in list)
        {
            if (value.HasFlag(attr)) output.Add(value.GetAttrParam<T>());
        }
        
        return output;
    }
}

public interface IGetParamAttribute<T>
{
    T Get();
}

