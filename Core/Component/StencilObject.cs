using UnityEngine;
using Shin_UnityLibrary;

public class StencilObject : MonoBehaviour
{
    void Start()
    {
        // Mesh Renderer��MeshFilter�𕡐�����
        MeshRenderer sourceRenderer = gameObject.GetComponent<MeshRenderer>();
        MeshFilter sourceMeshFilter = gameObject.GetComponent<MeshFilter>();

        if (sourceRenderer == null || sourceMeshFilter == null)
        {
            Debug.LogError("Source object must have Mesh Renderer and Mesh Filter components.");
            return;
        }

        // ���������I�u�W�F�N�g�𐶐�
        GameObject cloneObject = new GameObject("Stencil");
        MeshRenderer cloneRenderer = cloneObject.AddComponent<MeshRenderer>(cloneObject);
        MeshFilter cloneMeshFilter = cloneObject.AddComponent<MeshFilter>(cloneObject);

        // ��������Renderer�̃}�e���A����ݒ�i�K�v�ɉ����āj
        cloneRenderer.sharedMaterials = sourceRenderer.sharedMaterials;

        // �e�I�u�W�F�N�g�Ɏq�I�u�W�F�N�g�Ƃ��Ēǉ�
        cloneObject.transform.SetParent(transform);

        // �I�u�W�F�N�g�̈ʒu�Ɖ�]��ݒ�i�K�v�ɉ����āj
        cloneObject.transform.localPosition = Vector3.zero;
        cloneObject.transform.localRotation = Quaternion.identity;
        cloneObject.transform.localScale = Vector3.zero;
    }
}