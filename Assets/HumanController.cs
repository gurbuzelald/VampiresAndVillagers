using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanController : BaseCharacter
{    
    [SerializeField] private float speed;

    private Transform[] _targets;
    
    private int _currentTargetIndex = 0;
    

    private RaycastHit[] hit;

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

    void Update()
    {
        MoveTowardsCurrentTarget();

        SetRandomTargetIndex();

        HumanStates(CheckAround(ref hit, transform, radius, ref layerMask), ref currentTargetPosition, _targets[_currentTargetIndex],
                    ref isHidding, ref isEscaping, ref isPatrolling);
    }

    void SetRandomTargetIndex()
    {
        if (Vector3.Distance(transform.position, _targets[_currentTargetIndex].position) < 0.1f
            && !isHidding)
        {
            _currentTargetIndex = Random.RandomRange(0, _targets.Length);
        }
    }

    private void MoveTowardsCurrentTarget()
    {
        if (!isHidding)
        {
            transform.LookAt(currentTargetPosition);

            transform.position = Vector3.MoveTowards(transform.position, currentTargetPosition, speed * Time.deltaTime);
        }
    }
}
