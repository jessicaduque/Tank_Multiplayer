using System;
using Unity.Netcode;
using UnityEngine;

public class Health : NetworkBehaviour
{
    [field: SerializeField] public int maxHealth { get; private set; } = 100;

    public NetworkVariable<int> currentHealth = new NetworkVariable<int>();
    public Action<Health> onDie; 

    private bool _isDead;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (!IsServer)
        {
            return;
        }

        currentHealth.Value = maxHealth;
    }

    public void TakeDamage(int damageValue)
    {
        ModifyHealth(-damageValue);
    }

    public void RestoreHealth(int healValue)
    {
        ModifyHealth(healValue);
    }

    public void ModifyHealth(int value)
    {
        if (_isDead)
        {
            return;
        }

        int newHealth = currentHealth.Value + value;
        currentHealth.Value = Mathf.Clamp(newHealth, 0, maxHealth);

        if(currentHealth.Value == 0)
        {
            onDie?.Invoke(this);
            _isDead = true;
        }
    }

}
