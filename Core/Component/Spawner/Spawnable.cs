using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnable : MonoBehaviour
{
    public SpawnerBase parent;
    public bool isvisible = true;

    public void OnBecameInvisible()
    {
        if (isvisible)
        {
            parent?._spawnedList?.Remove(gameObject);
            isvisible = false;
        }
    }

    public void OnBecameVisible()
    {
        if(isvisible == false)
        {
            parent?._spawnedList?.Add(gameObject);
            isvisible = true;
        }
    }
}
