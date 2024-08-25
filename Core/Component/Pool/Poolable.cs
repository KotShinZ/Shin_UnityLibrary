using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Poolable : MonoBehaviour
{
    public GameObject prefab;

    public UnityEvent onInstantiated = new();
    public UnityEvent deleted = new();
}
