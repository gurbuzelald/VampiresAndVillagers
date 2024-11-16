using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ItemType
{
    None,
    OneHand,
    TwoHand
}

public class ItemEntity : MonoBehaviour
{
    public ItemType itemType;
}
