using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shin_UnityLibrary;

[RequireComponent(typeof(CameraOverideTime))]
public class SpawnerWithCamera : SpawnerList
{
    [Space(10)]
    [Header("カメラを追従するか")]
    public bool isWithCamera = true;
    CameraOverideTime cam;

    public override void Start()
    {
        base.Start();
        cam = GetComponent<CameraOverideTime>();
        cam.OnCameraMoveStartAction += ()=> base.Spawn();
    }

    public override async void Spawn()
    {
        if (!CanSpawn()) return;

        Utils.ExecuteAllEvent<IMoveable>((m, y) => m.SetActiveMove(false)); //カメラの移動中は動けない

        if (isWithCamera) cam.SetTarget(spawnList); //カメラのターゲットをセット
        if(isWithCamera) await cam.SetCamera(); //カメラを適用

        Utils.ExecuteAllEvent<IMoveable>((m, y) => m.SetActiveMove(true));

    }
}
