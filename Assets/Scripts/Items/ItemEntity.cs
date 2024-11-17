using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ItemType
{
    None,
    OneHand,
    TwoHand
}

public class ItemEntity : MonoBehaviour
{
    public string itemName;

    public ItemType itemType;

    public SphereCollider trigerCollider;

    public string itemMessage;

    public bool isGrabbed;

    public Entity grabedEntity;

    private void Awake()
    {
        trigerCollider = GetComponent<SphereCollider>();
    }

    public void SetGrabbedState(bool state)
    {
        trigerCollider.enabled = !state;

        isGrabbed = state;

        if (!state)
        {
            transform.parent = null;
            transform.rotation = Quaternion.identity;
            transform.position = new Vector3(transform.position.x,1f,transform.position.z);
        }
    }
}
