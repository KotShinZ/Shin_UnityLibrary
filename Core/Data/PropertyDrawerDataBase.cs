using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

#if UNITY_EDITOR
public static class PropertyDrawerDatabase
{
    private static Dictionary<System.Type, PropertyDrawer> _drawers = new Dictionary<System.Type, PropertyDrawer>() {
        { typeof(UnityEventWrapper), new UnityEventDrawer() },
    };
    public static IReadOnlyDictionary<System.Type, PropertyDrawer> dictDrawer => _drawers;

}
#endif