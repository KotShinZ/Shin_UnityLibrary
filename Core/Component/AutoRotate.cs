using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotate : MonoBehaviour
{
    [TitleDescription("一定速度回転させ続ける")]
    public int i;
    public Vector3 rotateSpeed = new Vector3(0,10,0);

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotateSpeed);
    }
}
