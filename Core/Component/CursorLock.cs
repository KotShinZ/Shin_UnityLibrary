using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorLock : MonoBehaviour
{
    public bool initLock = false;
    public bool visible = true;
    public bool lockPos = true;

    public void Start()
    {
        if (initLock) Lock();
    }

    // Start is called before the first frame update
    public void Lock()
    {
        if(visible) Cursor.visible = false;
        if(lockPos) Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    public void UnLock()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (visible) Cursor.visible = true;
            if (lockPos) Cursor.lockState = CursorLockMode.None;
        }
    }
}
