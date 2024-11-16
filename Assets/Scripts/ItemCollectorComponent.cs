using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollectorComponent : MonoBehaviour
{
    public ItemEntity targetItem;
    public BagComponent bagComponent;
    public ItemEntity currentItem;

    private void Awake()
    {
        this.gameObject.AddComponent<BagComponent>();
        bagComponent = GetComponent<BagComponent>();
    }

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
                if (bagComponent.IsBagFull())
                {
                    MessageUi.HideItemMessage();
                    MessageUi.ShowItemMessage("Bag Is Full");
                }
                else
                {
                    MessageUi.HideItemMessage();
                    if (currentItem != null)
                    {
                        currentItem.gameObject.SetActive(false);
                    }

                    targetItem.SetGrabbedState(true);
                    targetItem.transform.parent = this.transform;
                    targetItem.transform.localRotation = Quaternion.identity;
                    targetItem.transform.localPosition = Vector3.up + Vector3.forward;
                    currentItem = targetItem;
                    bagComponent.AddItem(targetItem);
                    targetItem = null;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (currentItem != null)
            {
                DropItem();
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ItemEntity itemEntity = bagComponent.GetItem(1);
            if (itemEntity != null)
                UseItemOnBage(itemEntity);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ItemEntity itemEntity = bagComponent.GetItem(2);
            if (itemEntity != null)
                UseItemOnBage(itemEntity);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ItemEntity itemEntity = bagComponent.GetItem(3);
            if (itemEntity != null)
                UseItemOnBage(itemEntity);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ItemEntity itemEntity = bagComponent.GetItem(4);
            if (itemEntity != null)
                UseItemOnBage(itemEntity);
        }
    }

    private void UseItemOnBage(ItemEntity itemEntity)
    {
        if (currentItem != null)
            currentItem.gameObject.SetActive(false);

        currentItem = itemEntity;
        currentItem.gameObject.SetActive(true);
    }

    public void DropItem()
    {
        bagComponent.RemoveItem(currentItem);
        currentItem.SetGrabbedState(false);
        currentItem = null;
    }
}


