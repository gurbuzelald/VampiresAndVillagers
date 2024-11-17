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

    private UnityEngine.AI.NavMeshAgent navMeshAgent;

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

        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
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
            navMeshAgent.SetDestination(_targets[_currentTargetIndex].position);
        }
        else
        {
            if (InDistance(baseCharacter.transform.position) && baseCharacter.currentState != State.Hiding)
            {
                navMeshAgent.SetDestination(baseCharacter.transform.position);

                if (attackComponent.IsAttackable(baseCharacter.transform))
                {
                    attackComponent.Attack(baseCharacter.transform);
                    healthComponent.AddHealth(10);
                }
            }
            else
            {
                baseCharacter = null;
                return;
            }
        }

        SendRayToForward();
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

    private bool IsDecreaseHealth()
    {
        return DecreaseHealthDelay();
    }

    private bool InDistance(Vector3 targetPosition)
    {
        return Vector3.Distance(transform.position, targetPosition) <= radius;
    }

    private void SendRayToForward()
    {
        Collider[] hit = Physics.OverlapSphere(transform.position, radius);

        float nearestDistance = Mathf.Infinity;

        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i].CompareTag("Human") &&
                hit[i].gameObject.GetComponent<BaseCharacter>().currentState != State.Hiding)
            {
                float tempMinDistance = Vector3.Distance(transform.position, hit[i].gameObject.transform.position);
                BaseCharacter newCharacterTarget = hit[i].gameObject.GetComponent<BaseCharacter>();

                if (tempMinDistance < nearestDistance &&
                    newCharacterTarget.currentState != State.Hiding)
                {
                    nearestDistance = tempMinDistance;

                    baseCharacter = newCharacterTarget;
                }
            }
        }
    }
}
