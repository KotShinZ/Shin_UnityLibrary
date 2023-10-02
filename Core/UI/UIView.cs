using System.Collections;
using System.Collections.Generic;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Shin_UnityLibrary;

public class UIView : MonoBehaviour
{
    [TitleDescription] public string t = "セーブデータをテキストに反映する";
    public Text text;
    public TextMeshProUGUI textPro;

    [SelectFields()] public GetField field = new GetField(typeof(SaveData));

    public void Awake()
    {
        if(text == null)text = GetComponent<Text>();
        if (textPro == null) textPro = GetComponent<TextMeshProUGUI>();
    }

    /// <summary>
    /// テキストを指定する
    /// </summary>
    /// <param name="_text"></param>
    void SetText(string _text)
    {
        if(text != null) text.text = _text;
        if (textPro != null) textPro.text = _text;
    }
    void SetText(int _text) { SetText(_text.ToString()); }
    void SetText(float _text) { SetText(_text.ToString()); }

    bool CanSetText()
    {
        return text != null || textPro != null;
    }

    public void Start()
    {
        if(CanSetText())
        {
            var s = new Subject<string>();
            s.Subscribe(s => SetText(s));
            SaveManager.instance.SubscribeField(field, s);
        }
    }
}
