using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHealthUi : MonoBehaviour
{
    [SerializeField] private TMP_Text health;

    public void HandlePlayerHealthChanged(int healthValue)
    {
        health.text = healthValue.ToString();
    }
}
