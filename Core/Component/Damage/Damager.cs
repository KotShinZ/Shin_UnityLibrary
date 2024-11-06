using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using Shin_UnityLibrary;
using Cysharp.Threading.Tasks;

[RequireComponent (typeof(Collider))]
public class Damager : HIttedTaged<Damageable>
{
    [TitleDescription] public string title = "�G���ƃ_���[�W��^����";

    public bool canDamege = true;
    public int damageAmout = 1;
    public float knockBackPower = 1;
    public float speadThreshold = -1;

    public void Reset()
    {
        speedRange = new MinMax(5, 50);
    }

    public override void Hit(CollisionData<Damageable> col)
    {
        Rigidbody r;
        var b = TryGetComponent<Rigidbody>(out r);
        if(b && speadThreshold != -1)
        {
            if (r.linearVelocity.magnitude < speadThreshold) return;
        }
       if(canDamege)
        {
            var messege = new Damageable.DamageMessage(this, damageAmout, col.direction * knockBackPower);
            col.component.ApplyDamage(messege);
        }
    }

    public void SetCanDamage(bool b)
    {
        canDamege = b;
    }
}
