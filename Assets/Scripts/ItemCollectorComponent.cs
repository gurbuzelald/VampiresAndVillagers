using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollectorComponent : MonoBehaviour
{
    public ItemEntity targetItem;
    public BagComponent bagComponent;
    public Hand rightHand;
    public Hand leftHand;

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
                        Debug.LogError("No Hand");
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
                    targetHand.SetHandToItem(targetItem);
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
            rightHand.SetHandToItem(itemEntity);
        }
        else
        {
            if (rightHand.currentItemOnHand == null)
            {
                rightHand.SetHandToItem(itemEntity);
            }
            else if (leftHand.currentItemOnHand == null)
            {
                if (rightHand.currentItemOnHand.itemType == ItemType.TwoHand)
                {
                    AddItemToBage(rightHand);

                    rightHand.SetHandToItem(itemEntity);
                }
                else
                {
                    
                    leftHand.SetHandToItem(itemEntity);
                }
            }
            else
            {
                if (rightHand.currentItemOnHand.itemType == ItemType.TwoHand)
                {
                    NoHand();

                    rightHand.SetHandToItem(itemEntity);
                }
                else
                {
                    AddItemToBage(leftHand);
                   leftHand.SetHandToItem(itemEntity);
                }
           
            }

        }


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
            SwapItemToHand(rightHand,leftHand);
        }
        else if (leftHand.currentItemOnHand != null)
        {
            bagComponent.RemoveItem(leftHand.currentItemOnHand);
            leftHand.currentItemOnHand.SetGrabbedState(false);
        }
    
    }


    public void AddItemToBage(Hand hand)
    {
        if (hand.currentItemOnHand != null)
        {
            hand.currentItemOnHand.gameObject.SetActive(false);
            hand.currentItemOnHand = null;
        }
    }

    public void SwapItemToHand(Hand currentItemHand,Hand targetItemHand)
    {
        ItemEntity itemEntity = currentItemHand.currentItemOnHand;

        ItemEntity itemEntity2 = targetItemHand.currentItemOnHand;

        if (itemEntity != null)
        {
            targetItemHand.SetHandToItem(itemEntity);
        }

        if (itemEntity2 != null)
        {
           currentItemHand.SetHandToItem(itemEntity2);
        }
    }
}


