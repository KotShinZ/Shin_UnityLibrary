using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDestroy : MonoBehaviour
{
    [TitleDescription]
    public string title = "—Ž‚¿‚½Žž‚ÉŽ©“®‚ÅŽ©•ª‚ð”j‰ó‚·‚é";
    public HeightObject heightObject;

    // Update is called once per frame
    void Update()
    {
        if(gameObject.transform.position.y < heightObject.height)
        {
            Destroy(gameObject);
        }
    }
}
