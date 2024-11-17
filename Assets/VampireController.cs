using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampireController : BaseCharacter
{
    [SerializeField] private float speed;

    private Transform[] _targets;
    private int _currentTargetIndex = 0;

    [SerializeField] float radius;

    private AttackComponent attackComponent;

    private HealthComponent healthComponent;

    private EnergyComponent energyComponent;

    [SerializeField] float decreaseHealthValue;
    private float lastDecreaseTime;

    private BaseCharacter baseCharacter;

    private PointsSingleton pointsSingleton;

    private UnityEngine.AI.NavMeshAgent navMeshAgent;

    private RaycastHit[] hits;

    private void Awake()
    {
        attackComponent = GetComponent<AttackComponent>();
        healthComponent = GetComponent<HealthComponent>();
        energyComponent = GetComponent<EnergyComponent>();
        pointsSingleton = FindAnyObjectByType<PointsSingleton>();

        baseCharacter = null;

        int targetCount = pointsSingleton.vampireTargetsObject.childCount;

        _targets = new Transform[targetCount];

        for (int i = 0; i < targetCount; i++)
        {
            _targets[i] = pointsSingleton.vampireTargetsObject.GetChild(i);
        }

        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();


        _hideAreasObject = pointsSingleton.hideAreasObject;

        int hideAreaID = _hideAreasObject.childCount;

        hideAreas = new Transform[hideAreaID];

        for (int i = 0; i < hideAreaID; i++)
        {
            hideAreas[i] = _hideAreasObject.GetChild(i);
        }
    }

    void Update()
    {
        HandleStates(_targets[_currentTargetIndex]);
        HandleMovementAndAttack();

        DecreaseHealth();
    }

    public void HandleStates(Transform patrolTarget)
    {
        switch (currentState)
        {
            case State.Patrolling:
                Patrol(patrolTarget);
                break;

            case State.Escaping:
                Escape();
                break;
            case State.Hiding:
                healthComponent.AddHealth(2);
                break;
            case State.Attacking:
                Attack();
                break;
        }
    }

    private void HandleMovementAndAttack()
    {
        if (baseCharacter == null)
        {
            currentState = State.Patrolling;

            navMeshAgent.speed = speed;
        }
        else
        {
            if (baseCharacter.haveGun && healthComponent.Health <= 30)
            {
                currentState = State.Escaping;
            }
            else if (InDistance(baseCharacter.transform.position) && baseCharacter.currentState != State.Hiding)
            {
                energyComponent.DecreseEnergy(1);

                navMeshAgent.SetDestination(baseCharacter.transform.position);

                if (!energyComponent.isEnergyZero)
                {
                    navMeshAgent.speed = speed * 1.5f;
                }
                else
                {
                    navMeshAgent.speed = speed;
                }

                if (attackComponent.IsAttackable(baseCharacter.transform))
                {
                    currentState = State.Attacking;

                    energyComponent.IncreaseEnergy(20);

                    Debug.Log("1");
                }
            }
            else
            {
                currentState = State.Patrolling;

                baseCharacter = null;
                return;
            }
        }

        SendRayToForward();
    }

    private void DecreaseHealth()
    {
        if (IsDecreaseHealth() && currentState != State.Hiding)
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
    private void Attack()
    {
        if (baseCharacter == null) return;
        attackComponent.Attack(baseCharacter.transform);
        healthComponent.AddHealth(10);
        currentState = State.Patrolling;
    }

    private void Patrol(Transform patrolTarget)
    {
        if (Vector3.Distance(transform.position, patrolTarget.position) < 0.1f)
        {
            _currentTargetIndex = Random.Range(0, _targets.Length);
        }

        navMeshAgent.SetDestination(patrolTarget.position);
    }

    private void Escape()
    {
        float nearestDistance = Mathf.Infinity;

        for (int i = 0; i < hideAreas.Length; i++)
        {
            float tempDistance = Vector3.Distance(transform.position, hideAreas[i].position);
            if (tempDistance < nearestDistance)
            {
                nearestDistance = tempDistance;
                currentHideAreaIndex = i;
            }
        }

        navMeshAgent.SetDestination(hideAreas[currentHideAreaIndex].position);

        if (Vector3.Distance(transform.position, hideAreas[currentHideAreaIndex].position) < .5f)
        {
            navMeshAgent.isStopped = true;
            currentState = State.Hiding;
        }
        if (currentState == State.Hiding)
        {
            StartCoroutine(DeactivateHiding(hiddenTime));
        }
    }

    private IEnumerator DeactivateHiding(float hiddenTime)
    {
        yield return new WaitForSeconds(hiddenTime);

        currentState = State.Patrolling;

        navMeshAgent.isStopped = false;

        yield return null;
    }
}
