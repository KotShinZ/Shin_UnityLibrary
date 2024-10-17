using Shin_UnityLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePooler : MonoBehaviour, IPooler
{
    public int poolSize = 20;
    public List<Poolable> pool = new List<Poolable>();

    public virtual GameObject Instantiate(GameObject prefab)
    {
        var ins = GameObject.Instantiate(prefab);
        ins.transform.SetParent(transform);
        ins.GetComponent<Poolable>().prefab = prefab;
        return ins;
    }

    /// <summary>
    /// プレハブからプールを考慮してインスタンスを取得
    /// </summary>
    /// <param name="prefabs"></param>
    /// <returns></returns>
    public GameObject GetInstance(GameObject prefab)
    {
        var ins = GetPooledObject(prefab);
        if (ins == null)
        {
            ins = Instantiate(prefab);
            AddToPool(ins);
            return ins;
        }
        else
        {
            ins.SetActive(true);
            return ins;
        }
    }

    /// <summary>
    /// プールに追加
    /// </summary>
    /// <param name="obj"></param>
    public void AddToPool(GameObject ins)
    {
        Poolable poolable = ins.GetComponent<Poolable>();
        pool.Add(poolable);
    }

    /// <summary>
    /// プールから取得
    /// </summary>
    /// <returns></returns>
    public GameObject GetPooledObject()
    {
        if(pool.Count < poolSize)
        {
            return null;
        }
        else
        {
            return pool.GetRandomInList().gameObject;
        }
    }

    /// <summary>
    /// プールからprefabに対応するオブジェクトを取得
    /// </summary>
    /// <param name="prefab"></param>
    /// <returns></returns>
    public GameObject GetPooledObject(GameObject prefab)
    {
        if (pool.Count < poolSize)
        {
            return null;
        }
        else
        {
            var list = pool.FindAll(x => x.prefab == prefab);
            return list.GetRandomInList().gameObject;
        }
    }

    /// <summary>
    ///  プールをリセット
    /// </summary>
    /// <exception cref="System.NotImplementedException"></exception>
    public void ResetPool()
    {
        for(int i = pool.Count - 1; i >= 0; i--)
        {
            GameObject.Destroy(pool[i].gameObject);
        }
        pool = new List<Poolable>();
    }
}
