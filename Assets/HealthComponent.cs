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

    public Action OnDead;

    public Action OnGetDamage;

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
            OnDead?.Invoke();
            Destroy(this.gameObject);
            Debug.LogError("Dead " + this.gameObject.name);
        }
        else
        {
            OnGetDamage?.Invoke();
        }

        OnHealthValueChanged?.Invoke(Health);
       
    }

    public void AddHealth(int addValue)
    {
        if (Health + addValue > initialHealth)
        {
            Health = initialHealth;
        }
        else
        {
            Health += addValue;
        }
    }
}
