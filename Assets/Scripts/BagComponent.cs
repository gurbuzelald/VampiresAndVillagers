using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagComponent : MonoBehaviour
{
    public List<ItemEntity> items;

    public int initialBagSize;

    private void Awake()
    {
        items = new List<ItemEntity>(initialBagSize);
    }

    public void AddItem(ItemEntity itemEntity)
    {
        if (items.Count < initialBagSize)
        {
            items.Add(itemEntity);
        }
        else
        {
            Debug.Log("No Place For Item In Bag");
        }
    }
}
