using System;
using UnityEngine;

public class AttackComponent : MonoBehaviour
{
    public int damage;

    public float attackDistance;

    public float attackLoopTime;

    public Action<GameObject> OnAttack;

    float lastAttackTime;

    private void Awake()
    {
        lastAttackTime = 0;
    }
    
    public bool IsAttackable(Transform target)
    {
        return AttackTimeFinished() && IsAttackableVision(target.position) && IsAttackableNear(target.position);
    }

    public bool AttackTimeFinished()
    {
        return Time.time - lastAttackTime >= attackLoopTime;
    }

    public void Attack(Transform _targetObject)
    {
        HealthComponent opponentHealth = _targetObject.GetComponent<HealthComponent>();

        if (opponentHealth!= null)
        {
            opponentHealth.GetDamage(damage);

            lastAttackTime = Time.time;

            OnAttack?.Invoke(_targetObject.gameObject); 
        }
    }

    private bool IsAttackableNear(Vector3 position)
    {
        return Vector3.Distance(transform.position, position) <= attackDistance;
    }
    private bool IsAttackableVision(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        float angle = Vector3.SignedAngle(transform.forward, direction, Vector3.up);

        return angle >= -30f && angle <= 30f;
    }
}
