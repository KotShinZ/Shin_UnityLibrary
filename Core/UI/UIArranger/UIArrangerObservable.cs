using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class UIArrangerObservable : UIArranger
{
    [SelectFields()] public GetField field = new(typeof(SaveData));
    [HideInInspector] public BaseDataInt data;

    [HideInInspector]public int preValue;

    public override void Start()
    {
        base.Start();

        var _data = field.GetValueFromList(Shin_UnityLibrary.SaveManager.instance.saveDatas);
        var data  = _data as BaseDataInt;
        preValue = data.value;

        data.Subscribe(n => {
            OnValueChanged(n);
            if(n> preValue) OnValueAdded(n - preValue, n, preValue);
            if(n< preValue) OnValueDecreased(preValue - n, n, preValue);
            preValue = n;
        });
    }

    public virtual void OnValueChanged(int i){ }
    public virtual void OnValueAdded(int delta , int newValue, int preValue){ }
    public virtual void OnValueDecreased(int delta, int newValue, int preValue) { }
}
