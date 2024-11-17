using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ItemCollectorComponent : MonoBehaviour
{
    public ItemEntity targetItem;
    public BagComponent bagComponent;
    public Hand rightHand;
    public Hand leftHand;
    public Action<ItemEntity> OnTakeItem;
    public Action<ItemEntity> DiscardItem;

    private void Awake()
    {
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

                    Hand targetHand = null;

                    ItemType itemType = targetItem.itemType;

                    if (itemType == ItemType.TwoHand)
                    {
                        NoHand();
                        targetHand = rightHand;
                    }
                    else
                    {
                        if (rightHand.currentItemOnHand != null)
                        {
                            if (rightHand.currentItemOnHand.itemType == ItemType.TwoHand)
                            {
                                AddItemToBage(rightHand);
                                targetHand = rightHand;
                            }
                            else
                            {
                                if (leftHand.currentItemOnHand != null)
                                {
                                    AddItemToBage(rightHand);
                                    targetHand = rightHand;
                                }
                                else
                                {
                                    targetHand = leftHand;
                                }
                            }
                        }
                        else
                        {
                            if (rightHand.currentItemOnHand == null)
                            {
                                targetHand = rightHand;
                            }
                            else if (leftHand.currentItemOnHand == null)
                            {
                                targetHand = leftHand;
                            }
                            else
                            {
                                AddItemToBage(leftHand);
                                targetHand = leftHand;
                            }

                        }
                       
                    }
              
                    targetItem.SetGrabbedState(true);
                    targetItem.grabedEntity = GetComponentInParent<Entity>();
                    SetItemToHand(targetHand,targetItem);
                    bagComponent.AddItem(targetItem);
                    targetItem = null;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            DropItem();
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
        if (itemEntity.itemType == ItemType.TwoHand)
        {
            NoHand();
            SetItemToHand(rightHand,itemEntity);
        }
        else
        {
            if (rightHand.currentItemOnHand == null)
            {
                SetItemToHand(rightHand,itemEntity);
            }
            else if (leftHand.currentItemOnHand == null)
            {
                if (rightHand.currentItemOnHand.itemType == ItemType.TwoHand)
                {
                    AddItemToBage(rightHand);
                    SetItemToHand(rightHand,itemEntity);
                }
                else
                {
                    SetItemToHand(leftHand,itemEntity);
                }
            }
            else
            {
                if (rightHand.currentItemOnHand.itemType == ItemType.TwoHand)
                {
                    NoHand();
                    SetItemToHand(rightHand,itemEntity);
                }
                else
                {
                    AddItemToBage(leftHand);
                    SetItemToHand(leftHand,itemEntity);
                } 
            }
        }
    }

    public void SetItemToHand(Hand hand,ItemEntity itemEntity)
    {
       hand.SetHandToItem(itemEntity);
       OnTakeItem?.Invoke(itemEntity);
    }

    public void NoHand()
    {
        AddItemToBage(rightHand);
        AddItemToBage(leftHand);
    }

    public void DropItem()
    {
        if (rightHand.currentItemOnHand != null)
        {
            bagComponent.RemoveItem(rightHand.currentItemOnHand);
            rightHand.currentItemOnHand.SetGrabbedState(false);
            DiscardItem?.Invoke(rightHand.currentItemOnHand);
            rightHand.currentItemOnHand = null;
            SwapItemToHand(rightHand,leftHand);
        }
        else if (leftHand.currentItemOnHand != null)
        {
            bagComponent.RemoveItem(leftHand.currentItemOnHand);
            leftHand.currentItemOnHand.SetGrabbedState(false);
            DiscardItem?.Invoke(leftHand.currentItemOnHand);
            leftHand.currentItemOnHand = null;
        }
    }

    public void AddItemToBage(Hand hand)
    {
        if (hand.currentItemOnHand != null)
        {
            hand.currentItemOnHand.gameObject.SetActive(false);
            DiscardItem?.Invoke(hand.currentItemOnHand);
            hand.currentItemOnHand = null;

        }
    }

    public void SwapItemToHand(Hand currentItemHand,Hand targetItemHand)
    {
        ItemEntity itemEntity = currentItemHand.currentItemOnHand;

        ItemEntity itemEntity2 = targetItemHand.currentItemOnHand;

        if (itemEntity != null)
        {
            SetItemToHand(targetItemHand,itemEntity);
        }

        if (itemEntity2 != null)
        {
            SetItemToHand(currentItemHand, itemEntity2);
        }
    }
}


