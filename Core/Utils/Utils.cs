using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShinjiGeneral
{
    public class Utils
    {
        public static Vector3 GetCameraForward()
        {
            var v = Camera.main.transform.forward;
            v.y = 0;
            return v.normalized;
        }
    }
}

