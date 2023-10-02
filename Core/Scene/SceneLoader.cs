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
        if(GetActiveScenes() == SceneNameEnum.Maneger || GetActiveScenes() == SceneNameEnum.NowLoading)
        {
            /* if (IsLoaded(SceneNameEnum.Game.ToString()) == true)
            {
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(SceneNameEnum.Game.ToString()));
            }*/
        }
        if (s.name != null && IsLoaded(s.ToString()) == true)
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
        Enum.TryParse<SceneNameEnum>(str, out s);
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

    public static async UniTask Load(SceneNameEnum scenes, bool isNowLoading = true, float t = 0, LoadSceneMode loadSceneMode = LoadSceneMode.Additive)
    {
        if (isNowLoading) await LoadNoActive(SceneNameEnum.NowLoading);

        await WaitTime(t);
        await SceneManager.LoadSceneAsync(scenes.ToString(), loadSceneMode);
        SetActiveSceneSafe(SceneManager.GetSceneByName(scenes.ToString()));
        Shin_UnityLibrary.SaveManager.instance.Save();

        if (isNowLoading) await Unload(SceneNameEnum.NowLoading);
    }
    public static async UniTask LoadNoActive(SceneNameEnum scenes, float t = 0, LoadSceneMode loadSceneMode = LoadSceneMode.Additive)
    {
        var s = SceneManager.GetActiveScene();
        await SceneManager.LoadSceneAsync(scenes.ToString(), loadSceneMode);
        SetActiveSceneSafe(s);
    }

    public static async UniTask Replace(SceneNameEnum scenes, bool isNowLoading = true, float t = 0, LoadSceneMode loadSceneMode = LoadSceneMode.Additive)
    {
        if (isNowLoading) await LoadNoActive(SceneNameEnum.NowLoading);

        await WaitTime(t);
        await Unload(GetActiveScenes());
        await SceneManager.LoadSceneAsync(scenes.ToString(), loadSceneMode);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(scenes.ToString()));

        if (isNowLoading) await Unload(SceneNameEnum.NowLoading);
    }

    public static async UniTask OnlyManeger(SceneNameEnum scenes, bool isNowLoading = true, float t = 0, LoadSceneMode loadSceneMode = LoadSceneMode.Additive)
    {
        await WaitTime(t);
        await LoadNoActive(SceneNameEnum.NowLoading);

        List<string> str = new List<string>();
        foreach (var scene in SceneManager.GetAllScenes())
        {
            var sc = GetActiveScenes(scene.name);
            if (!(sc == SceneNameEnum.Maneger || sc == SceneNameEnum.NowLoading))
            {
                Debug.Log(sc.ToString());
                await Unload(sc);
            }
        }

        await Load(scenes, false, 0, LoadSceneMode.Additive);

        await Unload(SceneNameEnum.NowLoading);
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