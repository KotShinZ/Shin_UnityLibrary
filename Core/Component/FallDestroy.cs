using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDestroy : MonoBehaviour
{
    [TitleDescription]
    public string title = "落ちた時に自動で自分を破壊する";

    public float fallHeight = -10f;

    // Update is called once per frame
    void Update()
    {
        if(gameObject.transform.position.y < fallHeight)
        {
            Destroy(gameObject);
        }
    }
}
