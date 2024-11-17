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


    private void Awake()
    {
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
    }

    void FixedUpdate()
    {
        RaycastHit[] hits = CheckAround(transform, radius, layerMask);

        MoveTowardsCurrentTarget();

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

    private void MoveTowardsCurrentTarget()
    {
        if (currentState != State.Hiding)
        {
            transform.LookAt(currentTargetPosition);

            transform.position = Vector3.MoveTowards(transform.position, currentTargetPosition, speed * Time.deltaTime);
        }
    }

    private RaycastHit[] CheckAround(Transform _transform, float radius, LayerMask layerMask)
    {
        return Physics.SphereCastAll(_transform.position, radius, _transform.forward, radius, layerMask);
    }
}
