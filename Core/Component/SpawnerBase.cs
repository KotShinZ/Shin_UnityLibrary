using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class SpawnerBase : MonoBehaviour
{
    [HideInInspector] public IReadOnlyList<GameObject> spawnedList => _spawnedList;
    [HideInInspector] public List<GameObject> _spawnedList;

    public virtual void SpawnedAction(GameObject ins)
    {

    }

    public virtual GameObject Spawn(GameObject prehub, Vector3 pos)
    {
        var ins = Instantiate(prehub, pos, Quaternion.identity);
        _spawnedList.Add(ins);
        SpawnedAction(ins);
        return ins;
    }

    /// <summary>
    /// スポーンした物の全削除
    /// </summary>
    /// <param name="isStop">生成を止めるかどうか</param>
    public void DeleteAll(bool isStop = true)
    {
        for(int i = 0; i < _spawnedList.Count; i++)
        {
            Destroy(spawnedList[i]);
        }
        _spawnedList = new();
        if(isStop) enabled = false;
    }
}
