using Shin_UnityLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPooler 
{
    public GameObject GetPooledObject();

    public void AddToPool(GameObject obj);

    public void ResetPool();
}
