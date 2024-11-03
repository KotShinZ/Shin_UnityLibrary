using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;
using UniRx;
using System;
using System.Linq;

public class GUISceneLoader : MonoBehaviour
{
    public List<KeySceneLoad> StartKeyScene;
    public List<KeySceneLoad> UpdateKeyScene;
    public List<KeySceneLoad> MethodKeyScene;

    public Subject<int> StartAudio = new();
    public Subject<int> UpdateAudio = new();
    public Subject<int> MethodAudio = new();

    public SceneNameEnum loadingScene;
    public SceneNameEnum managerScene;

    public void Reset()
    {
        var enums = Enum.GetNames(typeof(SceneNameEnum));
        if (enums.Contains("NowLoading")) loadingScene = (SceneNameEnum)Enum.Parse(typeof(SceneNameEnum), "NowLoading");
        if (enums.Contains("Manager")) managerScene = (SceneNameEnum)Enum.Parse(typeof(SceneNameEnum), "Manager");
    }

    private async void Start()
    {
        for (int i = 0; i < StartKeyScene.Count; i++)
        {
            var k = StartKeyScene[i];
            LateStartAudio(i);
            await k.LoadTypeScene(loadingScene, managerScene);
        }
    }
    async void LateStartAudio(int i)
    {
        await UniTask.WaitForFixedUpdate();
        StartAudio.OnNext(i);
    }

    private async void Update()
    {
        bool b = false;

        for (int i = 0; i < UpdateKeyScene.Count; i++)
        {
            var k = UpdateKeyScene[i];
            if (Input.GetKeyDown(k.key))
            {
                UpdateAudio.OnNext(i);
                b = true;
                k.flag = true;
            }
        }
        if (b == true)
        {
            foreach (var k in UpdateKeyScene)
            {
                if (k.flag)
                {
                    await SceneLoader.WaitTime(k.delay);
                }
            }
            if(loadingScene != SceneNameEnum.None) await SceneLoader.LoadNoActive(loadingScene);
            foreach (var k in UpdateKeyScene)
            {
                if (k.flag)
                {
                    await k.LoadTypeScene(loadingScene, managerScene);
                }
            }
            if (loadingScene != SceneNameEnum.None) await SceneLoader.Unload(loadingScene);
        }
    }

    public void LoadKeyScene(int num)
    {
        MethodAudio.OnNext(num);
        MethodKeyScene[num].LoadTypeScene(loadingScene, managerScene).Forget();
    }
}

[System.Serializable]
public class KeySceneLoad
{
    public KeyCode key;
    public SceneNameEnum scenes;
    public LoadSceneType type;
    public float delay;

    [HideInInspector] public bool flag;

    public KeySceneLoad(KeyCode key, SceneNameEnum scenes, LoadSceneType type, float delay = 0)
    {
        this.key = key;
        this.scenes = scenes;
        this.type = type;
        this.delay = delay;
    }


    public async UniTask LoadTypeScene(SceneNameEnum loadingScene = SceneNameEnum.None, SceneNameEnum managerScene = SceneNameEnum.None)
    {
        SceneNameEnum sceneName = scenes;
        var activescene = SceneManager.GetActiveScene();

        await SceneLoader.WaitTime(delay);

        switch (type)
        {
            case LoadSceneType.Single:
                await SceneLoader.Load(sceneName, loadingScene, 0, LoadSceneMode.Single);
                break;

            case LoadSceneType.Additive:
                await SceneLoader.Load(sceneName, loadingScene, 0, LoadSceneMode.Additive);
                break;

            case LoadSceneType.Unload:
                await SceneLoader.Unload(sceneName, 0);
                break;

            case LoadSceneType.Replace:
                await SceneLoader.Replace(sceneName, loadingScene, 0, LoadSceneMode.Additive);
                break;

            case LoadSceneType.OnlyManegerSingle:
                await SceneLoader.OnlyScenes(sceneName, managerScene, loadingScene);
                break;

            case LoadSceneType.LoadWaiting:

                break;

            case LoadSceneType.WaitingActivate:

                break;

            case LoadSceneType.AdditiveNoActive:
                await SceneLoader.LoadNoActive(sceneName, 0, LoadSceneMode.Additive);
                if(SceneLoader.IsLoaded(activescene.name)) SceneManager.SetActiveScene(activescene);
                break;
        }
    }
}