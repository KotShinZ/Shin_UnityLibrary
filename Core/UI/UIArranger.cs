using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class UIArranger : MonoBehaviour
{
    public GameObject parent;
    public GameObject prehub;

    [FoldoutGroup("Transform"), Vector2Slider(-0.5f,0.5f)] public Vector2 firstPosition;
    [FoldoutGroup("Transform"), Vector2Slider(0,0.1f)] public Vector2 space;
    [FoldoutGroup("Transform")] public float scale = 1;

    public Vector2Int MaxArrangeNum;

    public List<RectTransform> generated;

    [Space(10)] public bool arrangeDebugUpdate = false;

    public void Update()
    {
        if (arrangeDebugUpdate)
        {
            ReArrangeTransform();
        }
    }

    /// <summary>
    /// 2      1     ɕύX
    /// </summary>
    /// <param name="i"></param>
    /// <param name="j"></param>
    /// <returns></returns>
    protected int Get2Num(int i,int j)
    {
        return i * MaxArrangeNum.x + j;
    }

    /// <summary>
    ///  I u W F N g 𐶐      ɕ  ׂ 
    /// </summary>
    /// <param name="generateNum"></param>
    public void ReArrangeTransform()
    {
        for (int i = 0; i < MaxArrangeNum.y; i++)
        {
            for (int j = 0; j < MaxArrangeNum.x; j++)
            {
                if (generated.Count <= Get2Num(i, j)) goto BP;
                var rect = generated[Get2Num(i, j)];
                SetTransform(rect, i, j);
            }
        }
    BP:;
    }

    public void ReArrange(int generateNum = 1000)
    {
        for (int i = 0; i < MaxArrangeNum.y; i++)
        {
            for (int j = 0; j < MaxArrangeNum.x; j++)
            {
                var rect = generated[Get2Num(i, j)];
                SetTransform(rect, i, j);
                SetObjectParam(rect , i, j);
                if (generateNum < Get2Num(i, j)) goto BP;
            }
        }
    BP:;
    }

    /// <summary>
    ///  I u W F N g 𐶐    Ȃ     ׂ 
    /// </summary>
    /// <param name="generateNum"></param>
    [Button]
    public void ArrangeGenerate(int generateNum = 1000)
    {
        Delete();

        for (int i = 0; i < MaxArrangeNum.y; i++)
        {
            for (int j = 0; j < MaxArrangeNum.x; j++)
            {
                if (generateNum <= Get2Num(i, j)) goto BP;
                var ins = Instantiate(prehub);
                var rect = ins.GetComponent<RectTransform>();
                generated.Add(rect);
                SetTransform(rect, i, j);
                SetObjectParam(rect, i, j);
               
            }
        }
    BP:;
    }

    /// <summary>
    ///  I u W F N g ̃p     [ ^ Ȃǂ ݒ 
    /// </summary>
    /// <param name="rect"></param>
    /// <param name="i"></param>
    /// <param name="j"></param>
    public abstract void SetObjectParam(RectTransform rect, int x, int y);
    
    /// <summary>
    ///  I u W F N g ̈ʒu  ݒ 
    /// </summary>
    /// <param name="rect"></param>
    /// <param name="i"></param>
    /// <param name="j"></param>
    public virtual void SetTransform(RectTransform rect, int i , int j)
    {
        var _first = PerToScreen(firstPosition);
        var _space = PerToScreen(space);
        rect.localPosition = new Vector3(_first.x + _space.x * j, _first.y - _space.y * i, 0);
        rect.localScale = Vector3.one * scale;
        rect.SetParent(parent.transform);
    }

    /// <summary>
    ///          I u W F N g  폜
    /// </summary>
    public virtual void Delete()
    {
        if (generated != null && generated.Count != 0)
        {
            if (EditorApplication.isPlaying)
            {
                for (int i = generated.Count - 1; i >= 0; i--)
                {
                    if (generated[i] != null) Destroy(generated[i].gameObject);
                }
            }
            else
            {
                for (int i = generated.Count - 1; i >= 0; i--)
                {
                    if (generated[i] != null) EditorApplication.delayCall += () => DestroyImmediate(generated[i].gameObject);
                }
            }
        }
        generated = new();
    }

    public static Vector2 PerToScreen(Vector2 per)
    {
        return new Vector2(per.x * Screen.width, per.y * Screen.height);
    }
}
