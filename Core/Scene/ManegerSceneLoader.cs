using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

public class ManegerSceneLoader : MonoBehaviour
{
    private static bool Loaded { get; set; }

    void Awake()
    {
        if (Loaded) return;

        Loaded = true;

        SceneLoader.LoadNoActive(SceneNameEnum.Maneger).Forget();
    }
}
