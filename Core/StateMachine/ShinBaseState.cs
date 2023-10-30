using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public abstract class ShinBaseState : MonoBehaviour
{
    [HideInInspector]public ShinStateMachine thisStateMachine;
    [Label("ステートに入ることが出来るかどうか")]public bool canEnterState = true;
    [Label("現在のステートの状態でステートに入れるか？"), SerializeField] bool canEnterNowState = false;
    [Readonly] public bool isInState = false;

    public IObserver<Type> stateEnterObserver;

    public async UniTask m_OnStateEnter(ShinStateMachine stateMachine) {
        thisStateMachine = stateMachine;
        await OnStateEnter(stateMachine);
    }

    #region 仮想メソッド

    protected virtual async UniTask OnStateEnter(ShinStateMachine stateMachine){}
    public virtual void OnStateUpdate(ShinStateMachine stateMachine){}
    public virtual void OnStateUpdateNoEnterExit(ShinStateMachine stateMachine){}
    public virtual void OnUpdate() { }
    public virtual async UniTask OnStateExit(ShinStateMachine stateMachine){}

    /// <summary>
    /// ステートに入ることが出来るかどうか
    /// </summary>
    /// <param name="nowState"></param>
    /// <returns></returns>
    public virtual bool CanEnterState(ShinBaseState preState, ShinBaseState nextState) {
        if (preState == null) return true;
        if (nextState == preState && nextState.canEnterNowState == false) { return false; }
        return canEnterState; 
    }

    #endregion

    /// <summary>
    /// このステートに入る
    /// </summary>
    public void SetThisState()
    {
        thisStateMachine.SetState(this);
    }

    /// <summary>
    /// このステートから出る
    /// </summary>
    /// <param name="stateMachine"></param>
    /// <returns></returns>
    public async UniTask ExitState(ShinStateMachine stateMachine)
    {
        if(stateMachine.GetNowState().Equals(this)) await stateMachine.SetDefaultState();
    }
    public async UniTask ExitState(ShinStateMachine[] stateMachine)
    {
        foreach(var state in stateMachine)
        {
            if (state.GetNowState().Equals(this)) await state.SetDefaultState();
        }
    }

    
}
