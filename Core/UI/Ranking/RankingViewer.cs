using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class RankingViewer : MonoBehaviour
{
    public RankingManeger maneger;

    public TextMeshProUGUI text;
    public int n;
    public string defaultText = "0";

    public void Update()
    {
        SetData();
    }

    public virtual void SetData()
    {
        var b = maneger.Get(n - 1, out var ranking);
        if (b) text.text = ranking.data.num.value.ToString();
        else text.text = defaultText;
    }
}
