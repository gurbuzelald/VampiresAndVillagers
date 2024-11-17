using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacter : MonoBehaviour
{
    public enum State
    {
        Patrolling,
        Escaping,
        Hiding
    }

    public State currentState;

    [SerializeField] private float hiddenTime;

    public Transform _hideAreasObject;
    public Transform[] hideAreas;

    private int currentHideAreaIndex = 0;

    private void Awake()
    {
        currentState = State.Patrolling;
        currentHideAreaIndex = 0;
    }


    public void HandleStates(RaycastHit[] hits, ref Vector3 currentTargetPosition, Transform patrolTarget)
    {
        switch (currentState)
        {
            case State.Patrolling:
                Patrol(ref currentTargetPosition, patrolTarget);
                CheckForVampires(hits); // Transition to Escaping if vampires are detected
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
        currentTargetPosition = patrolTarget.position;
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

        currentTargetPosition = hideAreas[currentHideAreaIndex].position;

        if (Vector3.Distance(transform.position, hideAreas[currentHideAreaIndex].position) < 0.1f)
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
