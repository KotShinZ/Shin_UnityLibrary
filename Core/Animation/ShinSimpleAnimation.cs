using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;
using System.Collections;
using Cysharp.Threading.Tasks;
using System.Threading;
using UniRx;

[RequireComponent(typeof(Animator))]
public class ShinSimpleAnimation : MonoBehaviour
{
    PlayableGraph graph;
    AnimationMixerPlayable mixer;
    [HideInInspector] public AnimationClipPlayable prePlayable, currentPlayable;
    AnimationClip preClip;
    IEnumerator preTask;
    Animator animator;

    void Awake()
    {
        graph = PlayableGraph.Create();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        // AnimationClipをMixerに登録
        AnimationClip clipNull = new AnimationClip();
        currentPlayable = AnimationClipPlayable.Create(graph, clipNull);
        mixer = AnimationMixerPlayable.Create(graph, 2);
        mixer.ConnectInput(0, currentPlayable, 0);
        mixer.SetInputWeight(0, 1);

        // outputにmixerとanimatorを登録して、再生
        var output = AnimationPlayableOutput.Create(graph, "output", animator);
        output.SetSourcePlayable(mixer);
        graph.Play();
    }

    public void Play(AnimationClip clip)
    {

    }
    public void CrossFade(AnimationClip clip, float fadeTime, float spead = 1)
    {
        if (clip == preClip) return;
        preClip = clip;
        IEnumerator cor = CrossFadeAnim(clip, fadeTime, spead);
        Observable.FromCoroutine(() => CrossFadeAnim(clip, fadeTime, spead)).Subscribe().AddTo(this);
    }
    public async UniTask CrossFade(AnimationClip clip, float fadeTime, float spead = 1,  CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();
        

        if (clip == preClip) return;
        preClip = clip;

        //StopCoroutine(preTask);
        IEnumerator cor = CrossFadeAnim(clip, fadeTime, spead);
        Observable.FromCoroutine(()=> CrossFadeAnim(clip, fadeTime, spead)).Subscribe().AddTo(this);
        await UniTask.Delay((int)(clip.length * 1000));
        return;
    }
    IEnumerator CrossFadeAnim(AnimationClip clip, float fadeTime, float speed)
    {
        Debug.Log(clip.name);
        if (!graph.IsValid()) yield break; 
        // ClipPlayableを上書きは出来ない為、一旦mixerの1番と2番を一旦アンロード
        graph.Disconnect(mixer, 0);
        graph.Disconnect(mixer, 1);

        // 古いアニメーションを破棄し、次に再生するアニメーションを登録する
        if (prePlayable.IsValid())
            prePlayable.Destroy();
        prePlayable = currentPlayable;
        currentPlayable = AnimationClipPlayable.Create(graph, clip);
        currentPlayable.SetSpeed(speed);
        mixer.ConnectInput(1, prePlayable, 0);
        mixer.ConnectInput(0, currentPlayable, 0);

        // 指定時間でアニメーションをブレンド
        float waitTime = Time.timeSinceLevelLoad + fadeTime;
        yield return new WaitWhile(() =>
        {
            var diff = waitTime - Time.timeSinceLevelLoad;
            if (diff <= 0)
            {
                if(!mixer.IsValid()) return false;
                mixer.SetInputWeight(1, 0);
                mixer.SetInputWeight(0, 1);
                return false;
            }
            else
            {
                if (!mixer.IsValid()) return true;
                var rate = Mathf.Clamp01(diff / fadeTime);
                mixer.SetInputWeight(1, rate);
                mixer.SetInputWeight(0, 1 - rate);
                return true;
            }
        });
    }

    void OnDestroy()
    {
        graph.Destroy();
    }
}