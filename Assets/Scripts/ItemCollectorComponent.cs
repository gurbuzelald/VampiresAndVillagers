using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollectorComponent : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Item"))
        {
            ItemEntity itemEntity = other.GetComponent<ItemEntity>();

            string itemMessage = itemEntity.itemMessage;

            MessageUi.ShowItemMessage(itemMessage);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Item"))
        {
            MessageUi.HideItemMessage();
        }
    }
}
