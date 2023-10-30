using UnityEngine;
using UnityEditor;

public class ReplaceMaterial : EditorWindow
{
    public Material oldMaterial;
    public Material newMaterial;

    [MenuItem("Tools/Shin_UnityLibrary/Replace Material")]
    public static void ShowWindow()
    {
        GetWindow<ReplaceMaterial>("Replace Material");
    }

    private void OnGUI()
    {
        oldMaterial = (Material)EditorGUILayout.ObjectField("Old Material", oldMaterial, typeof(Material), false);
        newMaterial = (Material)EditorGUILayout.ObjectField("New Material", newMaterial, typeof(Material), false);

        if (GUILayout.Button("Replace"))
        {
            ReplaceMaterialsInScene();
        }
    }

    void ReplaceMaterialsInScene()
    {
        if (oldMaterial == null || newMaterial == null)
        {
            Debug.LogWarning("Both Old and New materials need to be set.");
            return;
        }

        Renderer[] renderers = GameObject.FindObjectsOfType<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            Material[] materials = renderer.sharedMaterials;
            for (int i = 0; i < materials.Length; i++)
            {
                if (materials[i] == oldMaterial)
                {
                    materials[i] = newMaterial;
                }
            }
            renderer.sharedMaterials = materials;
        }

        Debug.Log("Material replacement complete.");
    }
}