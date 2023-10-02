// List�̋@�\��g������N���X
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;
using System.Collections;

public static class ListExtension
{
    public static void RemoveAllListFunc<T>(this IList<T> list, Func<T, Loop> func)
    {
        var indexes = new Stack<int>(); // �폜����C���f�b�N�X��o���Ă���

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

    /// <summary>
    /// リストが同じかどうかを判定する（順不同）
    /// </summary>
    /// <param name="list1"></param>
    /// <param name="list2"></param>
    /// <returns></returns>
    public static bool AreListsEqual<T>(this List<T> list1, List<T> list2 , bool noOrder = false)
    {
        if (list1 == null || list2 == null)
            return false;

        if (list1.Count != list2.Count)
            return false;

        if(noOrder)
        {
            List<T> sortedList1 = list1.OrderBy(item => item).ToList();
            List<T> sortedList2 = list2.OrderBy(item => item).ToList();
            return sortedList1.SequenceEqual(sortedList2);
        }

        return list1.SequenceEqual(list2);
    }
}

/// <summary>
/// 重複すると、値を増やす、もしくは減らす
/// </summary>
/// <typeparam name="T"></typeparam>
[System.Serializable]
public class HashSetCountable<T> : HashSet<T> where T : ICountableObject
{
    public bool Increase(T t)
    {
        T target;
        var b = TryGetValue(t, out target);
        if (!b) { target.Increase(t); return true; }
        else { Add(t); return false; }
    }
    public void Increase(ICollection<T> collection)
    {
        foreach(var t in collection)
        {
            Increase(t);
        }
    }
    public bool Decrease(T t)
    {
        T target;
        var b = TryGetValue(t, out target);
        if (!b) { target.Decrease(t); return true; }
        else { Remove(t); return false; }
    }
    public void Decrease(ICollection<T> collection)
    {
        foreach (var t in collection)
        {
            Decrease(t);
        }
    }
}

// ���[�v������p�����邩�ǂ���
public enum Loop
{
    Continue = 0, // ������
    Break,        // �I������
    Remove,       // �폜����
}
