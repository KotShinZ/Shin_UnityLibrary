public interface IReceiveDamageMessage
{
    void OnReceiveMessage(DamageMessageType type, Damageable sender, Damageable.DamageMessage msg);
}

public enum DamageMessageType
{
    DEAD,
    DAMAGED
}