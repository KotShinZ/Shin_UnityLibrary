using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using Shin_UnityLibrary;
using Cysharp.Threading.Tasks;

[RequireComponent (typeof(Collider))]
public class Damageer : HIttedTaged<Damageable>
{
    [TitleDescription] public string title = "�G���ƃ_���[�W��^����"; 

    public int damageAmout = 1;
    public float knockBackPower = 1;
    public float speadThreshold = -1;

    public override void Hit(CollisionData<Damageable> col)
    {
        Rigidbody r;
        var b = TryGetComponent<Rigidbody>(out r);
        if(b && speadThreshold != -1)
        {
            if (r.linearVelocity.magnitude < speadThreshold) return;
        }
        var messege = new Damageable.DamageMessage(this, damageAmout, col.direction * knockBackPower);
        col.component.ApplyDamage(messege);
    }
}
