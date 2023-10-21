using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHolder : MonoBehaviour
{
    [TitleDescription]
    public string title = "アイテムを持っていて送ることが出来る";

    public ShinGetItemsBase items;

    public void Send(ItemHolder holder)
    {
        holder.items.getItems.Increase(items as IGetItems);
    }

    public void Get(ItemHolder target)
    {
        
    }
}
