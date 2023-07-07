using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModularMotion;
using Sirenix.OdinInspector;

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
    [Button("PlayerTillEnd")]
    public void PlayTillEnd()
    {
        GetUIMotion();
        gameObject.GetComponent<UIMotion>().PlayFromStartTillEnd();
    }

    [Button("PlayerOne")]
    public void PlayOne()
    {
        GetUIMotion();
        gameObject.GetComponent<UIMotion>().Play();
    }

    [Button("Reset")]
    public void ResetUI()
    {
        GetUIMotion();
        gameObject.GetComponent<UIMotion>().ResetMotion();
    }
}
