using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanController : MonoBehaviour
{
    [SerializeField] private Transform _targetsObject;
    [SerializeField] private Transform _hideAreasObject;
    [SerializeField] private float speed;

    private Transform[] _targets;
    private Transform[] _hideAreas;
    private int _currentTargetIndex = 0;
    private int _currentHideAreaIndex = 0;

    private RaycastHit[] hit;

    [SerializeField] float radius;

    public Vector3 currentTargetPosition;


    public bool isHiding;

    [SerializeField] float hiddenTime;

    private BaseCharacter baseCharacter;

    private void Awake()
    {
        isHiding = false;

        baseCharacter = GetComponent<BaseCharacter>();
        baseCharacter.isHidden = false;

        int targetID = _targetsObject.childCount;
        int hideAreaID = _hideAreasObject.childCount;

        _targets = new Transform[targetID];
        _hideAreas = new Transform[hideAreaID];

        for (int i = 0; i < targetID; i++)
        {
            _targets[i] = _targetsObject.GetChild(i);
        }

        for (int i = 0; i < hideAreaID; i++)
        {
            _hideAreas[i] = _hideAreasObject.GetChild(i);
        }

        _currentHideAreaIndex = 0;
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

    void Hide()
    {
        if (isHiding)
        {
            for (int i = 0; i < _hideAreas.Length; i++)
            {
                float tempMinDistance = Vector3.Distance(transform.position, _hideAreas[0].position);

                if (tempMinDistance > Vector3.Distance(transform.position, _hideAreas[i].position))
                {
                    tempMinDistance = Vector3.Distance(transform.position, _hideAreas[i].position);

                    _currentHideAreaIndex = i;
                }
            }

            if (Vector3.Distance(transform.position, _hideAreas[_currentHideAreaIndex].position) < .1f)
            {
                baseCharacter.isHidden = true;

                StartCoroutine(DelaySetHiddenFalse());
            }
        }
    }

    IEnumerator DelaySetHiddenFalse()
    {
        yield return new WaitForSeconds(hiddenTime);

        baseCharacter.isHidden = false;
        isHiding = false;

        yield return null;
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
                currentTargetPosition = _hideAreas[_currentHideAreaIndex].position;

                isHiding = true;
            }
            else if(!isHiding)
            {
                currentTargetPosition = _targets[_currentTargetIndex].position;
            }
        }
    }
}
