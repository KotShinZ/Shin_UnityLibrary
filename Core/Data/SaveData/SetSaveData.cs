using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSaveData : MonoBehaviour
{
    [SelectFields()] public SetField field = new SetField(typeof(SaveData));
    public Mode mode;

    public void Set()
    {
       switch (mode)
        {
            case Mode.Set:
                Shin_UnityLibrary.SaveManager.instance.SetField(field);
                break;
            case Mode.Add:
                var value = Shin_UnityLibrary.SaveManager.instance.GetField(field);
                Shin_UnityLibrary.SaveManager.instance.SetField(field);
                break;
        }
    }

    public enum Mode
    {
        Set,
        Add,
    }
}
