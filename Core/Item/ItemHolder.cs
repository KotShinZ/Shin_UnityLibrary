using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHolder : MonoBehaviour
{
    [TitleDescription]
    public string title = "�A�C�e���������Ă��đ��邱�Ƃ��o����";

    public ShinGetItemsBase items;

    public void Send(ItemHolder holder)
    {
        holder.items.getItems.Increase(items as IGetItems);
    }

    public void Get(ItemHolder target)
    {
        
    }
}
