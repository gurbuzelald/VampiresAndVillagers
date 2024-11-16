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

    private void Awake()
    {
        pointsSingleton = FindAnyObjectByType<PointsSingleton>();

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

        SendRayToForward();

        SetRandomTargetIndex();

        Hide();
    }

    void SetRandomTargetIndex()
    {
        if (Vector3.Distance(transform.position, _targets[_currentTargetIndex].position) < 0.1f)
        {
            _currentTargetIndex = Random.RandomRange(0, _targets.Length);
        }
    }

    private void MoveTowardsCurrentTarget()
    {        
        transform.LookAt(currentTargetPosition);

        transform.position = Vector3.MoveTowards(transform.position, currentTargetPosition, speed * Time.deltaTime);
    }

    private void SendRayToForward()
    {
        hit = Physics.SphereCastAll(transform.position, radius, transform.forward, radius);

        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i].collider.CompareTag("Vampire"))
            {
                currentTargetPosition = hideAreas[currentHideAreaIndex].position;

                isHiding = true;
            }
            else if(!isHiding)
            {
                currentTargetPosition = _targets[_currentTargetIndex].position;
            }
        }
    }
}
