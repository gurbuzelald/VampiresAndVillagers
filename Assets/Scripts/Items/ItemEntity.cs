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
    public ItemType itemType;

    public SphereCollider trigerCollider;

    public string itemMessage;

    private void Awake()
    {
        trigerCollider = GetComponent<SphereCollider>();
    }

    public void SetGrabbedState(bool state)
    {
        trigerCollider.enabled = !state;
    }
}
