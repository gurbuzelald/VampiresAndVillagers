using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacter : MonoBehaviour
{
    public bool isHidden;
    public bool isHiding;

    [SerializeField] float hiddenTime;

    public Transform _hideAreasObject;

    public Transform[] hideAreas;

    public int currentHideAreaIndex = 0;


    void Awake()
    {
        isHidden = false;

        currentHideAreaIndex = 0;
    }

    IEnumerator DelaySetHiddenFalse()
    {
        yield return new WaitForSeconds(hiddenTime);

        isHidden = false;
        isHiding = false;

        yield return null;
    }

    public void Hide()
    {
        if (isHiding)
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
                isHidden = true;

                StartCoroutine(DelaySetHiddenFalse());
            }
        }
    }
}
