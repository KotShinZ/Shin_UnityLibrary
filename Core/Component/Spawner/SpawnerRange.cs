using Shin_UnityLibrary;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class RangeSpawner : SpawnerBase
{
    public Transform center;
    [MinMaxRange(0, 30)] public MinMax range = new MinMax(0, 30);
    public bool lookat = true;

    /// <summary>
    /// プレイヤーの目の前にスポーン
    /// </summary>
    /// <param name="prehub"></param>
    /// <param name="speed"></param>
    /// <param name="randomize"></param>
    /// <returns></returns>
    public GameObject SpawnForward(GameObject prehub, float randomize = 0, float distance = 15)
    {
        Vector2 vec = new Vector2(center.transform.forward.x, center.transform.forward.z) * distance + new Vector2(center.position.x, center.position.z);
        if (randomize > 0) { vec += new Vector2(UnityEngine.Random.Range(-randomize, randomize), UnityEngine.Random.Range(-randomize, randomize)); }

        var ins = Spawn(prehub, Utils.GetDownPos(vec, new string[] { "Environment" }));
        return ins;
    }

    /// <summary>
    /// 周辺のどこかにスポーン
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="speed"></param>
    /// <returns></returns>
    public GameObject SpawnRange(GameObject obj, float randomize = 0)
    {
        float r = range.randomValue;
        float sita = UnityEngine.Random.Range(0f, 2f) * Mathf.PI;
        Vector2 vec = new Vector2(Mathf.Cos(sita), Mathf.Sin(sita)) * r + new Vector2(center.position.x, center.position.z);
        if (randomize > 0) { vec += new Vector2(UnityEngine.Random.Range(-randomize, randomize), UnityEngine.Random.Range(-randomize, randomize)); }

        return Spawn(obj, Utils.GetDownPos(vec, new string[] { "Environment" }));

    }

    /// <summary>
    /// スポーン
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="pos"></param>
    /// <param name="speed"></param>
    /// <returns></returns>
    public GameObject Spawn(GameObject obj, Vector3 pos)
    {
        var ins = base.Spawn(obj, pos, Quaternion.identity);
        if(lookat) ins.transform.LookAt(center);
        return ins;
    }

}
