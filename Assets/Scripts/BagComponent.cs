using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class BagComponent : MonoBehaviour
{
    public Dictionary<int,ItemEntity> items;
    public int initialBagSize = 4;
    public Action<int,ItemEntity> OnItemRemoved;
    public Action<int,ItemEntity> OnItemAdded;

    private void Awake()
    {
        items = new Dictionary<int, ItemEntity>();
        for (int i = 0; i < 4; i++)
        {
            items.Add(i,null);
        }
    }

    public void AddItem(ItemEntity itemEntity)
    {
        int index = 0;

        foreach (KeyValuePair<int, ItemEntity> keyValuePair in items)
        {
            if (keyValuePair.Value == null)
            {
                items[keyValuePair.Key] = itemEntity;
                OnItemAdded?.Invoke(index,itemEntity);
                break;
            }
            index++;
        }
    }

    public bool IsBagFull()
    {
        foreach (KeyValuePair<int, ItemEntity> keyValuePair in items)
        {
            if (keyValuePair.Value == null)
            {
                return false;
            }
        }

        return true;
    }

    public ItemEntity GetItem(int order)
    {
        if (order > items.Count)
            return null;

        return items[order - 1];
    }

    public void RemoveItem(ItemEntity itemEntity)
    {
        int index = 0;

        foreach (KeyValuePair<int, ItemEntity> keyValuePair in items)
        {
            if (keyValuePair.Value == itemEntity)
            {
                items[keyValuePair.Key] = null;
                OnItemRemoved?.Invoke(index,itemEntity);
                break;
            }
            index++;
        }
    }
}
