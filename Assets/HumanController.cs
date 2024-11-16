using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanController : MonoBehaviour
{
    [SerializeField] private Transform _targetsObject;
    
    [SerializeField] private float speed;

    private Transform[] _targets;
    
    private int _currentTargetIndex = 0;
    

    private RaycastHit[] hit;

    [SerializeField] float radius;

    public Vector3 currentTargetPosition;

    private BaseCharacter baseCharacter;

    private void Awake()
    {
        baseCharacter = GetComponent<BaseCharacter>();
        
        int targetID = _targetsObject.childCount;
       
        _targets = new Transform[targetID];
        

        for (int i = 0; i < targetID; i++)
        {
            _targets[i] = _targetsObject.GetChild(i);
        }
    }

    void Update()
    {
        MoveTowardsCurrentTarget();

        SendRayToForward();

        SetRandomTargetIndex();

        baseCharacter.Hide();
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
                currentTargetPosition = baseCharacter.hideAreas[baseCharacter.currentHideAreaIndex].position;

                baseCharacter.isHiding = true;
            }
            else if(!baseCharacter.isHiding)
            {
                currentTargetPosition = _targets[_currentTargetIndex].position;
            }
        }
    }
}
