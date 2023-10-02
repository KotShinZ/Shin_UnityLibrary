using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

public class EventCounter : MonoBehaviour
{
    [TitleDescription]
    public string title = "イベントが呼ばれた回数を数えて、一定回数に達するとイベントを出す";

    [Readonly] public int nowConut = 0;
    public List<UnityEventWithNum> unityEvents = new List<UnityEventWithNum>();

    [Button]
    public void EventCount(int count = 1)
    {
        nowConut += count;
        foreach(var e in unityEvents)
        {
            if(e.num == nowConut)
            {
                e.unityEvent.Invoke(e.num);
            }
        }
    }

    [System.Serializable]
    public class UnityEventWithNum
    {
        public int num;
        public UnityEvent<int> unityEvent;
    }
}
