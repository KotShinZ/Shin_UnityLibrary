using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shin_UnityLibrary;

[RequireComponent(typeof(CameraOverideTime))]
public class SpawnerWithCamera : SpawnerList
{
    [Space(10)]
    [Header("�J������Ǐ]���邩")]
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

        Utils.ExecuteAllEvent<IMoveable>((m, y) => m.SetActiveMove(false)); //�J�����̈ړ����͓����Ȃ�

        if (isWithCamera) cam.SetTarget(spawnList); //�J�����̃^�[�Q�b�g���Z�b�g
        if(isWithCamera) await cam.SetCamera(); //�J������K�p

        Utils.ExecuteAllEvent<IMoveable>((m, y) => m.SetActiveMove(true));

    }
}
