using UnityEngine;
using Shin_UnityLibrary;

public class StencilObject : MonoBehaviour
{
    void Start()
    {
        // Mesh RendererとMeshFilterを複製する
        MeshRenderer sourceRenderer = gameObject.GetComponent<MeshRenderer>();
        MeshFilter sourceMeshFilter = gameObject.GetComponent<MeshFilter>();

        if (sourceRenderer == null || sourceMeshFilter == null)
        {
            Debug.LogError("Source object must have Mesh Renderer and Mesh Filter components.");
            return;
        }

        // 複製したオブジェクトを生成
        GameObject cloneObject = new GameObject("Stencil");
        MeshRenderer cloneRenderer = cloneObject.AddComponent<MeshRenderer>(cloneObject);
        MeshFilter cloneMeshFilter = cloneObject.AddComponent<MeshFilter>(cloneObject);

        // 複製したRendererのマテリアルを設定（必要に応じて）
        cloneRenderer.sharedMaterials = sourceRenderer.sharedMaterials;

        // 親オブジェクトに子オブジェクトとして追加
        cloneObject.transform.SetParent(transform);

        // オブジェクトの位置と回転を設定（必要に応じて）
        cloneObject.transform.localPosition = Vector3.zero;
        cloneObject.transform.localRotation = Quaternion.identity;
        cloneObject.transform.localScale = Vector3.zero;
    }
}