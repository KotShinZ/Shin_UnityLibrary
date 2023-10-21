using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShinGetItemsBase : ScriptableObject
{
    public IGetItems getItems => this as IGetItems;
}
