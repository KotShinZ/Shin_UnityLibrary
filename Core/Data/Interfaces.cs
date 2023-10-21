using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �����邱�Ƃ��o����
/// </summary>
public interface ICountableObject
{
    public int num { get; set; }

    /// <summary>
    /// ���₷���Ƃ��o����
    /// </summary>
    /// <param name="item">���₷�Ώۂ̃A�C�e��</param>
    /// <returns>���₷���Ƃ��o�������ǂ���</returns>
    public bool Increase(ICountableObject item);
    public bool Decrease(ICountableObject item);
}

/// <summary>
/// �A�C�e���������A�󂯓n���A�������o����B
/// </summary>
public interface IGetItems
{
    public IReadOnlyList<IGetItems> items => itemsProtected; 
    protected List<IGetItems> itemsProtected { get; }

    public IGetItems this[string key]
    {
        get { return GetItemFromName(key); }
    }
    public IGetItems this[int num]
    {
        get { return items[num]; }
    }

    /// <summary>
    /// ���O��item����������(�ċA�I��)
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public IGetItems GetItemFromName(string name);

    /// <summary>
    /// �A�C�e���̎c�ʂ𑝂₷�i�Ȃ��Ȃ�V������������j
    /// </summary>
    /// <param name="item"></param>
    /// <param name="oneItem"></param>
    /// <returns></returns>
    public IGetItems Increase(IGetItems item, bool isNewCreate = true, bool oneItem = true);

    /// <summary>
    /// �A�C�e�������炷�i�����𖞂����Ɣj�󂷂�j
    /// </summary>
    /// <param name="item"></param>
    /// <param name="isDestroy"></param>
    /// <param name="oneItem"></param>
    /// <returns></returns>
    public IGetItems Decrease(IGetItems item, bool canDestroy = true, bool oneItem = true);
}