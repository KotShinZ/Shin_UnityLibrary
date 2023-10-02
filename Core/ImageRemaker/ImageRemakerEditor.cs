using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ImageRemaker
{
    class ReturnShader
    {
        public static ComputeShader returnshader()
        {
            var shader = (ComputeShader)Resources.Load("Shader/RamakeLibrary");
            return shader;
        }
    }
}

public class ImageRemakerEditor : MonoBehaviour
{
    
}
