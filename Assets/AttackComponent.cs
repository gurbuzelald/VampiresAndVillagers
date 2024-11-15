using System;
using UnityEngine;

public class AttackComponent : MonoBehaviour
{
    public int damage;

    public float attackDistance;

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

    public void Attack(Transform _targetObject)
    {
        if (GetAttackDelayTimer() < 1) return;

        if(gameObject.CompareTag("Vampire"))
        {
            if (Vector3.Distance(gameObject.transform.position, _targetObject.transform.position) < attackDistance)
            {
                if (IsAttackableVision(_targetObject))
                {
                    currentDamagerHealth = _targetObject.GetComponent<HealthComponent>();

                    if (currentDamagerHealth != null)
                    {
                        currentDamagerHealth.GetDamage(damage);

                        OnAttack?.Invoke(_targetObject.gameObject);

                        SetAttackDelayTimer(0);
                    }
                }
            }
        }
    }
    private bool IsAttackableVision(Transform targetObject)
    {
        Vector3 direction = (targetObject.transform.position - transform.position);

        float distance = direction.magnitude;

        direction = direction.normalized;

        Vector3 forward = transform.forward;

        float degree = Vector3.AngleBetween(forward, direction);

        return degree <= 30 && distance <= 2;
    }
}
