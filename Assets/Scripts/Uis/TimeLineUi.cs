using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class TimeLineUi : MonoBehaviour
{
    [SerializeField] private Image timeLineSlider;
    [SerializeField] private TMP_Text timeText;

    public void HandleTimeUpdated(float passedTime,float totalTime)
    {
        timeText.text = (totalTime - passedTime).ToString("0");

        timeLineSlider.fillAmount = (totalTime - passedTime) / totalTime;
    }
}
