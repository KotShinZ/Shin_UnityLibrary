using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shin_UnityLibrary;
using Cinemachine;
using UnityEngine.Events;
using System;

[RequireComponent (typeof(CinemachineVirtualCamera))]
public class CameraOverideTime : MonoBehaviour
{
    [TitleDescription] public string title = "カメラを一定時間上書きする";

    CinemachineVirtualCamera camera;

    [Label("カメラが移動し始めるまでの時間")]
    public float delay = 0;
    [Label("カメラが移動したあと待つ時間")]
    public float cameraTime = 3;
    public int cameraPriority = 100;
    public bool isPlayerStop = true;
    [Label("カメラが移動するのにかかる時間")]
    public float cameraBlendTime = 0.3f;

    public UnityEvent OnCameraMoveStart = new UnityEvent();
    public UnityEvent OnCameraTargeted = new UnityEvent();
    public UnityEvent OnCameraEnd = new UnityEvent();
    public Action OnCameraMoveStartAction;
    public Action OnCameraTargetedAction;
    public Action OnCameraEndAction;

    void Start()
    {
        camera = GetComponent<CinemachineVirtualCamera>();
        camera.enabled = false;
        camera.Priority = cameraPriority;
    }

    public async void ActiveCamera(float time = 3)
    {
        SetCamera(time).Forget();
    }

    public async UniTask SetCamera()
    {
        SetCamera(cameraTime);
    }
    public async UniTask SetCamera(float time = 3)
    {
        await Utils.Delay(delay);
        OnCameraMoveStart.Invoke();
        OnCameraMoveStartAction?.Invoke();

        camera.enabled = true;

        if (cameraBlendTime == -1) cameraBlendTime = 0.3f;
        await Utils.Delay(cameraBlendTime);

        OnCameraTargeted.Invoke();
        OnCameraTargetedAction?.Invoke();

        if(time != -1)
        {
            await Utils.Delay(time); //カメラを移動したまま待つ
            camera.enabled = false;
        }
        await Utils.Delay(cameraBlendTime);
        OnCameraEnd.Invoke();
        OnCameraEndAction?.Invoke();
    }

    public void SetTarget(GameObject gameObject)
    {
        camera.Follow = gameObject.transform;
        camera.LookAt = gameObject.transform;
    }

    /// <summary>
    /// 平均の位置にカメラの目標をセット
    /// </summary>
    /// <param name="objects"></param>
    /// <param name="cam"></param>
    public void SetTarget(List<GameObject> objects)
    {
        Vector3 add = Vector3.zero;
        objects.ForEach(o => add += o.transform.position);
        add = add / objects.Count;
        var obj = new GameObject();
        obj.transform.position = add;
        obj.AddComponent<AutoDestroy>();
        SetTarget(obj);
    }

    void SetSpeed()
    {
        if (cameraBlendTime != -1)
        {
            var brain = Camera.main.GetComponent<CinemachineBrain>();
            brain.m_DefaultBlend.m_Time = cameraBlendTime;
        }
        else
        {
            var brain = Camera.main.GetComponent<CinemachineBrain>();
            brain.m_DefaultBlend.m_Time = 0.3f;
        }
    }
}
