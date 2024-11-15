using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanController : MonoBehaviour
{
    [SerializeField] private Transform _targetsObject;
    [SerializeField] private float speed;

    private Transform[] _targets;
    private int _currentTargetIndex = 0;

    private void Awake()
    {
        int targetCount = _targetsObject.childCount;
        _targets = new Transform[targetCount];
        for (int i = 0; i < targetCount; i++)
        {
            _targets[i] = _targetsObject.GetChild(i);
        }
    }

    void Update()
    {

        MoveTowardsCurrentTarget();

        if (Vector3.Distance(transform.position, _targets[_currentTargetIndex].position) < 0.1f)
        {
            _currentTargetIndex = Random.RandomRange(0, _targets.Length);
        }
    }

    private void MoveTowardsCurrentTarget()
    {
        Vector3 targetPosition = _targets[_currentTargetIndex].position;

        transform.LookAt(targetPosition);

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }
}
