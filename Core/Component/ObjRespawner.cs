#define ISPLAYING

using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using Shin_UnityLibrary;


public class ObjRespawner : MonoBehaviour
{
    [TitleDescription] public string title = "óéÇøÇΩÇËâÛÇÍÇΩÇËÇµÇƒÇ‡\nå≥ÇÃèÍèäÇ≈ïúäàÇ∑ÇÈ";

    public float interval = 1f;
    public int limit = -1;

    public UnityEvent OnRespawnedEvent;

    private int nowLimitNum = 0;
    private GameObject respawnObj;

    private bool isPlaying;
    private bool isEditor;

    void SetRespawnObj()
    {
        respawnObj =  Utils.InstantiateCopy(gameObject);
        respawnObj.gameObject.SetActive(false);
    }

    private void Start()
    {
        SetRespawnObj();
    }


    private void OnDestroy()
    {
#if UNITY_EDITOR
        isPlaying = UnityEditor.EditorApplication.isPlaying;
        isEditor = true;
#else
        isPlaying = true;
        isEditor = false;
#endif

        if((isPlaying || !isEditor))
        {
            if (limit == -1 || nowLimitNum < limit)
            {
                nowLimitNum += 1;
                Respawn(respawnObj).Forget();
            }
        }
    }

    static async UniTask Respawn(GameObject ins)
    {
        if(ins != null)
        {
            var respawner = ins.GetComponent<ObjRespawner>();
            await UniTask.Delay((int)(respawner.interval * 1000));

            if (ins == null) return;
            ins.SetActive(true);

            respawner.OnRespawned();
        }
    }

    void OnRespawned()
    {
        OnRespawnedEvent.Invoke();
    }

}

