using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class OffsetControlBehaviour : PrefabControlPlayable
{
    public Transform parent;
    public Vector3 offset;

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        base.OnBehaviourPlay(playable, info);
        prefabInstance.transform.position = prefabInstance.transform.position + offset;
    }
}
