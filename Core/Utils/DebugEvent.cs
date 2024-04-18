using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugEvent : MonoBehaviour
{
    public void DebugLog()
    {
        Debug.Log("Debug");
    }

    public void DebugLog(string message)
    {
        Debug.Log(message);
    }

    public void DebugLog(int message)
    {
        Debug.Log(message);
    }

    public void DebugLog(float message)
    {
        Debug.Log(message);
    }

    public void DebugLog(bool message)
    {
        Debug.Log(message);
    }

    public void DebugLog(Vector2 message)
    {
        Debug.Log(message);
    }

    public void DebugLog(Vector3 message)
    {
        Debug.Log(message);
    }

    public void DebugLog(GameObject gameObject)
    {
        Debug.Log(gameObject);
    }

    public void DebugLog(Transform transform)
    {
        Debug.Log(transform);
    }

    public void DebugLog(Component component)
    {
        Debug.Log(component);
    }
}
