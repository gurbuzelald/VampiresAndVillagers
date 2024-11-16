using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampireController : MonoBehaviour
{
    [SerializeField] private Transform _targetsObject;
    [SerializeField] private float speed;

    private Transform[] _targets;
    private int _currentTargetIndex = 0;

    private RaycastHit[] hit;

    [SerializeField] float radius;
    [SerializeField] float distance;

    private GameObject currentHumanObject;

    private AttackComponent attackComponent;

    private HealthComponent healthComponent;

    [SerializeField] float decreaseHealthValue;
    private float lastDecreaseTime;

    private void Awake()
    {
        attackComponent = GetComponent<AttackComponent>();
        healthComponent = GetComponent<HealthComponent>();

        currentHumanObject = null;
        int targetCount = _targetsObject.childCount;
        _targets = new Transform[targetCount];
        for (int i = 0; i < targetCount; i++)
        {
            _targets[i] = _targetsObject.GetChild(i);
        }
    }

    void Update()
    {
        HandleMovementAndAttack();

        DecreaseHealth();
    }

    
    private void HandleMovementAndAttack()
    {
        if (currentHumanObject == null)
        {
            if (Vector3.Distance(transform.position, _targets[_currentTargetIndex].position) < 0.1f)
            {
                _currentTargetIndex = Random.Range(0, _targets.Length);
            }

            MoveTowardsCurrentTarget(_targets[_currentTargetIndex]);
            SendRayToForward();
        }
        else
        {
            if (InDistance(currentHumanObject.transform.position))
            {
                MoveTowardsCurrentTarget(currentHumanObject.transform);

                if (attackComponent.IsAttackable(currentHumanObject.transform))
                {
                    attackComponent.Attack(currentHumanObject.transform);
                    healthComponent.AddHealth(10);
                }
            }
            else
            {
                currentHumanObject = null;
            }
        }
    }

    private void MoveTowardsCurrentTarget(Transform currentTarget)
    {
        Vector3 targetPosition = currentTarget.position;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        transform.LookAt(targetPosition);
    }

    private void DecreaseHealth()
    {
        if (currentHumanObject == null)
        {
            if (IsDecreaseHealth())
            {
                healthComponent.GetDamage(1);

                lastDecreaseTime = Time.time;
            }
        }
        else
        {
            if (IsDecreaseHealth(currentHumanObject.transform))
            {
                healthComponent.GetDamage(1);

                lastDecreaseTime = Time.time;
            }
        }
        
    }

    public bool DecreaseHealthDelay()
    {
        return Time.time - lastDecreaseTime >= decreaseHealthValue;
    }

    private bool IsDecreaseHealth(Transform target = null)
    {
        if (target)
        {
            return !attackComponent.IsAttackable(target) &&
               DecreaseHealthDelay();
        }
        else
        {
            return DecreaseHealthDelay();
        }
        
    }

    private bool InDistance(Vector3 targetPosition)
    {
        return Vector3.Distance(transform.position, targetPosition) <= radius;
    }

    private void SendRayToForward()
    {
      
        hit = Physics.SphereCastAll(transform.position, radius, transform.forward, radius);

        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i].collider.CompareTag("Human") && hit[i].collider.gameObject != currentHumanObject)
            {
                currentHumanObject = hit[i].collider.gameObject;
            }
        }
    }
}
