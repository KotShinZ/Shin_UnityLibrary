using Cysharp.Threading.Tasks;
using Shin_UnityLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class SpawnerList : MonoBehaviour
{
    [TitleDescription] public string title = "Spawn()���Ă΂���List�ɓ����Ă���I�u�W�F�N�g���o��������\n�q�G�����L�[�ɂ���Ȃ炻�̏ꏊ�ɁAPrefab�Ȃ�I�u�W�F�N�g�̒��S�ɃX�|�[�������";

    public List<GameObject> spawnList;
    public int spawnLimit = 1;
    public float interval = -1;
    public GameObject parent = null;

    [FoldOut("Delay", true)] public float spawnDelayTime = 0.3f;
    [FoldOut("Delay")] public float destroyDelayTime = -1;

    [FoldOut("Reset", true)] public bool positionReset = false;
    [FoldOut("Reset")] public bool rotationReset = false;
    [FoldOut("Reset")] public bool scaleReset = false;

    [FoldOut("Offset", true)] public Vector3 positionOffset = Vector3.zero;
    [FoldOut("Offset")] public Vector3 rotationOffset = Vector3.zero;
    [FoldOut("Offset")] public Vector3 scaleOffset = Vector3.one;

    [FoldOut("Effect", true)] public bool isEffect = true;
    [FoldOut("Effect"), Header("null�Ȃ�f�t�H���g�̃G�t�F�N�g�ɂȂ�")] public GameObject effect;
    [FoldOut("Effect")] public Vector3 effectPositionOffset = Vector3.zero;
    [FoldOut("Effect")] public Vector3 effectRotationOffset = Vector3.zero;
    [FoldOut("Effect")] public Vector3 effectScaleOffset = Vector3.one;


    [HideInInspector] public List<GameObject> spawnedList = new();
    public IReadOnlyList<GameObject> spawnedObjects => spawnList;

    [Space(15)]
    public UnityEvent OnSpawnCalled = new UnityEvent();
    public UnityEvent spawned = new UnityEvent();

    [Readonly]public int spawnCount = 0;
    private float nowtime = 0;

    public virtual void Start()
    {
        spawnList.ForEach(g => { if (g != null && !IsPrefab(g)) g.SetActive(false); });
    }
    private void Update()
    {
        nowtime += Time.deltaTime;
    }

    /// <summary>
    /// ������ĂԂƁA�o�^����Ă���I�u�W�F�N�g���o������i�I�[�o�[���C�h�o����j
    /// </summary>
    public virtual void Spawn()
    {
        SpawnObj();
    }

    /// <summary>
    /// �I�[�o�[���C�h�͏o���Ȃ�
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
    /// �X�|�[�������I�u�W�F�N�g��Active�ɂ���
    /// </summary>
    /// <param name="b"></param>
    public virtual void SetActiveSpawned(bool b = true)
    {
        spawnedList.ForEach(g => g.SetActive(b));
    }

    /// <summary>
    /// �X�|�[������
    /// </summary>
    /// <returns></returns>
    public virtual async UniTask SpawnTask()
    {
        OnSpawnCalled.Invoke();
        await UniTask.Delay((int)(spawnDelayTime * 1000));

        if(gameObject != null)
        {
            nowtime = 0;
            spawnCount++;
            var inses = SpawnObjects(spawnList);
            if (isEffect) EffectSpawn(GetCenterPostion(inses));
        }
        

        List<GameObject> SpawnObjects(List<GameObject> objs)
        {
            spawnList.ForEach(obj =>
            {
                if (obj != null)
                {
                    var ins = SpawnObject(obj);
                    spawnedList.Add(ins);
                    spawned.Invoke();
                    if (destroyDelayTime != -1) Utils.DelayDestroy(ins, destroyDelayTime);
                }
            });
            return spawnedList;
        }

        GameObject SpawnObject(GameObject obj)
        {
            GameObject ins;
            if (IsPrefab(obj)) { ins = SpawnPrehub(obj); }
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
        /// �v���n�u���o��������
        /// </summary>
        /// <param name="objs"></param>
        GameObject SpawnPrehub(GameObject obj)
        {
            if(obj == null || gameObject == null || gameObject.transform == null) return null;

            var ins = Instantiate(obj, gameObject.transform.position, gameObject.transform.rotation);

            ins.transform.position += positionOffset;
            ins.transform.eulerAngles += rotationOffset;
            ins.transform.localScale = Vector3.Scale(ins.transform.localScale, scaleOffset);

            ins.SetActive(true);

            return ins;
        }
    }

    /// <summary>
    /// �v���n�u���ǂ���
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public bool IsPrefab(Object self)
    {
        /*var type = PrefabUtility.GetPrefabType(self);

        return
            type == PrefabType.Prefab ||
            type == PrefabType.ModelPrefab ||
            type == PrefabType.PrefabInstance ||
            type == PrefabType.ModelPrefabInstance ||
            type == PrefabType.PrefabInstance
        ;*/
        return false;
    }

    /// <summary>
    /// �G�t�F�N�g���o��������
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public GameObject EffectSpawn(Vector3 position)
    {
        GameObject eff = effect == null ? Resources.Load("DefoltSpawnEffect") as GameObject : effect;
        GameObject ins = Instantiate(eff, effectPositionOffset + position, Quaternion.Euler(effectRotationOffset));
        return ins;
    }

    /// <summary>
    /// �Q�[���I�u�W�F�N�g�����̒��S���W
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
