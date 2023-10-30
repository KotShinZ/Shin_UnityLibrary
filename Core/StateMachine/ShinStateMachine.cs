using Cysharp.Threading.Tasks;
using Shin_UnityLibrary;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using System;
using System.Linq;
using UniRx;

/// <summary>
/// シンプルなステートマシン(ステートの設定が出来る)
/// </summary>
public class ShinStateMachine : MonoBehaviour
{
    [SerializeField] protected List<ShinBaseState> states;

    [Readonly, SerializeField] protected ShinBaseState nowState;

    private bool isStateSetting = false; //ステートの遷移中かどうか

    public bool canEnterState = true;

    Queue<ShinBaseState> preEnterStates = new Queue<ShinBaseState>();

    private void Awake()
    {
        SetDefaultState(true).Forget();
        SetStateEnterObserver();
    }

    public void SetStateEnterObserver()
    {
        states.ForEach(s =>
        {
            var subject = new Subject<Type>();
            subject.Subscribe(type => SetState(type).Forget()).AddTo(s);
            s.stateEnterObserver = subject;
        });
    }

    /// <summary>
    /// 現在のステートを設定
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public async UniTask SetState(ShinBaseState _state)
    {
        if (CanSetState(nowState, _state))
        {
            preEnterStates.Enqueue(_state);
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
        SetState(type);
    }

    /// <summary>
    /// 現在のステートを強制的に設定
    /// </summary>
    /// <param name="_state"></param>
    /// <returns></returns>
    public async UniTask SetStateForce(ShinBaseState _state)
    {
        isStateSetting = true; //ステートの設定中である。
        if (nowState != null) { await nowState.OnStateExit(this); }  //ステートから出る
        nowState = _state; //現在のステートを設定
        await _state.m_OnStateEnter(this);

        isStateSetting = false;
    }

    /// <summary>
    /// ステートをセット出来るかどうか
    /// </summary>
    /// <param name="preState"></param>
    /// <param name="nextState"></param>
    public virtual bool CanSetState(ShinBaseState preState, ShinBaseState nextState)
    {
        if (!canEnterState) return false; //ステートに入れない状態である

        if (nextState.CanEnterState(preState, nextState))
        {
            return true;
        }
        else return false;
    }


    /// <summary>
    /// 現在のステートを取得
    /// </summary>
    /// <returns></returns>
    public ShinBaseState GetNowState()
    {
        return nowState;
    }

    /// <summary>
    /// デフォルトのステートに設定
    /// </summary>
    /// <returns></returns>
    public async UniTask SetDefaultState(bool force = true)
    {
        if (nowState != states[0])
        {
            if (force == true) await SetStateForce(states[0]);
            else SetState(states[0]);
        }
    }

    /// <summary>
    /// ステート中はUpdateメソッドを実行
    /// </summary>
    public virtual void Update()
    {
        if(preEnterStates.Count > 0)
        {
            var s = preEnterStates.Dequeue();
            //Debug.Log(s.gameObject.name);
            if (s != null) SetStateForce(s).Forget();
        }

        if (nowState != null && !isStateSetting) { nowState.OnStateUpdateNoEnterExit(this); }
        if (nowState != null) nowState.OnStateUpdate(this);
        states.ForEach(state => { state.OnUpdate(); });
    }

    /// <summary>
    /// このオブジェクトの全ステートマシンを取得
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