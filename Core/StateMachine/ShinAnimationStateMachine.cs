using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;
using Shin_UnityLibrary;

/// <summary>
/// �����ŃX�e�[�g���ړ�����X�e�[�g�}�V��
/// </summary>
public class ShinAnimationStateMachine : ShinStateMachine, IMoveable
{
    [SerializeField] ShinSimpleAnimation simpleAnimation;
    //public bool Additive = false;
    //public AvatarMask avatarMask;

    [Readonly] private ShinAnimationStateMachine[] shinAnimationStateMachines;
    public ShinAnimationStateMachine[] animationStateMachines => shinAnimationStateMachines;

#if UNITY_EDITOR
    [FoldOut("Advanced", true)] public bool enable = false;
    [FoldOut("Advanced")] public bool called = false;
    [FoldOut("Advanced")] public bool checkAutoEnter = false;
    [FoldOut("Advanced")] public bool checkAllStateMachineSelected = false;
    [FoldOut("Advanced")] public bool checkStateDenied = false;
    [FoldOut("Advanced")] public bool checkMachineDenied = false;
#endif

    public void Start()
    {
        shinAnimationStateMachines = GetStateMachines(gameObject);
    }

    /// <summary>
    /// �����𖞂����Ǝ����őJ�ڂ���
    /// </summary>
    public override void Update()
    {
#if UNITY_EDITOR
        if(enable) AdvanceDebug();
#endif

        AutoSetState(); //�����ŃX�e�[�g���؂�ւ��
        base.Update(); //�X�e�[�g�̑J��
    }

    void AutoSetState()
    {
        AutoEnter();
        AutoExit();

        void AutoEnter()
        {
            foreach (var s in states)
            {
                var state = s as ShinAnimateState;
                if (state == null) { continue; }
                var now = nowState as ShinAnimateState;
                if (now == null) { continue; }

                if (CheckEnterAutoState(now, state))
                {
                    SetState(s).Forget();
                }

                if (nowState == s && state.isBreak) goto END;//Break�Ȃ炻�̐�̃X�e�[�g�ɂȂ�Ȃ�
            }
        END:;
        }

        void AutoExit()
        {
            var n = nowState as ShinAnimateState;
            if (n != null && n.isAutoExitCondtion()) //�����𖞂����ƃX�e�[�g���o��
            {
                Exit();
            }
        }
    }

#if UNITY_EDITOR
    void AdvanceDebug()
    {
        var enter = false;

        called = false;
        checkAutoEnter = false;
        checkAllStateMachineSelected = false;
        checkStateDenied = false;
        checkMachineDenied = false;

        foreach (var s in states)
        {
            var state = s as ShinAnimateState;
            if (state == null) { continue; }
            var now = nowState as ShinAnimateState;
            if (now == null) { continue; }

            called = true;

            var enterThisState = state.isInAutoStateCondition();
            if (!enterThisState) { continue; }
            enter |= enterThisState;

            checkAllStateMachineSelected = isEnterFromSelectedListAllStateMachine(state);
            if (!checkAllStateMachineSelected) { continue; }

            checkStateDenied = s.CanEnterState(nowState, s);
            if (!checkStateDenied) { continue; }

            checkMachineDenied = CanSetState(nowState, s);
            if (checkMachineDenied) { continue; }
        }
        checkAutoEnter = enter;
    }
#endif

    /// <summary>
    /// //���ׂẴX�e�[�g�}�V�����Q�Ƃ��A�X�e�[�g�ɓ���邩�ǂ����𔻒肷��
    /// </summary>
    /// <param name="animateState"></param>
    /// <returns></returns>
    public virtual bool CheckEnterAutoState(ShinAnimateState preState, ShinAnimateState nextState)
    {
        if (nextState.isInAutoStateCondition() == false) return false;

        return isEnterFromSelectedListAllStateMachine(nextState);
    }

    /// <summary>
    /// �S�X�e�[�g�}�V���ŃX�e�[�g���I������Ă��邩�ǂ���
    /// </summary>
    /// <param name="nextState"></param>
    /// <returns></returns>
    public virtual bool isEnterFromSelectedListAllStateMachine(ShinAnimateState nextState)
    {
        foreach (var machine in shinAnimationStateMachines)
        {
            var now = machine.nowState;
            if (nextState.isEnterFromSelectedList(now) == false) return false;
        }
        return true;
    }

    /// <summary>
    /// �A�j���[�V�������Đ�
    /// </summary>
    public async UniTask PlayAnim(AnimationClip clip, float speed, float fadeTime = 0.6f)
    {
        //Debug.Log(clip.name+ " _ " + gameObject.name);
        if (clip == null) return;
        await simpleAnimation.CrossFade(clip, fadeTime, speed, default);
    }

    /// <summary>
    /// �X�e�[�g���o�鎞�́A�f�t�H���g�̃X�e�[�g�ɂȂ�
    /// </summary>
    public void Exit()
    {
        SetDefaultState().Forget();
    }

    /// <summary>
    /// �q�I�u�W�F�N�g�̑S�X�e�[�g�}�V�����擾
    /// </summary>
    /// <param name="gameObject"></param>
    /// <returns></returns>
    public static ShinAnimationStateMachine[] GetStateMachines(GameObject gameObject)
    {
        return gameObject.GetComponents<ShinAnimationStateMachine>();
    }

    public static void SetActiveAllAnimateStateMachine(bool active, List<string> tags)
    {
        SetActiveAllComponet<ShinAnimationStateMachine>(active, tags);
    }

    public T GetState<T>() where T : ShinBaseState
    {
        foreach (var machine in GetStateMachines(gameObject))
        {
            foreach (var state in machine.states)
            {
                if (state is T) return (T)state;
            }
        }
        return null;
    }

    public void SetActiveMove(bool b)
    {
        canEnterState = b;
    }
}
