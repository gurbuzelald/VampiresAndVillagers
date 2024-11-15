using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampireController : MonoBehaviour
{
    [SerializeField] private Transform _targetsObject;
    [SerializeField] private float speed;

    private Transform[] _targets;
    private int _currentTargetIndex = 0;

    private bool isSeeingHuman;

    private RaycastHit hit;
    private Ray ray;

    private GameObject currentHumanObject;

    private void Awake()
    {
        isSeeingHuman = false;

        int targetCount = _targetsObject.childCount;
        _targets = new Transform[targetCount];
        for (int i = 0; i < targetCount; i++)
        {
            _targets[i] = _targetsObject.GetChild(i);
        }
    }

    void Update()
    {
        if (!isSeeingHuman)
        {
            if (Vector3.SqrMagnitude(transform.position - _targets[_currentTargetIndex].position) < 0.1f * 0.1f)
            {
                _currentTargetIndex = Random.Range(0, _targets.Length);
            }

            MoveTowardsCurrentTarget(_targets[_currentTargetIndex]);
        }
        else
        {
            MoveTowardsCurrentTarget(currentHumanObject.transform);
        }

        SendRayToForward();
    }

    private void MoveTowardsCurrentTarget(Transform currentTarget)
    {
        Vector3 targetPosition = currentTarget.position;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        transform.LookAt(targetPosition);
    }

    private void SendRayToForward()
    {
        ray = new Ray(transform.position, transform.forward);
        ray.origin = transform.position;

        if (Physics.Raycast(ray, out hit, 100f))
        {
            if (hit.collider.CompareTag("Human") && hit.collider.gameObject != currentHumanObject)
            {
                currentHumanObject = hit.collider.gameObject;
                isSeeingHuman = true;
            }
        }
        else
        {
            Debug.DrawRay(ray.origin, ray.direction * 100f, Color.green);
        }
    }
}
