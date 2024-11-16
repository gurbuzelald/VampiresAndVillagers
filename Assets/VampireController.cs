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

    private AttackComponent attackComponent;

    private HealthComponent healthComponent;

    [SerializeField] float decreaseHealthValue;
    private float lastDecreaseTime;

    private HumanController humanController;

    private void Awake()
    {
        attackComponent = GetComponent<AttackComponent>();
        healthComponent = GetComponent<HealthComponent>();

        humanController = null;

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
        if (humanController == null)
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
            if (InDistance(humanController.transform.position) && !humanController.isHidden)
            {
                MoveTowardsCurrentTarget(humanController.transform);

                if (attackComponent.IsAttackable(humanController.transform))
                {
                    attackComponent.Attack(humanController.transform);
                    healthComponent.AddHealth(10);
                }
            }
            else
            {
                humanController = null;
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

        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i].collider.CompareTag("Human") && hit[i].collider.gameObject != humanController)
            {
                humanController = hit[i].collider.gameObject.GetComponent<HumanController>();
            }
        }
    }
}
