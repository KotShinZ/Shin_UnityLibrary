using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;
using System;
using System.Linq;

public class ManegerSceneLoader : MonoBehaviour
{
    private static bool Loaded { get; set; }
    public SceneNameEnum manegerScene = SceneNameEnum.None;

    void Awake()
    {
        if (Loaded) return;

        Loaded = true;

        SceneLoader.LoadNoActive(manegerScene).Forget();
    }

    public void Reset()
    {
        var enums = Enum.GetNames(typeof(SceneNameEnum));
        if (enums.Contains("Maneger")) manegerScene = (SceneNameEnum)Enum.Parse(typeof(SceneNameEnum), "Maneger");
    }
}
