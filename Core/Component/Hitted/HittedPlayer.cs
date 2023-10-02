using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Shin_UnityLibrary;
using Cysharp.Threading.Tasks;

public class HittedPlayer<T> : HittedTagedGameObject where T : Component
{
    private void Reset()
    {
        tags = new List<string>() { "Player" };
    }

    public virtual async UniTask HitPlayer(T t , CollisionData<Transform> col) { }

    public override void Hit(CollisionData<Transform> col)
    {
        var t = GetHitComponent<T>(col.component.gameObject);
        HitPlayer(t, col).Forget();
    }
}

