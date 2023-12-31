using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 存在してはいけないシーンを消す
/// </summary>
public class CleanScene : MonoBehaviour
{
    public SceneNameEnum deleteScene;

    // Update is called once per frame
    void Update()
    {
        if (SceneLoader.IsLoaded(deleteScene.ToString()))
        {
            SceneLoader.Unload(deleteScene).Forget();
        }
    }

}
