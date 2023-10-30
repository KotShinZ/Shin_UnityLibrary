using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using UnityEngine.Events;

public class ObjStateManeger : MonoBehaviour
{
    public BaseDataInt nowState = new(0);

    public List<ListObj> stateObjects;

    public List<CountEvent> events;

    [System.Serializable]
    public class ListObj
    {
        public List<GameObject> objs;
    }

    [System.Serializable]
    public class CountEvent
    {
        public int n;
        public UnityEvent<int> OnChanged = new();
    }

    private void Start()
    {
        int n = 0;
        foreach (var obj in stateObjects)
        {
            if (n > 0)
            {
                obj.objs.ForEach(g => g.SetActive(false));
            }
            n++;
        }

        nowState.Subscribe(n =>
        {
            if(n >= 0 && n < stateObjects.Count)
            {
                AllFalse();
                stateObjects[n].objs.ForEach(o => o.gameObject.SetActive(true));
            }
            events.ForEach(e =>
            {
                if (e.n == n) { e.OnChanged?.Invoke(n); }
            });
        });
    }

    public void AllFalse()
    {
        foreach (var obj in stateObjects)
        {
            obj.objs.ForEach(g => g.SetActive(false));
        }
    }

    public void Next()
    {
        nowState += 1;
    }

    public void Back()
    {
        nowState.value -= 1;
    }
}
