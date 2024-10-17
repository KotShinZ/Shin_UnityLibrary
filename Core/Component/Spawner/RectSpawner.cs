using Shin_UnityLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectSpawner :  Spawner
{
    [Header("Prefabs")]
    public List<GameObject> prefabs;

    [Header("Rect")]
    public Transform rect1;
    public Transform rect2;

    /// <summary>
    ///  Rect‚Ì’†‚ÅRandom‚ÈˆÊ’u‚ðŽæ“¾
    /// </summary>
    /// <returns></returns>
    public Vector3 GetRandomRectPosition()
    {
        return new Vector3(Random.Range(rect1.position.x, rect2.position.x), Random.Range(rect1.position.y, rect2.position.y), Random.Range(rect1.position.z, rect2.position.z));
    }

    public override Vector3 GetSpawnPosition()
    {
        return GetRandomRectPosition();
    }

    public override GameObject GetSpawnPrefab()
    {
        return prefabs.GetRandomInList();
    }
}
