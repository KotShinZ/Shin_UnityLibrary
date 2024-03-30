using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Scene = UnityEngine.SceneManagement.Scene;

public static class SceneLoader
{
    #region SubSceneMethods

    public static async UniTask WaitTime(float t)
    {
        await UniTask.Delay(Mathf.RoundToInt(t * 1000));
    }

    public static SceneNameEnum GetActiveScenes()
    {
        return GetActiveScenes(SceneManager.GetActiveScene().name);
    }
    public static void SetActiveScenes(SceneNameEnum scenes)
    {
        SetActiveSceneSafe(SceneManager.GetSceneByName(scenes.ToString()));
    }
    public static void SetActiveSceneSafe(Scene s)
    {
        /*if(GetActiveScenes() == SceneNameEnum.Maneger || GetActiveScenes() == SceneNameEnum.NowLoading)
        {
             if (IsLoaded(SceneNameEnum.Game.ToString()) == true)
            {
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(SceneNameEnum.Game.ToString()));
            }
        }*/

        if (s != null && s.name != null )
        {
            SceneManager.SetActiveScene(s);
        }
    }
    public static bool IsLoaded(string sceneName)
    {
        var sceneCount = SceneManager.sceneCount;

        for (var i = 0; i < sceneCount; i++)
        {
            var scene = SceneManager.GetSceneAt(i);

            if (scene.name == sceneName && scene.isLoaded) return true;
        }

        return false;
    }

    public static SceneNameEnum GetActiveScenes(string str)
    {
        SceneNameEnum s;
        Enum.TryParse(str, out s);
        return s;
    }

    #endregion

    #region LoadMethod

    public static AsyncOperation UnloadOperation()
    {
        return UnloadOperation(GetActiveScenes());
    }
    public static AsyncOperation UnloadOperation(SceneNameEnum scenes)
    {
        Resources.UnloadUnusedAssets();
        Debug.Log(scenes.ToString());
        return SceneManager.UnloadSceneAsync(scenes.ToString());
    }
    public static async UniTask Unload(SceneNameEnum scenes, float t = 0)
    {
        var s = SceneManager.GetActiveScene();
        await WaitTime(t);
        await UnloadOperation(scenes);
        if(s.name != scenes.ToString())
        {
            SetActiveSceneSafe(s);
        }
    }

    public static bool IsSceneLoaded(SceneNameEnum scene)
    {
        var _scene = SceneManager.GetSceneByName(scene.ToString());
        return _scene != null && _scene.isLoaded;
    }

    public static async UniTask WaitSceneLoaded(SceneNameEnum scene)
    {
        await UniTask.WaitUntil(() => IsSceneLoaded(scene) == true);
    }

    /// <summary>
    /// ローディングをはさむ
    /// </summary>
    /// <param name="loadingScene"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static async UniTask LoadingScene(SceneNameEnum loadingScene,  Func<UniTask> action)
    {
        if(loadingScene != SceneNameEnum.None) await LoadNoActive(loadingScene);

        await action();

        if (loadingScene != SceneNameEnum.None)
        {
            await Unload(loadingScene);
        }
    }

    public static async UniTask Load(SceneNameEnum scenes, SceneNameEnum loadingScene = SceneNameEnum.None, float t = 0, LoadSceneMode loadSceneMode = LoadSceneMode.Additive)
    {
        await LoadingScene(loadingScene, async () =>
        {
            await WaitTime(t);
            await SceneManager.LoadSceneAsync(scenes.ToString(), loadSceneMode);
            SetActiveSceneSafe(SceneManager.GetSceneByName(scenes.ToString()));
            Shin_UnityLibrary.SaveManager.instance.Save();
        });
    }
    public static async UniTask LoadNoActive(SceneNameEnum scenes, float t = 0, LoadSceneMode loadSceneMode = LoadSceneMode.Additive)
    {
        var s = SceneManager.GetActiveScene();
        await SceneManager.LoadSceneAsync(scenes.ToString(), loadSceneMode);
        SetActiveSceneSafe(s);
    }

    public static async UniTask Replace(SceneNameEnum scenes, SceneNameEnum loadingScene = SceneNameEnum.None, float t = 0, LoadSceneMode loadSceneMode = LoadSceneMode.Additive)
    {
        await LoadingScene(loadingScene, async () =>
        {
            await WaitTime(t);
            await Unload(GetActiveScenes());
            await SceneManager.LoadSceneAsync(scenes.ToString(), loadSceneMode);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(scenes.ToString()));
        });
    }

    /// <summary>
    /// あるシーンだけ残してをシーンをロードする
    /// </summary>
    /// <param name="scenes"></param>
    /// <param name="isNowLoading"></param>
    /// <param name="t"></param>
    /// <param name="loadSceneMode"></param>
    /// <returns></returns>
    public static async UniTask OnlyScenes(SceneNameEnum scenes, List<SceneNameEnum> onlyScenes, SceneNameEnum loadingScene = SceneNameEnum.None, float t = 0, LoadSceneMode loadSceneMode = LoadSceneMode.Additive)
    {
        await WaitTime(t);

        await LoadingScene(loadingScene, async () =>
        {
            foreach (var scene in SceneManager.GetAllScenes()) //マネージャーScene以外Unload
            {
                var sc = GetActiveScenes(scene.name);

                if(loadingScene != SceneNameEnum.None && sc == loadingScene) { continue; } //Loadingはアンロードしない
                //Debug.Log(sc.ToString());
                if (!onlyScenes.Contains(sc))
                {
                    //Debug.Log(sc.ToString() + "_______");
                    if(sc != SceneNameEnum.None) await Unload(sc);
                }
            }
            await Load(scenes, loadingScene: SceneNameEnum.None, 0, LoadSceneMode.Additive);
        });
        SetActiveScenes(scenes);
    }
    public static async UniTask OnlyScenes(SceneNameEnum scenes, SceneNameEnum onlyScene, SceneNameEnum loadingScene = SceneNameEnum.None, float t = 0, LoadSceneMode loadSceneMode = LoadSceneMode.Additive)
    {
        await OnlyScenes(scenes, new List<SceneNameEnum>() { onlyScene }, loadingScene, t, loadSceneMode);
    }
    #endregion
}

public enum LoadSceneType
{
    Single = 0,
    Additive = 1,
    Unload,
    Replace,
    OnlyManegerSingle,
    LoadWaiting,
    WaitingActivate,
    AdditiveNoActive
}

public enum SceneNameEnumf
{
    This,
    Game,
    Maneger,
    Battle,
    GameOver,
    GameClear,
    GameEnd,
    Quit,
    Title,
    ReStart,
    NowLoading,
    PlayerUI,
    StatusUI,
    StageSelect
}