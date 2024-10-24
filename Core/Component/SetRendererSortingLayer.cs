using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRendererSortingLayer : MonoBehaviour
{
    public SortingLayer sortingLayer;
    public int sortingOrder = 0;

    void OnValidate()
    {
        Renderer renderer = GetComponent<Renderer>();
        if(renderer == null)
        {
            return;
        }
        //renderer.sortingLayerName = sortingLayer.name;
        renderer.sortingOrder = sortingOrder;
    }
}
