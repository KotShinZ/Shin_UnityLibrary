using Cysharp.Threading.Tasks;
using Shin_UnityLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class SpawnerList : SpawnerBase
{
    [TitleDescription] public string title = "Spawn()が呼ばれるとListに入っているオブジェクトを出現させる\nヒエラルキーにあるならその場所に、Prefabならオブジェクトの中心にスポーンされる";

    public List<GameObject> spawnList;
    public List<bool> isPrehub = new();
    public int spawnLimit = 1;
    public float interval = -1;
    public GameObject parent = null;
    

    [FoldOut("AutoSpawn", true)] public bool initSpawn = false;
    [FoldOut("AutoSpawn")] public bool autoSpawn = false;
    [FoldOut("AutoSpawn")] public float autoSpawnInterval = 1;

    [FoldOut("Delay", true)] public float spawnDelayTime = 0.3f;
    [FoldOut("Delay")] public float destroyDelayTime = -1;

    [FoldOut("Reset", true)] public bool positionReset = false;
    [FoldOut("Reset")] public bool rotationReset = false;
    [FoldOut("Reset")] public bool scaleReset = false;

    [FoldOut("Offset", true)] public Vector3 positionOffset = Vector3.zero;
    [FoldOut("Offset")] public Vector3 rotationOffset = Vector3.zero;
    [FoldOut("Offset")] public Vector3 scaleOffset = Vector3.one;

    [FoldOut("Effect", true)] public bool isEffect = false;
    [FoldOut("Effect")] public GameObject effect;
    [FoldOut("Effect")] public Vector3 effectPositionOffset = Vector3.zero;
    [FoldOut("Effect")] public Vector3 effectRotationOffset = Vector3.zero;
    [FoldOut("Effect")] public Vector3 effectScaleOffset = Vector3.one;

    public IReadOnlyList<GameObject> spawnedObjects => spawnList;

    [Space(15)]
    public UnityEvent OnSpawnCalled = new UnityEvent();
    public UnityEvent spawned = new UnityEvent();

    [Readonly]public int spawnCount = 0;
    private float nowtime = 0;

    public virtual void Start()
    {
        for(int i = 0; i < spawnList.Count; i++) {
            if (isPrehub[i] == false) spawnList[i].SetActive(false);
        }
        if(initSpawn) Spawn();
    }

    public void OnValidate()
    {
#if UNITY_EDITOR
        isPrehub = new();
        if (spawnList != null && spawnList.Count > 0)
            for (int i = 0; i < spawnList.Count; i++)
            {
                if (spawnList[i] == null) continue;
                var type = PrefabUtility.GetPrefabType(spawnList[i]);
                isPrehub.Add(type == PrefabType.Prefab ||
                 type == PrefabType.ModelPrefab ||
                 type == PrefabType.PrefabInstance ||
                 type == PrefabType.ModelPrefabInstance ||
                 type == PrefabType.PrefabInstance);
                ;
            }
#endif
    }

    private void Update()
    {
        nowtime += Time.deltaTime;

        if(autoSpawn)
        {
            if(nowtime > autoSpawnInterval)
            {
                Spawn();
                nowtime = 0;
            }
        }
    }

    /// <summary>
    /// これを呼ぶと、登録されているオブジェクトが出現する（オーバーライド出来る）
    /// </summary>
    public virtual void Spawn()
    {
        SpawnObj();
       
    }
    public virtual void SpawnNum(int num)
    {
        for (int i = 0; i < num; i++)
        {
            Spawn();
        }
    }

    /// <summary>
    /// オーバーライドは出来ない
    /// </summary>
    public void SpawnObj()
    {
        if(!CanSpawn()) { return; }

        SpawnTask();
    }

    public bool CanSpawn()
    {
        if (spawnList.Count == 0) return false;
        if (spawnCount >= spawnLimit && spawnLimit != -1) return false;
        if (nowtime <= interval && interval != -1) return false;
        return true;
    }

    /// <summary>
    /// スポーンしたオブジェクトをActiveにする
    /// </summary>
    /// <param name="b"></param>
    public virtual void SetActiveSpawned(bool b = true)
    {
        _spawnedList.ForEach(g => g.SetActive(b));
    }

    /// <summary>
    /// スポーンする
    /// </summary>
    /// <returns></returns>
    public virtual async UniTask SpawnTask()
    {
        OnSpawnCalled?.Invoke();
        if(spawnDelayTime > 0) await UniTask.Delay((int)(spawnDelayTime * 1000));

        int n = 0;
        if (gameObject != null)
        {
            nowtime = 0;
            spawnCount++;
            var inses = SpawnObjects(spawnList, n);

            if (isEffect) EffectSpawn(GetCenterPostion(inses));
            n++;
        }

        List<GameObject> SpawnObjects(List<GameObject> objs, int n)
        {
            List<GameObject> list = new List<GameObject>();
            spawnList.ForEach(obj =>
            {
                if (obj != null)
                {
                    var ins = SpawnObject(obj, n);
                    _spawnedList.Add(ins);
                    list.Add(ins);
                    spawned?.Invoke();
                    if (destroyDelayTime != -1) Utils.DelayDestroy(ins, destroyDelayTime);
                }
            });
            return list ;
        }

        GameObject SpawnObject(GameObject obj, int n)
        {
            GameObject ins;
            if (IsPrefab(obj, n)) { ins = SpawnPrehub(obj); }
            else ins = SpawnObj(obj);
            return ins;
        }


        GameObject SpawnObj(GameObject obj)
        {
            var ins = obj;

            ins.SetActive(true);

            ins.transform.position += positionOffset;
            ins.transform.eulerAngles += rotationOffset;
            ins.transform.localScale = Vector3.Scale(ins.transform.localScale, scaleOffset);

            return ins;
        }

        /// <summary>
        /// プレハブを出現させる
        /// </summary>
        /// <param name="objs"></param>
        GameObject SpawnPrehub(GameObject obj)
        {
            if (obj == null || gameObject == null || gameObject.transform == null) return null;
            var ins = base.Spawn(obj, gameObject.transform.position, gameObject.transform.rotation);
            ins.transform.position += positionOffset;
            ins.transform.eulerAngles += rotationOffset;
            ins.transform.localScale = Vector3.Scale(ins.transform.localScale, scaleOffset);

            ins.SetActive(true);

            return ins;
        }
    }

    /// <summary>
    /// プレハブかどうか
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public bool IsPrefab(Object self, int n)
    {
        return isPrehub[n];
    }

    /// <summary>
    /// エフェクトを出現させる
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public GameObject EffectSpawn(Vector3 position)
    {
        GameObject eff = effect == null ? Resources.Load("DefoltSpawnEffect") as GameObject : effect;
        GameObject ins = null;
       if(eff != null) ins = Instantiate(eff, effectPositionOffset + position, Quaternion.Euler(effectRotationOffset));
        return ins;
    }

    /// <summary>
    /// ゲームオブジェクトたちの中心座標
    /// </summary>
    /// <param name="gameObjects"></param>
    /// <returns></returns>
    public Vector3 GetCenterPostion(List<GameObject> gameObjects)
    {
        if (gameObjects.Count == 0) return Vector3.zero;
        Vector3 center = Vector3.zero;
        foreach (GameObject gameObject in gameObjects)
        {
            center += gameObject.transform.position;
        }
        center /= gameObjects.Count;
        return center;
    }
}
