using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputActionAnyKeyEvent : MonoBehaviour
{
    private InputAction _pressAnyKeyAction =
                new InputAction(type: InputActionType.PassThrough, binding: "*/<Button>", interactions: "Press");
    public UnityEvent<InputAction.CallbackContext> OnStarted;

    private void Awake()
    {
        _pressAnyKeyAction.Enable();
        _pressAnyKeyAction.canceled += ctx => OnStarted.Invoke(ctx);
    }
}
