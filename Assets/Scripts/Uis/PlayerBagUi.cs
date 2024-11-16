using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBagUi : MonoBehaviour
{
    private Dictionary<int, BagSlotUi> bagSlotUis = new Dictionary<int, BagSlotUi>();
    [SerializeField] private List<BagSlotUi> bagSlots;

    private void Awake()
    {
        int index = 0;

        foreach (BagSlotUi bagSlotUi in bagSlots)
        {
            bagSlotUis.Add(index,bagSlotUi);
            index++;
        }
    }

    public void InitiliaPlayerBagUi(Dictionary<int,ItemEntity> playerItems)
    {
        foreach (KeyValuePair<int, ItemEntity> keyValuePair in playerItems)
        {
            AddToSlot(keyValuePair.Key,keyValuePair.Value.itemName);
        }
    }

    public void AddToSlot(int index,string itemName)
    {
        BagSlotUi targetBagSlotUi = bagSlotUis[index];

        targetBagSlotUi.SetAsFilled(itemName);
    }

    public void RemoveToSlot(int index)
    {
        BagSlotUi targetBagSlotUi = bagSlotUis[index];

        targetBagSlotUi.SetAsEmpty(index);
    }
}
