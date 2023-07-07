// Listの機能を拡張するクラス
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
        var indexes = new Stack<int>(); // 削除するインデックスを覚えておく

        for (int i = 0; i < list.Count; i++)
        {
            Loop next = func(list[i]);
            if (next == Loop.Remove)
            {
                indexes.Push(i);
            }
            else if (next == Loop.Break) // 処理を中断
            {
                break;
            }
        }

        foreach (int i in indexes)
        {
            list.RemoveAt(i); // 最後にまとめて消す
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

// ループ処理を継続するかどうか
public enum Loop
{
    Continue = 0, // 続ける
    Break,        // 終了する
    Remove,       // 削除する
}
