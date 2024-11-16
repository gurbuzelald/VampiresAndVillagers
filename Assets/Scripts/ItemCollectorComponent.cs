using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollectorComponent : MonoBehaviour
{
    public ItemEntity targetItem;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Item"))
        {
            ItemEntity itemEntity = other.GetComponent<ItemEntity>();

            string itemMessage = itemEntity.itemMessage;

            targetItem = itemEntity;

            MessageUi.ShowItemMessage(itemMessage);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Item"))
        {
            MessageUi.HideItemMessage();
            targetItem = null;
        }
    }

    private void Update()
    {
        if (targetItem != null)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                MessageUi.HideItemMessage();
                targetItem.SetGrabbedState(true);
                targetItem.transform.parent = this.transform;
                targetItem.transform.localRotation = Quaternion.identity;
                targetItem.transform.localPosition = Vector3.up + Vector3.forward;
                targetItem = null;
            }
        }
    }
}
