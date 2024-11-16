using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BagSlotUi : MonoBehaviour
{
    public TMP_Text itemIdText;

    public void SetAsFilled(string itemId)
    {
        itemIdText.text = itemId;
    }

    public void SetAsEmpty(int index)
    {
        itemIdText.text = index.ToString();
    }
}
