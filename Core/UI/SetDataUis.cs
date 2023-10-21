using Shin_UnityLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class SetDataUis : MonoBehaviour
{
    public MonoBehaviour component;
    [SelectFields()] public GetField field;

    [FoldOut("UI")] public List<Text> text;
    [FoldOut("UI")] public List<TextMeshProUGUI> textMeshPro;
    [FoldOut("UI")] public List<Slider> slider;
    [FoldOut("UI")] public List<Slider> sliderMax;

    private void OnValidate()
    {
        if(component != null) {
            field = new GetField(component.GetType()); 
        }
    }

    public void Update()
    {
        var value = field.GetValue(component);
       
    }

    public void SetValue(object obj)
    {
        if(obj.ToString() != null && obj.ToString() != "")
        {
            SetValueString(obj.ToString());
        }
        SetValueFloat((float)obj);
    }

    public void SetValueString(string str)
    {
        text.ForEach(t => { t.text = str; });
        textMeshPro.ForEach(t => { t.text = str; });
    }
    public void SetValueFloat(float f)
    {
        slider.ForEach(s=> s.value = f);
        sliderMax.ForEach(s=> s.maxValue = f);
    }
}
