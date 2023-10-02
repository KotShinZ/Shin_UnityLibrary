using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Gettable : MonoBehaviour
{
    public float radius = 1;
    [Tag]public List<string> tags = new List<string>();

    private List<GameObject> players = new List<GameObject>();
    private bool isGetted = false;

    [Space(7)]
    public UnityEvent getted = new UnityEvent();

    private void Awake()
    {
        foreach(var t in tags)
        {
            var objs = GameObject.FindGameObjectsWithTag(t);
            players.AddRange(objs);
        }
    }

    private void Update()
    {
        if (isGetted) return;

        foreach(var p in players)
        {
            if (Vector3.Distance(p.transform.position, transform.position) > radius) continue;
            Getted(p).Forget();
            return;
        }
    }

    async UniTask Getted(GameObject p)
    {
        isGetted = true;
        getted.Invoke();
        await Get(p);
        Destroy(gameObject);
    }

    public virtual async UniTask Get(GameObject p) { }
}
