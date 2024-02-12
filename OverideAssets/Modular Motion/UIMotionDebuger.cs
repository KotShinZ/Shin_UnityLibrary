using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModularMotion;

[RequireComponent(typeof(UIMotion))]
public class UIMotionDebuger : MonoBehaviour
{
    UIMotion uIMotion;

    void Start()
    {
        GetUIMotion();
    }

    void GetUIMotion()
    {
        if(uIMotion == null)
        {
            uIMotion = GetComponent<UIMotion>();
        }
    }

    public void PlayTillEnd()
    {
        GetUIMotion();
        gameObject.GetComponent<UIMotion>().PlayFromStartTillEnd();
    }

    public void PlayOne()
    {
        GetUIMotion();
        gameObject.GetComponent<UIMotion>().Play();
    }

    public void ResetUI()
    {
        GetUIMotion();
        gameObject.GetComponent<UIMotion>().ResetMotion();
    }
}
