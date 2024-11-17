using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public ItemEntity currentItemOnHand;

    public Transform spawnPointItem;

    public void SetHandToItem(ItemEntity itemEntity)
    {
        currentItemOnHand = itemEntity;
        itemEntity.transform.parent = this.transform;
        itemEntity.gameObject.SetActive(true);

        itemEntity.transform.position = spawnPointItem.transform.position;

        itemEntity.transform.rotation = spawnPointItem.transform.rotation;
    }
}
