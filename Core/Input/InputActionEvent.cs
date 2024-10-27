using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputActionEvent : MonoBehaviour
{
    public InputAction action;
    [Space(10)]
    public UnityEvent<InputAction.CallbackContext> started;
    public UnityEvent<InputAction.CallbackContext> performed;
    public UnityEvent<InputAction.CallbackContext> canceled;

    private void Awake()
    {
        action.Enable();
        action.started += ctx => started.Invoke(ctx);
        action.performed += ctx => performed.Invoke(ctx);
        action.canceled += ctx => canceled.Invoke(ctx);
    }
}
