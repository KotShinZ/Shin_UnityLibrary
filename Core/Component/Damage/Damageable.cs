using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using System;

public partial class Damageable : MonoBehaviour
{
    public int maxHitPoints;
    public float invulnerabiltyTime;

    [Range(0.0f, 360.0f)]
    public float hitAngle = 360.0f;
    [Range(0.0f, 360.0f)]
    [FormerlySerializedAs("hitForwardRoation")] //SHAME!
    public float hitForwardRotation = 360.0f;

    public bool isInvulnerable { get; set; }
    public bool isDeath => currentHitPoints <= 0;
    public virtual int currentHitPoints
    {
        get { return m_currentHitPoints; }
        set
        {
            m_currentHitPoints = value;

        }
    }
    public int m_currentHitPoints;

    public UnityEvent OnDeath, OnBecomeVulnerable, OnResetDamage, OnReceiveDamage, OnHitWhileInvulnerable;

    [Tooltip("When this gameObject is damaged, these other gameObjects are notified.")]
    [EnforceInterface(typeof(IReceiveDamageMessage))]
    public List<MonoBehaviour> onDamageMessageReceivers;

    protected float m_timeSinceLastHit = 0.0f;
    protected Collider m_Collider;

    System.Action schedule;

    public virtual void Start()
    {
        ResetDamage();
        m_Collider = GetComponent<Collider>();
    }

    void Update()
    {
        if (isInvulnerable)
        {
            m_timeSinceLastHit += Time.deltaTime;
            if (m_timeSinceLastHit > invulnerabiltyTime)
            {
                m_timeSinceLastHit = 0.0f;
                isInvulnerable = false;
                OnBecomeVulnerable.Invoke();
            }
        }
    }

    public void ResetDamage()
    {
        currentHitPoints = maxHitPoints;
        isInvulnerable = false;
        m_timeSinceLastHit = 0.0f;
        OnResetDamage.Invoke();
    }

    public void SetColliderState(bool enabled)
    {
        m_Collider.enabled = enabled;
    }

    public void ApplyDamage(DamageMessage data)
    {
        if (currentHitPoints <= 0)
        {//ignore damage if already dead. TODO : may have to change that if we want to detect hit on death...
            return;
        }

        if (isInvulnerable)
        {
            OnHitWhileInvulnerable.Invoke();
            return;
        }

        Vector3 forward = transform.forward;
        forward = Quaternion.AngleAxis(hitForwardRotation, transform.up) * forward;

        //we project the direction to damager to the plane formed by the direction of damage
        Vector3 positionToDamager = data.damageSource - transform.position;
        positionToDamager -= transform.up * Vector3.Dot(transform.up, positionToDamager);

        if (Vector3.Angle(forward, positionToDamager) > hitAngle * 0.5f)
            return;
        isInvulnerable = true;
        currentHitPoints -= data.amount;

        if (currentHitPoints <= 0)
            schedule += OnDeath.Invoke; //This avoid race condition when objects kill each other.
        else
            OnReceiveDamage.Invoke();

        var messageType = currentHitPoints <= 0 ? DamageMessageType.DEAD : DamageMessageType.DAMAGED;

        for (var i = 0; i < onDamageMessageReceivers.Count; ++i)
        {
            var receiver = onDamageMessageReceivers[i] as IReceiveDamageMessage;
            if (receiver != null) receiver.OnReceiveMessage(messageType, this, data);
        }
    }
    public void ApplyDamage(int n)
    {
        if (currentHitPoints <= 0)
        {//ignore damage if already dead. TODO : may have to change that if we want to detect hit on death...
            return;
        }

        if (isInvulnerable)
        {
            OnHitWhileInvulnerable.Invoke();
            return;
        }

        Vector3 forward = transform.forward;
        forward = Quaternion.AngleAxis(hitForwardRotation, transform.up) * forward;

        isInvulnerable = true;
        currentHitPoints -= n;

        if (currentHitPoints <= 0)
            schedule += OnDeath.Invoke; //This avoid race condition when objects kill each other.
        else
            OnReceiveDamage.Invoke();

        var messageType = currentHitPoints <= 0 ? DamageMessageType.DEAD : DamageMessageType.DAMAGED;

        for (var i = 0; i < onDamageMessageReceivers.Count; ++i)
        {
            var receiver = onDamageMessageReceivers[i] as IReceiveDamageMessage;
            if (receiver != null) receiver.OnReceiveMessage(messageType, this, new DamageMessage(n));
        }
    }

    void LateUpdate()
    {
        if (schedule != null)
        {
            schedule();
            schedule = null;
        }
    }

    private void OnDestroy()
    {
        schedule += OnDeath.Invoke;

        var messageType = DamageMessageType.DEAD;

        for (var i = 0; i < onDamageMessageReceivers.Count; ++i)
        {
            var receiver = onDamageMessageReceivers[i] as IReceiveDamageMessage;
            if (receiver != null) receiver.OnReceiveMessage(messageType, this, new DamageMessage(0));
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Vector3 forward = transform.forward;
        forward = Quaternion.AngleAxis(hitForwardRotation, transform.up) * forward;

        if (Event.current.type == EventType.Repaint)
        {
            UnityEditor.Handles.color = Color.blue;
            UnityEditor.Handles.ArrowHandleCap(0, transform.position, Quaternion.LookRotation(forward), 1.0f,
                EventType.Repaint);
        }


        UnityEditor.Handles.color = new Color(1.0f, 0.0f, 0.0f, 0.5f);
        forward = Quaternion.AngleAxis(-hitAngle * 0.5f, transform.up) * forward;
        UnityEditor.Handles.DrawSolidArc(transform.position, transform.up, forward, hitAngle, 1.0f);
    }
#endif
}


[Serializable] public class UnityEventNum<T> : UnityEvent<T> { }
[Serializable] public class UnityEventDamageable : UnityEvent<Damageable> { }