using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class OffsetControlAsset : ControlPlayableAsset
{
    public Vector3 offset;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        var parent = go.GetComponent<PlayableDirector>().GetGenericBinding(this) as GameObject;
        Debug.Log(go.GetComponent<PlayableDirector>().GetGenericBinding(this));
        if (parent != null)
        {
            graph.GetResolver().SetReferenceValue(sourceGameObject.exposedName, parent);
            sourceGameObject.defaultValue = parent;
            
            Debug.Log(sourceGameObject.Resolve(graph.GetResolver()).name);
        }
        var playable = base.CreatePlayable(graph, go);
        //Debug.Log(sourceGameObject.Resolve(graph.GetResolver()).name);

        return playable;
    }
}
