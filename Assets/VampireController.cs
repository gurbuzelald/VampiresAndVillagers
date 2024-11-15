using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampireController : MonoBehaviour
{

    private Vector3 start;
    private Vector3 end;
    private float speed;

    private int randomNumber;

    [SerializeField] Transform _targetsObject;

    private int currentTargetID;
    // Start is called before the first frame update

    private void Awake()
    {
        currentTargetID = 1;
    }
    void Start()
    {
        speed = 0.5f;
        start = _targetsObject.transform.GetChild(0).position;
        end = _targetsObject.transform.GetChild(1).position;

        
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Vector3.Distance(transform.position, _targetsObject.position) < .1f)
        {
            if (currentTargetID < _targetsObject.childCount)
            {
                currentTargetID++;
            }
            else
            {
                currentTargetID = 0;
            }
            
        }*/
        
        transform.position = Vector3.Lerp(_targetsObject.transform.GetChild(0).position, _targetsObject.transform.GetChild(1).position, Mathf.PingPong(Time.time * speed, 1));

        //gameObject.transform.LookAt(_targetsObject.GetChild(currentTargetID));
    }
}
