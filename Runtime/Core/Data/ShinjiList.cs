// List�̋@�\���g������N���X
using System.Collections.Generic;
using System;
using System.ComponentModel;
using UnityEngine;
using System.Linq;
using Sirenix.Utilities;

public static class ShinjiList
{
    public static void RemoveAllListFunc<T>(this IList<T> list, Func<T, Loop> func)
    {
        var indexes = new Stack<int>(); // �폜����C���f�b�N�X���o���Ă���

        for (int i = 0; i < list.Count; i++)
        {
            Loop next = func(list[i]);
            if (next == Loop.Remove)
            {
                indexes.Push(i);
            }
            else if (next == Loop.Break) // �����𒆒f
            {
                break;
            }
        }

        foreach (int i in indexes)
        {
            list.RemoveAt(i); // �Ō�ɂ܂Ƃ߂ď���
        }
    }

    public static List<C> GetComponentsList<T , C>(this IList<T> list)
    {
        var components = new List<C>();
        var _list = list.OfType<UnityEngine.Component>();
        foreach (var l in _list)
        {
           components.Add( l.GetComponent<C>());
        }
        return components;
    }

    public static void DebugList<T>(this IList<T> list)
    {
        for(int i = 0; i < list.Count;i++)
        {
            var l = list[i];
            Debug.Log(typeof(T).Name + "  List");
            Debug.Log("Count   "+list.Count);
            Debug.Log(i + "  " + l == null);
        }
    }
}

// ���[�v�������p�����邩�ǂ���
public enum Loop
{
    Continue = 0, // ������
    Break,        // �I������
    Remove,       // �폜����
}
