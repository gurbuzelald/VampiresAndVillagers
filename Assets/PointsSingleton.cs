using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsSingleton : MonoBehaviour
{
    private static PointsSingleton _instance;

    public Transform hideAreasObject;
    public Transform vampireTargetsObject;
    public Transform humanTargetsObject;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
