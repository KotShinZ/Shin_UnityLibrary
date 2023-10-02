using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public interface IObservableStr
{
    IDisposable SubscribeToString(IObserver<string> observer);
}
