using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HittedTagedGameObject : HIttedTaged<Transform>
{
    [TitleDescription] public string title = "当たるとEventを実行する（タグでマスク）";
}
