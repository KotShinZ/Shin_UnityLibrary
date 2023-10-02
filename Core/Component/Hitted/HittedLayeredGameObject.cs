using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HittedLayeredGameObject : HIttedLayered<Transform>
{
    [TitleDescription]public string title = "当たるとEventを実行する（レイヤーでマスク）";
}
