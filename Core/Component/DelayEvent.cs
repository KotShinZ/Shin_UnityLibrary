using Cysharp.Threading.Tasks;
using Shin_UnityLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DelayEvent : MonoBehaviour
{
    public UnityEvent OnDelayed = new();
    public float delay;

    public void Invoke()
    {
        Delay().Forget();
    }

    public async UniTask Delay()
    {
        await Utils.Delay(delay);
        OnDelayed?.Invoke();
    }
}
