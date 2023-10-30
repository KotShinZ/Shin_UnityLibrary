using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Events;
using Shin_UnityLibrary;

public abstract class ShinAnimateState : ShinBaseState //ステートにアニメーション切り替えを同期させる
{
    [FoldOut("Animation", true)] public AnimationClip preAnimation = null;
    [FoldOut("Animation")] public AnimationClip thisAnimation;
    [FoldOut("Animation")] public AnimationClip endAnimation = null;
    [FoldOut("Animation", space = 10)] public float preAnimationSpead = 1f;
    [FoldOut("Animation")] public float thisAnimationSpead = 1f;
    [FoldOut("Animation")] public float endAnimationSpead = 1f;

    [Space(10)]
    [SelectDerivedClass("選択されたステートからのみ、このステートに遷移可能")]
    public DerivedClass selectedAnimations = null;

    [Label("優先度が低いステートに進むことが出来るか？")] public bool isBreak = false;

    public UnityEvent onAnimateEnter = new();
    public UnityEvent onAnimateStay = new();
    public UnityEvent onAnimateExit = new();

    UniTask animationTask;

    /// <summary>
    /// ステートマシンをアニメーションステートマシンにする
    /// </summary>
    /// <param name="shinStateMachine"></param>
    /// <returns></returns>
    public ShinAnimationStateMachine ResolveAnimate(ShinStateMachine shinStateMachine)
    {
        if (shinStateMachine is ShinAnimationStateMachine)
        {
            return shinStateMachine as ShinAnimationStateMachine;
        }
        else
        {
            Debug.LogError("AnimationState is in AnimationMachine!!");
            Application.Quit();
            return null;
        }
    }

    #region　仮想メソッド

    /// <summary>
    /// 継承して決めるステートに入る事の出来る条件
    /// </summary>
    /// <returns></returns>
    public virtual bool isInAutoStateCondition()
    {
        return false;
    }

    /// <summary>
    /// 継承して決めるステートをでなければならない条件
    /// </summary>
    /// <returns></returns>
    public virtual bool isAutoExitCondtion()
    {
        return false;
    }

    #endregion

    #region オーバーライド

    protected override sealed async UniTask OnStateEnter(ShinStateMachine _stateMachine)
    {
        isInState = true;
        var stateMachine = ResolveAnimate(_stateMachine);

        #region PreAnimation

        await OnPreAnimateEnter(stateMachine);
        if (preAnimation != null)
        {
            var preTask = stateMachine.PlayAnim(preAnimation, preAnimationSpead);
            await foreach (var progress in UniTaskAsyncEnumerable.EveryUpdate())
            {
                // タスクが完了したらループを抜ける
                if (preTask.Status.IsCompleted())
                    break;

                // フレームごとの処理
                OnPreAnimateUpdate(stateMachine);
            }
        }
        await OnPreAnimateExit(stateMachine);

        #endregion

        #region ThisAnimation
        if (thisAnimation != null)
        {
            animationTask = stateMachine.PlayAnim(thisAnimation, thisAnimationSpead);
            if (thisAnimation.isLooping == false)
            {
                animationTask.ContinueWith(() => stateMachine.Exit()).Forget();
            }
        }
        #endregion

        onAnimateEnter.Invoke();
        await OnAnimateEnter(stateMachine);
    }
    public override sealed void OnStateUpdate(ShinStateMachine _stateMachine)
    {
        var stateMachine = ResolveAnimate(_stateMachine);
        onAnimateStay.Invoke();
        OnAnimateUpdate(stateMachine);
    }
    public override sealed void OnStateUpdateNoEnterExit(ShinStateMachine _stateMachine)
    {
        var stateMachine = ResolveAnimate(_stateMachine);
        OnAnimateUpdateNoEnterExit(stateMachine);
    }

    public override sealed async UniTask OnStateExit(ShinStateMachine _stateMachine)
    {
        var stateMachine = ResolveAnimate(_stateMachine);
        onAnimateExit.Invoke();
        await OnAnimateExit(stateMachine);
        isInState = false;

        #region EndAnimation

        await OnEndAnimateEnter(stateMachine);
        if (endAnimation != null)
        {
            var endTask = stateMachine.PlayAnim(endAnimation, endAnimationSpead);
            await foreach (var progress in UniTaskAsyncEnumerable.EveryUpdate())
            {
                // タスクが完了したらループを抜ける
                if (endTask.Status.IsCompleted())
                    break;

                // フレームごとの処理
                OnEndAnimateUpdate(stateMachine);
            }
        }
        await OnEndAnimateExit(stateMachine);

        #endregion
    }

    /// <summary>
    /// ステートに入ることが出来るかどうか
    /// </summary>
    /// <param name="nowState"></param>
    /// <returns></returns>
    public sealed override bool CanEnterState(ShinBaseState preState, ShinBaseState nextState)
    {
        if (isEnterFromSelectedList(preState) == false) return false;  //入る前のステートが選択されているかどうか？

        return base.CanEnterState(preState, nextState);
    }

    #endregion

    #region アニメーションステート
    public virtual async UniTask OnAnimateEnter(ShinAnimationStateMachine stateMachine) { }
    public virtual void OnAnimateUpdate(ShinAnimationStateMachine stateMachine) { }
    public virtual void OnAnimateUpdateNoEnterExit(ShinAnimationStateMachine stateMachine) { }
    public virtual async UniTask OnAnimateExit(ShinAnimationStateMachine stateMachine) { }

    public virtual async UniTask OnPreAnimateEnter(ShinAnimationStateMachine stateMachine) { }
    public virtual void OnPreAnimateUpdate(ShinAnimationStateMachine stateMachine) { }
    public virtual void OnPreAnimateUpdateNoEnterExit(ShinAnimationStateMachine stateMachine) { }
    public virtual async UniTask OnPreAnimateExit(ShinAnimationStateMachine stateMachine) { }

    public virtual async UniTask OnEndAnimateEnter(ShinAnimationStateMachine stateMachine) { }
    public virtual void OnEndAnimateUpdate(ShinAnimationStateMachine stateMachine) { }
    public virtual void OnEndAnimateUpdateNoEnterExit(ShinAnimationStateMachine stateMachine) { }
    public virtual async UniTask OnEndAnimateExit(ShinAnimationStateMachine stateMachine) { }
    #endregion

    /// <summary>
    /// 入る前のステートが選択されているかどうか？
    /// </summary>
    /// <param name="nowState"></param>
    /// <param name="strings"></param>
    /// <returns></returns>
    public bool isEnterFromSelectedList(ShinBaseState preState)
    {
        foreach (var state in selectedAnimations.list)
        {
            //Debug.Log(preState.GetType().FullName + "==" + state + " , ");
            if (preState.GetType() == state)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Selectのタイプを指定
    /// </summary>
    public virtual void Reset()
    {
        if (selectedAnimations == null) selectedAnimations = new(SetSelectType(), addToObject: this);
    }

    public virtual void OnEnable()
    {
        if (selectedAnimations == null) selectedAnimations = new(SetSelectType(), addToObject: this);
    }

    public abstract Type SetSelectType();
}
