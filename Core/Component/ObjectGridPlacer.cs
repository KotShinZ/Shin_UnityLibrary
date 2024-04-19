using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class ObjectGridPlacer : MonoBehaviour
{
    public GameObject objectToPlace;
    public Vector3Int size = new Vector3Int(5, 5, 5); // �I�u�W�F�N�g�̐��i���A�����A���s���j
    public Vector3 spacing = new Vector3(2, 2, 2); // �Ԋu
    public Vector3 rotation = Vector3.zero; // ��]
    public Vector3 scale = Vector3.one; // �X�P�[��

    // �O��̔z�u����ۑ�
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
        // �I�u�W�F�N�g�̐��A�Ԋu�A��]�A�X�P�[�����ς�������m�F
        bool propertiesChanged = size != lastSize || spacing != lastSpacing || rotation != lastRotation || scale != lastScale;

        if (propertiesChanged)
        {
            UpdateObjects();
        }

        // ���݂̏�Ԃ�ۑ�
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
       
        // �q�I�u�W�F�N�g�̐������Ғl�ƈقȂ邩�AforceRecreate��true�̏ꍇ�A�Đ���
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
            // �ʒu�A��]�A�X�P�[���̍X�V�݂̂��s��
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