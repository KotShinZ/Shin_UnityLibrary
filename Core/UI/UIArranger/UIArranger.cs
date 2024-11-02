using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;
using UnityEngine.Events;
using Shin_UnityLibrary;

public class UIArranger : MonoBehaviour
{
    [TitleDescription] public string t = "UIを等間隔に並べる";

    public GameObject parent;
    public GameObject prehub;

    [FoldOut("Transform", true), Vector2Slider(-1.5f, 1.5f)] public Vector2 firstPosition;　//一つ目の位置
    [FoldOut("Transform"), Vector2Slider(0, 1f)] public Vector2 space = new Vector2(0.02f, 0.02f); //間隔
    [FoldOut("Transform")] public float scale = 1; //それぞれの大きさ

    public Vector2Int MaxArrangeNum = new Vector2Int(2, 2); //並べる最大数
    public bool instantinateCopy;

    [Space(5)]
    public List<RectTransform> generated; //生成したオブジェクトたち

    [Space(10)]
    public bool arrangeDebugEditor = false; //並べた様子をデバッグするか

    private int maxCount => MaxArrangeNum.x * MaxArrangeNum.y;

    public UnityEvent<GameObject> OnInstantiated = new UnityEvent<GameObject>();

    public Action<GameObject, Vector2Int> instantiatedAction;

    public virtual void Start()
    {
        ArrangeGenerate();
        ReArrangeTransform();
    }

    private void OnValidate()
    {
#if UNITY_EDITOR
        if (arrangeDebugEditor)
        {
            if (generated.Count != maxCount)
            {
                ArrangeGenerate();
            }
            else
            {
                ReArrangeTransform();
            }
        }
#endif
    }

    /// <summary>
    ///合計の並べることの出来る数を返す
    /// </summary>
    /// <param name="i"></param>
    /// <param name="j"></param>
    /// <returns></returns>
    protected int Get2Num(int i, int j)
    {
        return i * MaxArrangeNum.x + j;
    }

    protected int Get2Num(Vector2 vec)
    {
        return (int)(vec.x * MaxArrangeNum.x + vec.y);
    }

    /// <summary>
    /// 生成出来る数だけ繰り返す
    /// </summary>
    /// <param name="func"></param>
    /// <param name="max"></param>
    public void Iter(Func<int, int, bool> func = null, int max = -1, bool newGenerate = false)
    {
        for (int i = 0; i < MaxArrangeNum.y; i++)
        {
            for (int j = 0; j < MaxArrangeNum.x; j++)
            {
                if (max != -1 && max < Get2Num(i, j)) return;
                if (newGenerate == false)
                {
                    if (generated == null || generated.Count < maxCount)
                    {
                        ArrangeGenerate();
                    }
                }

                if (func != null)
                {
                    bool b = func.Invoke(i, j);
                    if (b) return;
                }
            }
        }
    }

    /// <summary>
    ///  新規生成をせずに位置だけ並べる
    /// </summary>
    /// <param name="generateNum"></param>
    public void ReArrangeTransform()
    {
        Iter((i, j) =>
        {
            var rect = generated[Get2Num(i, j)];
            SetTransform(rect, i, j);
            return false;
        });
    }

    /// <summary>
    /// 初期化処理も行いつつ並べる
    /// </summary>
    /// <param name="generateNum"></param>
    public void ReArrange(Func<RectTransform, int, int, bool> func = null, int max = 1000)
    {
        Iter((i, j) =>
        {
            var rect = generated[Get2Num(i, j)];
            SetTransform(rect, i, j);
            SetObjectParam(rect, i, j);
            return false;
        }, max);
    }

    /// <summary>
    /// 前回の物を削除し、初期化処理もして、並べる
    /// </summary>
    /// <param name="generateNum"></param>
    [ContextMenu("Arrange")]
    public void ArrangeGenerate()
    {
        Delete();

        Iter((i, j) =>
        {
            var rect = GenerateNewObject();
            SetTransform(rect, i, j);
            SetObjectParam(rect, i, j);
            rect.gameObject.SetActive(true);
            OnInstantiated?.Invoke(rect.gameObject);
            instantiatedAction?.Invoke(rect.gameObject, new Vector2Int(i,j));
            return false;
        }, newGenerate: true);
    }

    RectTransform GenerateNewObject()
    {
        GameObject ins;
        if (instantinateCopy) ins = Utils.InstantiateCopy(prehub);
        else ins = Instantiate(prehub);

        var rect = ins.GetComponent<RectTransform>();

        if (instantinateCopy == false)
        {
            if (parent != null) rect.SetParent(parent.transform);
            else rect.SetParent(GetComponent<RectTransform>());
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.sizeDelta = prehub.GetComponent<RectTransform>().sizeDelta;
        }

        generated.Add(rect);
        return rect;
    }

    /// <summary>
    /// オブジェクトのパラメータを設定（初期化処理）
    /// </summary>
    /// <param name="rect"></param>
    /// <param name="i"></param>
    /// <param name="j"></param>
    public virtual void SetObjectParam(RectTransform rect, int x, int y) { }

    /// <summary>
    ///  オブジェクトの位置を設定
    /// </summary>
    /// <param name="rect"></param>
    /// <param name="i"></param>
    /// <param name="j"></param>
    public virtual void SetTransform(RectTransform rect, int i, int j)
    {
        var _first = PerToScreen(firstPosition);
        var _space = PerToScreen(space);
        rect.localPosition = new Vector3(_first.x + _space.x * j, _first.y - _space.y * i, 0);
        rect.localScale = Vector3.one * scale;

        if (parent != null) rect.SetParent(parent.transform);
        else rect.SetParent(GetComponent<RectTransform>());
    }

    /// <summary>
    ///  全部消す
    /// </summary>
    public virtual void Delete()
    {
        if (generated == null || generated.Count == 0) return;
#if UNITY_EDITOR
        if (!EditorApplication.isPlaying)
        {
            DeleteObjs();
        }
#else
        DeleteObjs();
#endif

        generated = new();

        void DeleteObjs()
        {
            for (int i = generated.Count - 1; i >= 0; i--)
            {
                if (generated[i] != null) Utils.DelayFrameAction(1, () => Destroy(generated[i].gameObject));
            }
        }
    }

    /// <summary>
    /// 比率をスクリーンサイズにする
    /// </summary>
    /// <param name="per"></param>
    /// <returns></returns>
    public static Vector2 PerToScreen(Vector2 per)
    {
        return new Vector2(per.x * 1920 / 2, per.y * 1080 / 2);
    }
}
