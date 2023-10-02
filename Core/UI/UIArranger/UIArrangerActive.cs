using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIArrangerActive : UIArrangerObservable
{
    public List<string> activeObjectName;

    public override void OnValueChanged(int value)
    {
        foreach (var obj in activeObjectName)
        {
            for (int n = 0; n < generated.Count; n++)
            {
                generated[n].Find(obj).gameObject.SetActive(n < value);
            }
        }
    }
}
