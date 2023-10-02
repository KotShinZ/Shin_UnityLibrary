using UnityEditor;
[CustomEditor(typeof(SplineCreater))]
[CanEditMultipleObjects]
class SplineWidthEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();

        base.OnInspectorGUI();

        if (EditorGUI.EndChangeCheck())
        {
            EditorApplication.delayCall += () =>
            {
                foreach (var target in targets)
                    ((SplineCreater)target).LoftAllRoads();
            };
        }
    }
}
