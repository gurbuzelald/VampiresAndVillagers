using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampireController : BaseCharacter
{
    [SerializeField] private float speed;

    private Transform[] _targets;
    private int _currentTargetIndex = 0;

    private RaycastHit[] hit;

    [SerializeField] float radius;

    private AttackComponent attackComponent;

    private HealthComponent healthComponent;

    [SerializeField] float decreaseHealthValue;
    private float lastDecreaseTime;

    private BaseCharacter baseCharacter;

    private PointsSingleton pointsSingleton;

    private void Awake()
    {
        attackComponent = GetComponent<AttackComponent>();
        healthComponent = GetComponent<HealthComponent>();
        pointsSingleton = FindAnyObjectByType<PointsSingleton>();

        baseCharacter = null;

        int targetCount = pointsSingleton.vampireTargetsObject.childCount;

        _targets = new Transform[targetCount];

        for (int i = 0; i < targetCount; i++)
        {
            _targets[i] = pointsSingleton.vampireTargetsObject.GetChild(i);
        }
    }

    void Update()
    {
        HandleMovementAndAttack();

        DecreaseHealth();
    }

    
    private void HandleMovementAndAttack()
    {
        if (baseCharacter == null)
        {
            if (Vector3.Distance(transform.position, _targets[_currentTargetIndex].position) < 0.1f)
            {
                _currentTargetIndex = Random.Range(0, _targets.Length);
            }

            MoveTowardsCurrentTarget(_targets[_currentTargetIndex]);

        }
        else
        {
            if (InDistance(baseCharacter.transform.position) && !baseCharacter.isHidden)
            {
                MoveTowardsCurrentTarget(baseCharacter.transform);

                if (attackComponent.IsAttackable(baseCharacter.transform))
                {
                    attackComponent.Attack(baseCharacter.transform);
                    healthComponent.AddHealth(10);
                }
            }
            else
            {
                baseCharacter = null;
            }
        }

        SendRayToForward();
    }

    private void MoveTowardsCurrentTarget(Transform currentTarget)
    {
        Vector3 targetPosition = currentTarget.position;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        transform.LookAt(targetPosition);
    }

    private void DecreaseHealth()
    {
        if (IsDecreaseHealth())
        {
            healthComponent.GetDamage(1);

            lastDecreaseTime = Time.time;
        }
    }

    public bool DecreaseHealthDelay()
    {
        return Time.time - lastDecreaseTime >= decreaseHealthValue;
    }

    private bool IsDecreaseHealth(Transform target = null)
    {
        return DecreaseHealthDelay();
    }

    private bool InDistance(Vector3 targetPosition)
    {
        return Vector3.Distance(transform.position, targetPosition) <= radius;
    }

    private void SendRayToForward()
    {
        hit = Physics.SphereCastAll(transform.position, radius, transform.forward, radius);

        float nearestDistance = Mathf.Infinity;

        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i].collider.CompareTag("Human"))
            {
                float tempMinDistance = Vector3.Distance(transform.position, hit[i].collider.gameObject.transform.position);

                if (tempMinDistance < nearestDistance)
                {
                    nearestDistance = tempMinDistance;

                    baseCharacter = hit[i].collider.gameObject.GetComponent<BaseCharacter>();
                }
            }
            if (baseCharacter)
                if (baseCharacter.isHidden)
                {
                    baseCharacter = null;
                    return;
                }
        }
    }
}
