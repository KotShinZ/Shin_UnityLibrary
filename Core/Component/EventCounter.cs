using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

public class EventCounter : MonoBehaviour
{
    [TitleDescription]
    public string title = "�C�x���g���Ă΂ꂽ�񐔂𐔂��āA���񐔂ɒB����ƃC�x���g���o��";

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
