using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSaveData : MonoBehaviour
{
    [SelectFields()] public SetField field = new SetField(typeof(SaveData));

    public void Set()
    {
        Shin_UnityLibrary.SaveManager.instance.SetField(field);
    }

    enum Mode
    {
        Set,
        Add,
    }
}
