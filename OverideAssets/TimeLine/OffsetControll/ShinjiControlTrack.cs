using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
[TrackBindingType(typeof (GameObject))]
[TrackClipType(typeof(OffsetControlAsset), false)]
public class OffsetControlTrack : ControlTrack
{

}
