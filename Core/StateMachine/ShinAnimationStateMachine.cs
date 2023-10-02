using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;

/// <summary>
/// �����ŃX�e�[�g���ړ�����X�e�[�g�}�V��
/// </summary>
public class ShinAnimationStateMachine : ShinStateMachine, IMoveable
{
    public ShinSimpleAnimation simpleAnimation;
    //public bool Additive = false;
    //public AvatarMask avatarMask;

    [Readonly] private ShinAnimationStateMachine[] shinAnimationStateMachines;
    public ShinAnimationStateMachine[] animationStateMachines => shinAnimationStateMachines;

    public void Start()
    {
        shinAnimationStateMachines = GetStateMachines(gameObject);
    }

    /// <summary>
    /// �����𖞂����Ǝ����őJ�ڂ���
    /// </summary>
    public override void Update()
    {
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
                var now = s as ShinAnimateState;
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

    /// <summary>
    /// //���ׂẴX�e�[�g�}�V�����Q�Ƃ��A�X�e�[�g�ɓ���邩�ǂ����𔻒肷��
    /// </summary>
    /// <param name="animateState"></param>
    /// <returns></returns>
    public virtual bool CheckEnterAutoState(ShinAnimateState preState, ShinAnimateState nextState)
    {
        if (nextState.isInAutoStateCondition() == false) return false;
        
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
