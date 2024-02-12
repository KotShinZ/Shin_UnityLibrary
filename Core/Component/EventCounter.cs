using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventCounter : MonoBehaviour
{
    [TitleDescription]
    public string title = "�C�x���g���Ă΂ꂽ�񐔂𐔂��āA���񐔂ɒB����ƃC�x���g���o��";

    public int nowConut = 0;
    public List<UnityEventWithNum> unityEvents = new List<UnityEventWithNum>();

    [ContextMenu("Event")]
    public void ExecuteEvent()
    {
        foreach (var e in unityEvents)
        {
            e.unityEvent.Invoke(e.num);
        }
    }


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
