using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimeEvent : MonoBehaviour
{
    [Readonly]public float nowtime;
    public List<UnityEventWithNum> timeEvents = new List<UnityEventWithNum>();

    private bool counting = false;

    public void Update()
    {
        if (counting)
        {
            nowtime += Time.deltaTime;
            foreach (var unityEvent in timeEvents)
            {
                if(!unityEvent.evented &&unityEvent.num > nowtime)
                {
                    unityEvent.unityEvent.Invoke(unityEvent.num);
                }
            }
        }
    }

    public void TimerStart()
    {
        counting = true;
    }

    public void TimerStop()
    {
        counting = false;
    }

    public void TimerReset()
    {
        nowtime = 0;
    }

    [System.Serializable]
    public class UnityEventWithNum
    {
        public int num;
        public UnityEvent<int> unityEvent;
        [HideInInspector]public bool evented;
    }
}
