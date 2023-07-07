using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class MySinpleAnimation
{
    public static async UniTask PlayAwait(this SimpleAnimation simpleAnimation , string anim)
    {
        simpleAnimation.Play(anim);
        await UniTask.WaitUntil(() => simpleAnimation.GetState(anim).normalizedTime >= 1);
    }
}
