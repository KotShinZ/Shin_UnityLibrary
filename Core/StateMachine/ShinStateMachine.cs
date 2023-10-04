using Cysharp.Threading.Tasks;
using Shin_UnityLibrary;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using System;
using System.Linq;

/// <summary>
/// �V���v���ȃX�e�[�g�}�V��(�X�e�[�g�̐ݒ肪�o����)
/// </summary>
public class ShinStateMachine : MonoBehaviour
{
    [SerializeField] protected List<ShinBaseState> states;

    [Readonly, SerializeField]protected ShinBaseState nowState;

    private bool stateEnteredThisFrame = false;
    private bool isStateSetting = false; //�X�e�[�g�̑J�ڒ����ǂ���

    public bool canEnterState = true;

    private void Awake()
    {
        SetDefaultState().Forget();
    }

    /// <summary>
    /// ���݂̃X�e�[�g��ݒ�
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public async UniTask SetState(ShinBaseState _state)
    {
        if(CanSetState(nowState , _state))
        {
            SetStateForce(_state).Forget();
        }
    }
    public async UniTask SetState<T>() where T : ShinBaseState
    {
        var _state = states.Find(s => s is T);
        await SetState(_state);
    }
    public async UniTask SetState(Type type)
    {
        var _state = states.Find(s => s.GetType() == type);
        await SetState(_state);
    }
    public void SetState(string _state)
    {
        SetState(Type.GetType(_state)).Forget();
    }
    public void SetState(GameObject _state)
    {
        var type = _state.GetComponent<ShinBaseState>().GetType();
        Debug.Log(type);
        SetState(type);
    }

    /// <summary>
    /// ���݂̃X�e�[�g�������I�ɐݒ�
    /// </summary>
    /// <param name="_state"></param>
    /// <returns></returns>
    public async UniTask SetStateForce(ShinBaseState _state)
    {
        isStateSetting = true; //�X�e�[�g�̐ݒ蒆�ł���B

        if (nowState != null) { await nowState.OnStateExit(this); }  //�X�e�[�g����o��
        nowState = _state; //���݂̃X�e�[�g��ݒ�
        await _state.m_OnStateEnter(this);

        isStateSetting = false;
    }

    /// <summary>
    /// �X�e�[�g���Z�b�g�o���邩�ǂ���
    /// </summary>
    /// <param name="preState"></param>
    /// <param name="nextState"></param>
    public virtual bool CanSetState(ShinBaseState preState, ShinBaseState nextState)
    {
        //await UniTask.WaitUntil(() => stateEnteredThisFrame == false);

        if (stateEnteredThisFrame == true) { return false; }//�X�e�[�g�̈ړ���1�t���[�����̂�
        stateEnteredThisFrame = true;

        if (!canEnterState) return false; //�X�e�[�g�ɓ���Ȃ���Ԃł���

        if(nextState == preState && nextState.canEnterNowState == false) { return false; }
        if (nextState.CanEnterState(preState, nextState))
        {
            return true;
        }
        else return false;
    }


    /// <summary>
    /// ���݂̃X�e�[�g���擾
    /// </summary>
    /// <returns></returns>
    public ShinBaseState GetNowState()
    {
        return nowState;
    }

    /// <summary>
    /// �f�t�H���g�̃X�e�[�g�ɐݒ�
    /// </summary>
    /// <returns></returns>
    public async UniTask SetDefaultState(bool force = true)
    {
        if(force == true) await SetStateForce(states[0]);
        else SetState(states[0]);
    }

    /// <summary>
    /// �X�e�[�g����Update���\�b�h�����s
    /// </summary>
    public virtual void Update()
    {
        if (nowState != null && !isStateSetting) { nowState.OnStateUpdateNoEnterExit(this);}
        if(nowState != null) nowState.OnStateUpdate(this);
        stateEnteredThisFrame = false;
        states.ForEach(state => { state.OnUpdate(); });
    }

    /// <summary>
    /// ���̃I�u�W�F�N�g�̑S�X�e�[�g�}�V�����擾
    /// </summary>
    /// <returns></returns>
    public List<ShinStateMachine> GetAllStateMachineOfThisObject()
    {
        return GetAllStateMachineOfThisObject(this.gameObject);
    }

    public static List<ShinStateMachine> GetAllStateMachineOfThisObject(GameObject obj)
    {
        return obj.GetComponents<ShinStateMachine>().ToList();
    }

    public static void SetActiveAllStateMachine(bool active, List<string> tags)
    {
        SetActiveAllComponet<ShinStateMachine>(active, tags);
    }
    public static void SetActiveAllComponet<T>(bool active, List<string> tags) where T : MonoBehaviour
    {
        var machines = FindObjectsOfType<T>();
        foreach (var machine in machines)
        {
            if (Utils.CompereTag(machine.gameObject, tags)) machine.enabled = active; 
        }
    }
}