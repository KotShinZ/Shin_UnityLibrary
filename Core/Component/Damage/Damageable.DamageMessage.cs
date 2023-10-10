using UnityEngine;

public partial class Damageable : MonoBehaviour
{
    public struct DamageMessage
    {
        public MonoBehaviour damager;
        public int amount;
        public Vector3 direction;
        public Vector3 damageSource;
        public bool throwing;

        public bool stopCamera;

        public DamageMessage(MonoBehaviour damager, int amount, Vector3 direction)
        {
            this.damager = damager;
            this.amount = amount;
            this.direction = direction;
            damageSource = damager.transform.position;
            throwing = false;
            stopCamera = false;
        }
        public DamageMessage(int amount)
        {
            this.damager = null;
            this.amount = amount;
            this.direction = Vector3.zero;
            damageSource = Vector3.zero;
            throwing = false;
            stopCamera = false;
        }
    }
}

