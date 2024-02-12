using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cysharp.Threading.Tasks;
using UnityEngine.Video;

public class MasterUIManeger : UIManeger<MasterUIManeger>
{
    public GameObject popUpBig;
    public Image AllFade;
    public RawImage saturate;

    public static RectTransform m_mainCanvas;
    public static RectTransform mainCanvas
    {
        get
        {
            if (mainCanvas == null) m_mainCanvas = GameObject.Find("Canvas(ñºëOÇê‚ëŒÇ…ïœÇ¶Ç»Ç¢Ç≈Ç≠ÇæÇ≥Ç¢ÅIÅI)").GetComponent<RectTransform>();
            return m_mainCanvas;
        }
    }

    #region General

    public RectTransform UIInstantinate(GameObject prefab)
    {
        var ins = Instantiate(prefab, mainCanvas);
        var rect = ins.GetComponent<RectTransform>();
        return rect;
    }
    public RectTransform UIInstantinate(GameObject prefab, Vector2 position)
    {
        var ins = Instantiate(prefab, mainCanvas);
        var rect = ins.GetComponent<RectTransform>();
        rect.position = position;
        return rect;

    }

    #endregion

    #region Fade

    public async UniTask Fade(Color color, float duration, float easeTime)
    {
        FadeIn(color, easeTime);
        await SceneLoader.WaitTime(duration);
        FadeOut(easeTime);
    }
    public async UniTask Fade(Color color, float duration, float inEaseTime, float outEaseTime)
    {
        FadeIn(color, inEaseTime);
        await SceneLoader.WaitTime(duration);
        FadeOut(outEaseTime);
    }
    public void FadeIn(Color color, float easeTime)
    {
        color.a = 0;
        AllFade.color = color;
        AllFade.gameObject.SetActive(true);
        //AllFade.DOFade(1f ,easeTime);
    }
    public void FadeOut(float easeTime)
    {
        //AllFade.DOFade(0f, easeTime);
        AllFade.gameObject.SetActive(false);
    }

    #endregion

    #region Saturated line

    public async UniTask Saturate(Color color, float duration)
    {
        SaturateIn(color);
        await SceneLoader.WaitTime(duration);
        SaturateOut();
    }
    public void SaturateIn(Color color)
    {
        saturate.color = color;
        saturate.gameObject.SetActive(true);
        saturate.GetComponent<VideoPlayer>().Play();
    }
    public void SaturateOut()
    {
        saturate.GetComponent<VideoPlayer>().Stop();
        saturate.gameObject.SetActive(false);
    }

    #endregion
}
