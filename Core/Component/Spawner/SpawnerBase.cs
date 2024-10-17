using Shin_UnityLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// スポーンした物の管理をする
/// </summary>
public class SpawnerBase : MonoBehaviour
{

    public bool canSpawn = true;
    public IReadOnlyList<GameObject> spawnedList => _spawnedList;
    [Readonly]public List<GameObject> _spawnedList;

    public UnityEvent<GameObject> onSpawned = new();

    public SimplePooler pooler;

    public virtual void Awake()
    {
        _spawnedList = new();
    }

    public virtual void SpawnedAction(GameObject ins)
    {

    }

    public virtual GameObject Spawn(GameObject prehub, Vector3 pos, Quaternion quaternion)
    {
        if (canSpawn)
        {
            GameObject ins = null;
            if(pooler != null)
            {
                ins = pooler.GetInstance(prehub); // プールから取得
                ins.transform.position = pos;
                ins.transform.rotation = quaternion;
            }
            if(ins == null)
            {
                ins = Instantiate(prehub, pos, quaternion); // プールから取得できなかったら新規生成
            }

            ins.transform.parent = this.transform;
            if(ins.TryGetComponent(out Spawnable spawnable))
            {
                spawnable.parent = this;
            }
            _spawnedList.Add(ins);
            SpawnedAction(ins);
            onSpawned.Invoke(ins);
            return ins;
        }
        return null;
    }

    #region Delete

    /// <summary>
    /// スポーンした物の全削除
    /// </summary>
    /// <param name="isGenerateStop">生成を止めるかどうか</param>
    public void DeleteAll(bool isGenerateStop = true)
    {
        int count = _spawnedList.Count;
        for (int i = 0; i < count; i++)
        {
            if (_spawnedList[i] != null)
            {
                Destroy(_spawnedList[i]);
            }
        }
        _spawnedList = new();
        if (isGenerateStop) enabled = false;
    }
    public void DeleteAllSpawner(bool isGenerateStop)
    {
        DeleteAllSpawnerInstance(isGenerateStop);
    }

    public static void DeleteAllSpawnerInstance(bool isGenerateStop)
    {
        var spawners = FindObjectsByType<SpawnerBase>(sortMode: FindObjectsSortMode.None);
        foreach (var s in spawners)
        {
            s.DeleteAll(isGenerateStop);
        }
    }
    public static void DeleteAllSpawnerInstance<T>(bool isGenerateStop) where T : SpawnerBase
    {
        var spawners = FindObjectsByType<T>(sortMode: FindObjectsSortMode.None);
        foreach (var s in spawners)
        {
            s.DeleteAll(isGenerateStop);
        }
    }

    #endregion
}
