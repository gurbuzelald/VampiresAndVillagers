using System;
using UnityEngine;

public class AttackComponent : MonoBehaviour
{
    public int damage;

    public float attackDistance;

    public float attackDelay;

    public float attackDelayTimer;

    public Action<GameObject> OnAttack;

    private HealthComponent currentDamagerHealth;

    private void Start()
    {
        attackDelayTimer = 0;
    }

    private void Update()
    {
        GetAttackDelayTimer();
        Attack();
    }
    private float GetAttackDelayTimer()
    {
        attackDelayTimer += Time.deltaTime;

        return attackDelayTimer;
    }

    private void SetAttackDelayTimer(float _attackDelayTimer)
    {
        attackDelayTimer = _attackDelayTimer;
    }

    public void Attack()
    {
        if (GetAttackDelayTimer() < 1) return;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, attackDistance))
        {
            currentDamagerHealth = hit.collider.GetComponent<HealthComponent>();

            if (currentDamagerHealth != null)
            {
                currentDamagerHealth.GetDamage(damage);

                OnAttack?.Invoke(hit.collider.gameObject);

                SetAttackDelayTimer(0);
            }
        }
    }
}
