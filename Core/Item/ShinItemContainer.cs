using Codice.Client.Common.FsNodeReaders;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

[System.Serializable, CreateAssetMenu(fileName = "ItemData", menuName = "Shin_UnityLibrary/ScriptableObjects/ItemHolder")]
public class ShinItemContainer : ShinGetItemsBase, IGetItems
{
    List<IGetItems> IGetItems.itemsProtected => thisItems;
    List<IGetItems> thisItems => (List<IGetItems>)items.Cast<IGetItems>();
    [SerializeField, EnforceInterface(typeof(IGetItems))] List<ShinGetItemsBase> items = new();

    /// <summary>
    /// ���O��item����������(�ċA�I��)
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public virtual IGetItems GetItemFromName(string name)
    {
        foreach (var i in thisItems)
        {
            var child = i.GetItemFromName(name);
            if (child != null) return child;
        }
        return null;
    }

    /// <summary>
    /// �A�C�e���̎c�ʂ𑝂₷�i�Ȃ��Ȃ�V������������j
    /// </summary>
    /// <param name="item"></param>
    /// <param name="oneItem"></param>
    /// <returns></returns>
    public virtual IGetItems Increase(IGetItems item, bool isNewCreate = true, bool oneItem = true)
    {
        bool b = false;

        foreach (var i in item.items)
        {
            b = i.Increase(item, false, oneItem) == null ? true : b;
            if (b && oneItem) return i;
        }
        if (isNewCreate && b == false)
        {
            items.Add(item as ShinGetItemsBase);
        }
        return null;
    }

    /// <summary>
    /// �A�C�e�������炷�i�����𖞂����Ɣj�󂷂�j
    /// </summary>
    /// <param name="item"></param>
    /// <param name="isDestroy">�����ŃA�C�e���������邩�ǂ���</param>
    /// <param name="oneItem">��̃A�C�e���݂̂�ΏۂƂ��邩</param>
    /// <returns></returns>
    public virtual IGetItems Decrease(IGetItems item, bool canDestroy = true, bool oneItem = true)
    {
        bool b = false;
        List<int> removeList = new();

        int n = 0;
        foreach (var i in item.items)
        {
            b = i.Decrease(item, false, oneItem) == null ? true : b;
            if (canDestroy && b && isDestroy())
            {
                removeList.Add(n);
            }
            if (oneItem == false && b == true) return i;
            n++;
        }

        if (removeList.Count > 0 && canDestroy) //�����A�C�e�����폜
        {
            foreach (var k in removeList)
            {
                thisItems.RemoveAt(k);
            }
        }
        return null;
    }

    /// <summary>
    /// ���g���j�󂳂�����
    /// </summary>
    /// <returns></returns>
    public virtual bool isDestroy()
    {
        return thisItems.Count == 0;
    }
}
