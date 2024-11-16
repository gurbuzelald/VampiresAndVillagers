using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagComponent : MonoBehaviour
{
    public List<ItemEntity> items;

    public int initialBagSize = 4;

    private void Awake()
    {
        items = new List<ItemEntity>();
    }

    public void AddItem(ItemEntity itemEntity)
    {
        items.Add(itemEntity);
    }

    public bool IsBagFull()
    {
        return items.Count >= initialBagSize;
    }

    public ItemEntity GetItem(int order)
    {
        if (order > items.Count)
            return null;

        return items[order - 1];
    }

    public void RemoveItem(ItemEntity itemEntity)
    {
        items.Remove(itemEntity);
    }
}
