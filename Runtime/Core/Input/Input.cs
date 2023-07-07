using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shin_UnityLibrary
{
    public class InputS : MonoBehaviour
    {
        public static bool GetKeyAll(List<KeyCode> keys)
        {
            bool b = false;
            foreach (var k in keys)
            {
                b = b && Input.GetKey(k);
            }
            return b;
        }

        public static bool GetKeyAny(List<KeyCode> keys)
        {
            bool b = false;
            foreach (var k in keys)
            {
                b = b && Input.GetKey(k);
            }
            return b;
        }
    }
}

