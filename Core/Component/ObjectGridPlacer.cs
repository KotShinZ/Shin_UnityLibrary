using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class ObjectGridPlacer : MonoBehaviour
{
    public GameObject objectToPlace;
    public Vector3Int size = new Vector3Int(5, 5, 5); // オブジェクトの数（幅、高さ、奥行き）
    public Vector3 spacing = new Vector3(2, 2, 2); // 間隔
    public Vector3 rotation = Vector3.zero; // 回転
    public Vector3 scale = Vector3.one; // スケール

    // 前回の配置情報を保存
    private Vector3Int lastSize;
    private Vector3 lastSpacing, lastRotation, lastScale;

    public List<GameObject> instantiated;
    public UnityEvent<GameObject> OnInstantiate;

    public bool updateAtStart = true;

    void Start()
    {
        if(updateAtStart)UpdateObjects(true);
    }

    public bool updateAnyway = false;

    void Update()
    {
        if (updateAnyway)
        {
            UpdateObjectProperties();
        }
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        // オブジェクトの数、間隔、回転、スケールが変わったか確認
        bool propertiesChanged = size != lastSize || spacing != lastSpacing || rotation != lastRotation || scale != lastScale;

        if (propertiesChanged)
        {
            UpdateObjects();
        }

        // 現在の状態を保存
        lastSize = size;
        lastSpacing = spacing;
        lastRotation = rotation;
        lastScale = scale;
    }
#endif

    [ContextMenu("Update")]
    void Updates()
    {
        UpdateObjects(true);
    }

    void UpdateObjects(bool force = false)
    {
       
        // 子オブジェクトの数が期待値と異なるか、forceRecreateがtrueの場合、再生成
        if (transform.childCount != size.x * size.y * size.z || force)
        {
            foreach (Transform child in transform)
            {
#if UNITY_EDITOR
                if (EditorApplication.isPlaying == false)
                {
                    GameObject.DestroyImmediate(child.gameObject);
                }
                else
#endif
                {
                    GameObject.Destroy(child.gameObject);
                }
            }
            PlaceObjects();
        }
        else
        {
            // 位置、回転、スケールの更新のみを行う
            UpdateObjectProperties();
        }
    }

    void PlaceObjects()
    {
        instantiated = new();
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                for (int z = 0; z < size.z; z++)
                {
                    Vector3 position = new Vector3(x * spacing.x, y * spacing.y, z * spacing.z) + transform.position;
                    GameObject newObj = Instantiate(objectToPlace, position, Quaternion.Euler(rotation), transform);
                    newObj.transform.localScale = scale;
                    instantiated.Add(newObj);
                    OnInstantiate.Invoke(newObj);
                }
            }
        }
    }

    void UpdateObjectProperties()
    {
        int count = 0;
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                for (int z = 0; z < size.z; z++)
                {
                    if (count < transform.childCount)
                    {
                        Transform child = transform.GetChild(count);
                        child.localPosition = new Vector3(x * spacing.x, y * spacing.y, z * spacing.z);
                        child.localRotation = Quaternion.Euler(rotation);
                        child.localScale = scale;
                        count++;
                    }
                }
            }
        }
    }
}