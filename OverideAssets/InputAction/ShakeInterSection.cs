#if Shin_Overide_InputSystem    

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShakeInterSection : IInputInteraction<float>
{
#if UNITY_EDITOR
    [UnityEditor.InitializeOnLoadMethod]
#else
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
#endif
    public static void Initialize()
    {
        // 初回にInteractionを登録する必要がある
        InputSystem.RegisterInteraction<ShakeInterSection>();
    }
    public int deltaPoint = 1;
    public float timeOut = 1;
    public int shakeNum = 2;
    public float shakeInterval = 1;

    private float preValue = 0;
    private float prepreValue = 0;
    private int nowShakeNum = 0;

    private float preShakeTime = 0;

    public void Process(ref InputInteractionContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Waiting:
                if(context.ControlIsActuated(deltaPoint)) //デルタがある程度大きいとスタート
                {
                    context.Started();
                }
               
                break;

            case InputActionPhase.Started:
                context.SetTimeout(timeOut); //一定時間立つとキャンセル
                if (Time.time - preShakeTime > shakeInterval) { context.Canceled(); } //前回のシェイクから一定時間立つとキャンセル
                if (isShake(ref context))
                {
                    context.Performed();
                }
                break;
        }
    }

    bool isShake(ref InputInteractionContext context)
    {
        var nowValue = context.ReadValue<float>();
        var nowDelta = nowValue - preValue;
        var preDelta = preValue - prepreValue;

        //Debug.Log(nowDelta.ToString() + "  , "+ preDelta.ToString() + "  ,  " + (nowDelta * preDelta).ToString());

        if(nowDelta * preDelta < 0)
        {
            //Debug.Log(nowShakeNum);
            nowShakeNum += 1;　//キャンセルされるまでシェイクを数える
            preShakeTime = Time.time; //前回のシェイクの時間
        }

        prepreValue = preValue;
        preValue = nowValue;

        if (nowShakeNum >= shakeNum) return true; //指定回数シェイクするとPerform

        return false;
    }
    
    public void Reset()
    {
        preShakeTime = Time.time;
        nowShakeNum = 0;
        preValue = 0;
        prepreValue = 0;
    }
}

#endif