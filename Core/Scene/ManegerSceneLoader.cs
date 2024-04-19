using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;
using System;
using System.Linq;

public class ManegerSceneLoader : MonoBehaviour
{
    public SceneNameEnum manegerScene = SceneNameEnum.None;
    public bool dontMultipleLoad = true;
   // public bool dontDestroyOnLoad = false;

    void Awake()
    {

        if(! dontMultipleLoad)
        {
            SceneLoader.LoadNoActive(manegerScene).Forget();
        }
        else
        {
            if (!SceneLoader.IsSceneLoaded(manegerScene)) SceneLoader.LoadNoActive(manegerScene).Forget();
        }
        
       
    }

    /*public void Reset()
    {
        var enums = Enum.GetNames(typeof(SceneNameEnum));
        if (enums.Contains("Maneger")) manegerScene = (SceneNameEnum)Enum.Parse(typeof(SceneNameEnum), "Maneger");
    }*/
}
