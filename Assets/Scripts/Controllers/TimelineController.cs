using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TimelineController : MonoBehaviour
{
    [SerializeField] private float GameTime;
    [SerializeField] private bool IsGameTimeStarted;

    private const float TotalGameTime = 200;

    public Action OnGameTimeStarted;
    public Action OnGameTimeFinished;
    public Action<float,float> OnTimeUpdated;
    public void StartGameTime()
    {
        IsGameTimeStarted = true;
        OnGameTimeStarted?.Invoke();
    }

    private void Update()
    {
        if (IsGameTimeStarted == false)
            return;
        
        GameTime += Time.deltaTime;

        OnTimeUpdated?.Invoke(GameTime,TotalGameTime);

        if (GameTime >= TotalGameTime)
        {
            FinishGameTime();
        }
    }

    public void FinishGameTime()
    {
        IsGameTimeStarted = false;
        OnGameTimeFinished?.Invoke();
    }
}
