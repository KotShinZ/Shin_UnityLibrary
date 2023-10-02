using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManeger<T> : SingletonMonoBehaviour<T> where T : MonoBehaviourWithInit, new()
{
    public List<Canvas> canvases;

    public void SetActiveCanvas(bool b, int n)
    {
        if(canvases != null && canvases[n] != null) { canvases[n].gameObject.SetActive(b); }
        
    }
    public void SetActiveCanvas(bool b, string n)
    {
        if (canvases != null && canvases.Find(c => c.name == n) != null) {
            canvases.Find(c => c.name == n).gameObject.SetActive(b);
        }
    }
}
