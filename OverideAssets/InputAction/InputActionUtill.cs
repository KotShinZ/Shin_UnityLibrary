using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public static class InputActionUtill : MonoBehaviour
{
    /// <summary>
    /// 待って長押しかどうか判定する
    /// 長押しならholdedActionを実行する
    /// </summary>
    /// <param name="end"></param>
    /// <param name="holdedAction"></param>
    /// <returns></returns>
    public static async UniTask<bool> IsHoldedAction(this InputAction action, Action holdedAction = null)
    {
        var holded = false;
        CancellationTokenSource source = new CancellationTokenSource();
        UniTask.Action(async c => {
            c.ThrowIfCancellationRequested();
            await UniTask.WaitUntil(() => action.GetTimeoutCompletionPercentage() >= 1);
            holded = true;
            if (holdedAction != null) holdedAction?.Invoke();
        }, source.Token)();

        await UniTask.WaitUntil(() => action.WasReleasedThisFrame());
        source.Cancel();

        return holded;
    }

}
