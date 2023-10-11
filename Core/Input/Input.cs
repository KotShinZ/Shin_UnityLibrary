using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

namespace Shin_UnityLibrary
{
    public class InputS : MonoBehaviour
    {
        public static bool GetKeyAll(List<KeyCode> keys)
        {
            bool b = false;
            foreach (var k in keys)
            {
                b = b && Input.GetKey(k);
            }
            return b;
        }

        public static bool GetKeyAny(List<KeyCode> keys)
        {
            bool b = false;
            foreach (var k in keys)
            {
                b = b && Input.GetKey(k);
            }
            return b;
        }
    }

    [Serializable]
    public class ToggleBase : IDisposable
    {
        [Readonly]public bool state = false;
        public Action<bool> ToggledAction;
        public Action<bool> ToggleEnabledAction;
        public Action<bool> ToggleDisasbledAction;

        IDisposable task;

        public ToggleBase(Predicate<bool> key)
        {
            task = Observable.EveryUpdate().Subscribe(_=> Toggle(key));
        }
        public virtual void Toggled() { }
        public virtual void ToggleEnabled() { }
        public virtual void ToggleDisasbled() { }

        public void Toggle(Predicate<bool> key)
        {
            if (key.Invoke(state))
            {
                if (state) { 
                    state = false; 
                    Toggled(); 
                    ToggleDisasbled();
                    if (ToggledAction != null) ToggledAction.Invoke(state);
                    if (ToggleDisasbledAction != null) ToggleDisasbledAction.Invoke(state); 
                }
                else {
                    state = true; 
                    Toggled(); 
                    ToggleEnabled();
                    if(ToggledAction != null) ToggledAction.Invoke(state);
                    if (ToggleEnabledAction != null) ToggleEnabledAction.Invoke(state);
                }
            }
        }

        public void Dispose()
        {
            task?.Dispose();
        }
    }
}

