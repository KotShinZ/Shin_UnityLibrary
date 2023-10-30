using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public abstract class ShinBaseState : MonoBehaviour
{
    [HideInInspector]public ShinStateMachine thisStateMachine;
    [Label("�X�e�[�g�ɓ��邱�Ƃ��o���邩�ǂ���")]public bool canEnterState = true;
    [Label("���݂̃X�e�[�g�̏�ԂŃX�e�[�g�ɓ���邩�H"), SerializeField] bool canEnterNowState = false;
    [Readonly] public bool isInState = false;

    public IObserver<Type> stateEnterObserver;

    public async UniTask m_OnStateEnter(ShinStateMachine stateMachine) {
        thisStateMachine = stateMachine;
        await OnStateEnter(stateMachine);
    }

    #region ���z���\�b�h

    protected virtual async UniTask OnStateEnter(ShinStateMachine stateMachine){}
    public virtual void OnStateUpdate(ShinStateMachine stateMachine){}
    public virtual void OnStateUpdateNoEnterExit(ShinStateMachine stateMachine){}
    public virtual void OnUpdate() { }
    public virtual async UniTask OnStateExit(ShinStateMachine stateMachine){}

    /// <summary>
    /// �X�e�[�g�ɓ��邱�Ƃ��o���邩�ǂ���
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
    /// ���̃X�e�[�g�ɓ���
    /// </summary>
    public void SetThisState()
    {
        thisStateMachine.SetState(this);
    }

    /// <summary>
    /// ���̃X�e�[�g����o��
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
