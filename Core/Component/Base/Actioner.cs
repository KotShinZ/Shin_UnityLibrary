using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Actioner : MonoBehaviour
{
    public UnityEvent onAction;

    public virtual void Action()
    {
        onAction.Invoke();
    }
}
