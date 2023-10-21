using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 数えることが出来る
/// </summary>
public interface ICountableObject
{
    public int num { get; set; }

    /// <summary>
    /// 増やすことが出来る
    /// </summary>
    /// <param name="item">増やす対象のアイテム</param>
    /// <returns>増やすことが出来たかどうか</returns>
    public bool Increase(ICountableObject item);
    public bool Decrease(ICountableObject item);
}

/// <summary>
/// アイテムを持ち、受け渡し、検索が出来る。
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
    /// 名前でitemを検索する(再帰的に)
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public IGetItems GetItemFromName(string name);

    /// <summary>
    /// アイテムの残量を増やす（ないなら新しく生成する）
    /// </summary>
    /// <param name="item"></param>
    /// <param name="oneItem"></param>
    /// <returns></returns>
    public IGetItems Increase(IGetItems item, bool isNewCreate = true, bool oneItem = true);

    /// <summary>
    /// アイテムを減らす（条件を満たすと破壊する）
    /// </summary>
    /// <param name="item"></param>
    /// <param name="isDestroy"></param>
    /// <param name="oneItem"></param>
    /// <returns></returns>
    public IGetItems Decrease(IGetItems item, bool canDestroy = true, bool oneItem = true);
}