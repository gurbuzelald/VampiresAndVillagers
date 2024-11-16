using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacter : MonoBehaviour
{
    public bool isHidding;
    public bool isEscaping;
    public bool isPatrolling;


    [SerializeField] float hiddenTime;

    public Transform _hideAreasObject;

    public Transform[] hideAreas;

    public int currentHideAreaIndex = 0;


    void Awake()
    {
        isHidding = false;

        currentHideAreaIndex = 0;
    }

    public void Hide(ref bool isHidding, ref bool isEscaping, ref bool isPatrolling)
    {
        if (isEscaping)
        {
            float nearestDistance = Mathf.Infinity;

            for (int i = 0; i < hideAreas.Length; i++)
            {
                float tempMinDistance = Vector3.Distance(transform.position, hideAreas[i].position);

                if (tempMinDistance < nearestDistance)
                {
                    nearestDistance = tempMinDistance;

                    currentHideAreaIndex = i;
                }
            }

            if (Vector3.Distance(transform.position, hideAreas[currentHideAreaIndex].position) < .1f)
            {
                isHidding = true;
                isEscaping = false;
                isPatrolling = false;

                StartCoroutine(DeActivateHidding(hiddenTime));
            }
        }
    }

    IEnumerator DeActivateHidding(float hiddenTime)
    {
        yield return new WaitForSeconds(hiddenTime);

        if (isHidding)
        {
            isHidding = false;

            isPatrolling = true;
        }
        yield return null;
    }

    public void HumanStates(RaycastHit[] hit, ref Vector3 currentTargetPosition, Transform patrolTarget,
                            ref bool isHidding, ref bool isEscaping, ref bool isPatrolling)
    {
        Hide(ref isHidding, ref isEscaping, ref isPatrolling);

        Patrol(hit, ref currentTargetPosition, patrolTarget,
              ref isHidding, ref isEscaping, ref isPatrolling);

        Escape(hit, ref currentTargetPosition,
               ref isHidding, ref isEscaping, ref isPatrolling);
    }

    public void Patrol(RaycastHit[] hit, ref Vector3 currentTarget, Transform patrolTarget,
                       ref bool isHidding, ref bool isEscaping, ref bool isPatrolling)
    {
        for (int i = 0; i < hit.Length; i++)
        {
            if (!isHidding && !isPatrolling)
            {
                currentTarget = patrolTarget.position;

                isPatrolling = true;
            }
            else
            {
                isPatrolling = false;
            }
        }
    }

    public void Escape(RaycastHit[] hit, ref Vector3 currentTarget,
                       ref bool isHidding, ref bool isEscaping, ref bool isPatrolling)
    {
        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i].collider.CompareTag("Vampire") && !isHidding && !isEscaping)
            {
                currentTarget = hideAreas[currentHideAreaIndex].position;

                isEscaping = true;
            }
            else
            {
                isEscaping = false;
            }
        }
    }

    public RaycastHit[] CheckAround(ref RaycastHit[] hit, Transform _transform, float radius, ref LayerMask layerMask)
    {
        hit = Physics.SphereCastAll(_transform.position, radius, _transform.forward, radius, layerMask);

        return hit;
    }
}
