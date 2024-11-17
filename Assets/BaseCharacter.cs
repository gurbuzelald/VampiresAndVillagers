using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacter : MonoBehaviour
{
    public enum State
    {
        Patrolling,
        Escaping,
        Hiding,
        Attacking
    }

    public State currentState;

    public bool haveGun;

    public float hiddenTime;

    public Transform _hideAreasObject;
    public Transform[] hideAreas;

    public int currentHideAreaIndex = 0;
}
