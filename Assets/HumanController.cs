using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanController : BaseCharacter
{    
    [SerializeField] private float speed;

    private Transform[] _targets;
    
    private int _currentTargetIndex = 0;
    
    [SerializeField] float radius;

    public Vector3 currentTargetPosition;

    private PointsSingleton pointsSingleton;

    [SerializeField] LayerMask layerMask;

    private RaycastHit[] hits;

    private UnityEngine.AI.NavMeshAgent navMeshAgent;


    private void Awake()
    {
        currentState = State.Patrolling;
        currentHideAreaIndex = 0;

        pointsSingleton = FindAnyObjectByType<PointsSingleton>();

        _hideAreasObject = pointsSingleton.hideAreasObject;

        int hideAreaID = _hideAreasObject.childCount;

        hideAreas = new Transform[hideAreaID];

        for (int i = 0; i < hideAreaID; i++)
        {
            hideAreas[i] = _hideAreasObject.GetChild(i);
        }

        int targetID = pointsSingleton.humanTargetsObject.childCount;
       
        _targets = new Transform[targetID];
        

        for (int i = 0; i < targetID; i++)
        {
            _targets[i] = pointsSingleton.humanTargetsObject.GetChild(i);
        }

        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    void Update()
    {
        hits = CheckAround(transform, radius, layerMask);

        SetRandomTargetIndex();

        HandleStates(hits, ref currentTargetPosition, _targets[_currentTargetIndex]);
    }

    void SetRandomTargetIndex()
    {
        if (Vector3.Distance(transform.position, _targets[_currentTargetIndex].position) < 0.1f
            && currentState != State.Hiding)
        {
            _currentTargetIndex = Random.RandomRange(0, _targets.Length);
        }
    }

    private RaycastHit[] CheckAround(Transform _transform, float radius, LayerMask layerMask)
    {
        return Physics.SphereCastAll(_transform.position, radius, _transform.forward, radius, layerMask);
    }


    public void HandleStates(RaycastHit[] hits, ref Vector3 currentTargetPosition, Transform patrolTarget)
    {
        switch (currentState)
        {
            case State.Patrolling:
                Patrol(ref currentTargetPosition, patrolTarget);
                CheckForVampires(hits);
                break;

            case State.Escaping:
                Escape(ref currentTargetPosition);
                break;

            case State.Hiding:
                break;
        }
    }

    private void Patrol(ref Vector3 currentTargetPosition, Transform patrolTarget)
    {
        if (navMeshAgent != null && patrolTarget != null)
        {
            navMeshAgent.SetDestination(patrolTarget.position);
        }
    }

    private void Escape(ref Vector3 currentTargetPosition)
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

        if (navMeshAgent != null)
        {
            navMeshAgent.SetDestination(hideAreas[currentHideAreaIndex].position);
        }

        if (Vector3.Distance(transform.position, hideAreas[currentHideAreaIndex].position) < 1)
        {
            currentState = State.Hiding;
            StartCoroutine(DeactivateHiding(hiddenTime));
        }
    }

    private void CheckForVampires(RaycastHit[] hits)
    {
        foreach (var hit in hits)
        {
            if (hit.collider.CompareTag("Vampire"))
            {
                currentState = State.Escaping;
                return;
            }
        }
    }

    private IEnumerator DeactivateHiding(float hiddenTime)
    {
        yield return new WaitForSeconds(hiddenTime);

        currentState = State.Patrolling;
    }
}
