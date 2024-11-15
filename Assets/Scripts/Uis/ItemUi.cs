using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemUi : MonoBehaviour
{
    [SerializeField] private Transform ligtingUiParent;

    [SerializeField] private Transform gunUiParent;

    [SerializeField] private TMP_Text totalBulletAmountText;

    [SerializeField] private TMP_Text ligtingRemainingPercentageText;

    public void SetStateGunUi(bool state)
    {
        gunUiParent.gameObject.SetActive(state);
    }

    public void SetStateLigtingUi(bool state)
    {
        ligtingUiParent.gameObject.SetActive(state);
    }

    public void HandleAmountGun(int currentBullet,int totalBullet)
    {
        totalBulletAmountText.text = (currentBullet) + "/" + (totalBullet - currentBullet);
    }

    public void HandleLightingAmount(int current, int total)
    {
        ligtingRemainingPercentageText.text = "%" + (((float)current / (float)total)) * 100f;
    }
}
