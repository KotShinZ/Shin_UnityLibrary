using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class LayerMaskAttribute : PropertyAttribute { }

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(LayerMaskAttribute))]
public class LayerMaskDrawer : PropertyDrawer
{

    public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, prop);

        prop.intValue = EditorGUILayout.MaskField("Layers", prop.intValue, UnityEditorInternal.InternalEditorUtility.layers); ;

        EditorGUI.EndProperty();
    }
}
#endif