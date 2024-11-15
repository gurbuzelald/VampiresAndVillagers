using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    public bool isDead;

    public int Health;

    public int initialHealth;

    public Action<int> OnHealthValueChanged;

    public Action OnPlayerDead;

    public Action OnPlayerGetDamage;

    private void Awake()
    {
        Health = initialHealth;
    }

    public void GetDamage(int damage)
    {
        if (isDead)
            return;

        Health -= damage;

        if (Health <= 0)
        {
            Health = 0;
            isDead = true;
            OnPlayerDead?.Invoke();
        }
        else
        {
            OnPlayerGetDamage?.Invoke();
        }

        OnHealthValueChanged?.Invoke(Health);
       
    }

    public void AddHealth(int addValue)
    {
        Health += addValue;
    }
}
