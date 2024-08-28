using Shin_UnityLibrary;
using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using UnityEngine;
using UnityEngineInternal;

/// <summary>
/// 自動でするスポーンさせる。
/// </summary>
public abstract class Spawner : SpawnerBase
{
    [Header("Auto Spawn")]
    public bool autoSpawn = true;
    public int initspawnCount = 0;
    public float autoSpawnInterval = 1f;
    public int autoSpawn_MaxCount = 100;
    private float _autoSpawnTimer = 0;
    [Header("Offset")]
    public Vector3 spawnOffset;
    public Vector3 spawnRotation;
    public Vector3 spawnScale = Vector3.one;

    // Start is called before the first frame update

    public virtual void Start()
    {
        for (int i = 0; i < initspawnCount; i++)
        {
            SpawnOnce();
        }
    }

    public virtual void Update()
    {
        AutoSpawn();
    }

    public abstract Vector3 GetSpawnPosition();

    public abstract GameObject GetSpawnPrefab();

    public virtual Quaternion GetQuaternion(Vector3 pos, GameObject prefab)
    {
        return Quaternion.identity;
    }

    public override void SpawnedAction(GameObject ins)
    {
        base.SpawnedAction(ins);
        ins.transform.position += spawnOffset;
        ins.transform.eulerAngles += spawnRotation;
        ins.transform.localScale = new Vector3(ins.transform.localScale.x * spawnScale.x, ins.transform.localScale.y * spawnScale.y, ins.transform.localScale.z * spawnScale.z);
    }

    public void SpawnOnce()
    {
        var pos = GetSpawnPosition();
        var prefab = GetSpawnPrefab();
        var quaternion = GetQuaternion(pos, prefab);
        Spawn(prefab, pos, quaternion);
    }

    /// <summary>
    /// 毎フレーム呼ぶことで、自動でスポーンする
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="quaternion"></param>
    protected void AutoSpawn()
    {
        if (autoSpawn == false) return;

        _autoSpawnTimer += Time.deltaTime;
        if (_autoSpawnTimer > autoSpawnInterval)
        {
            _autoSpawnTimer = 0;
            if (spawnedList.Count < autoSpawn_MaxCount)
            {
                SpawnOnce();
            }
        }
    }
}
