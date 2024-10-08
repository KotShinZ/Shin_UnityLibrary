//  SingletonMonoBehaviour.cs
//  http://kan-kikuchi.hatenablog.com/entry/SingletonMonoBehaviour
//
//  Created by kan.kikuchi on 2016.04.19.

using UnityEngine;

/// <summary>
/// MonoBehaviourを継承し、初期化メソッドを備えたシングルトンなクラス
/// </summary>
public class SingletonMonoBehaviour<T> : MonoBehaviourWithInit where T :MonoBehaviourWithInit, new()
{

    //インスタンス
    private static T _instance;

    //インスタンスを外部から参照する用(getter)
    public static T instance
    {
        get
        {
            //インスタンスがまだ作られていない
            if (_instance == null)
            {

                //シーン内からインスタンスを取得
                _instance = (T)GameObject.FindObjectOfType(typeof(T));

                //シーン内に存在しない場合はエラー
                if (_instance == null)
                {
                    var obj = new GameObject(typeof(T).Name);
                    _instance = obj.AddComponent<T>();
                
                    _instance.InitIfNeeded();
                }
                //発見した場合は初期化
                else
                {
                    _instance.InitIfNeeded();
                }
            }
            return _instance;
        }
    }

    //=================================================================================
    //初期化
    //=================================================================================

    protected sealed override void Awake()
    {
        //存在しているインスタンスが自分であれば問題なし
        if (this == instance)
        {
            InitIfNeeded();
            return;
        }

        //自分じゃない場合は重複して存在しているので、エラー
        Debug.LogWarning(typeof(T) + " is duplicated");
    }

}

/// <summary>
/// 初期化メソッドを備えたMonoBehaviour
/// </summary>
public class MonoBehaviourWithInit : MonoBehaviour
{

    //初期化したかどうかのフラグ(一度しか初期化が走らないようにするため)
    private bool _isInitialized = false;

    /// <summary>
    /// 必要なら初期化する
    /// </summary>
    public void InitIfNeeded()
    {
        if (_isInitialized)
        {
            return;
        }
        Init();
        _isInitialized = true;
    }

    /// <summary>
    /// 初期化(Awake時かその前の初アクセス、どちらかの一度しか行われない)
    /// </summary>
    protected virtual void Init() { }

    //sealed overrideするためにvirtualで作成
    protected virtual void Awake() { }

}