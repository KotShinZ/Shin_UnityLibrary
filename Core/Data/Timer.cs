using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using Cysharp.Threading.Tasks;
using System.Threading;

public class Timer : BaseDataFloat, IDisposable
{
    List<TimerEvent> timeEvents = new List<TimerEvent>();
    IDisposable timeTask;
    IDisposable loopTask;

    public Action StartEvent;
    public Action StopEvent;

    public Timer(float _num = 0, bool initStart = true) : base(_num)
    {
        value = _num;
        if (initStart) Start();
    }

    public void SetTimerEvent(float time, Action<float> action)
    {
        var events = new TimerEvent(time, action);
        timeEvents.Add(events);
        events.SetEvent(prop);
    }

    public void Reset()
    {
        timeEvents.ForEach(t => t.Reset());
        value = 0;
    }
    public void Start()
    {
        StartEvent?.Invoke();
        timeTask = Observable.EveryUpdate().Subscribe(_ => value += Time.deltaTime);
    }
    public void Stop()
    {
        StopEvent?.Invoke();
        timeTask?.Dispose();
    }
    public void SetLoop(float loopTime)
    {
        loopTask = Observable.EveryUpdate().Subscribe(_=> { if (value > loopTime) Reset(); });
    }

    public void Dispose()
    {
        timeTask?.Dispose();
        loopTask?.Dispose();
    }
}

public class TimerEvent
{
    bool called = false;
    float time;
    Action<float> action;

    public TimerEvent(float time, Action<float> action)
    {
        this.time = time;
        this.action = action;
    }

    public void Reset()
    {
        called = false;
    }

    public void SetEvent(IObservable<float> observable)
    {
        observable.Subscribe(t => {
            if (t > time && called == false)
            {
                action(t);
                called = true;
            }
        });
    }
}
