using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacter : MonoBehaviour
{
    public bool isHidden;
    public bool isHiding;

    [SerializeField] float hiddenTime;

    [SerializeField] private Transform _hideAreasObject;

    public Transform[] hideAreas;

    public int currentHideAreaIndex = 0;


    void Awake()
    {
        isHidden = false;

        int hideAreaID = _hideAreasObject.childCount;

        hideAreas = new Transform[hideAreaID];

        for (int i = 0; i < hideAreaID; i++)
        {
            hideAreas[i] = _hideAreasObject.GetChild(i);
        }

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
            for (int i = 0; i < hideAreas.Length; i++)
            {
                float tempMinDistance = Vector3.Distance(transform.position, hideAreas[0].position);

                if (tempMinDistance > Vector3.Distance(transform.position, hideAreas[i].position))
                {
                    tempMinDistance = Vector3.Distance(transform.position, hideAreas[i].position);

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
