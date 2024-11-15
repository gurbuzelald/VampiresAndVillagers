using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private GamePlayUiController gamePlayUiController;
    [SerializeField] private TimelineController timelineController;

    private void Start()
    {
        InitGame();
    }

    private void InitGame()
    {
        timelineController.OnTimeUpdated+=gamePlayUiController.timeLineUi.HandleTimeUpdated;

        timelineController.OnGameTimeStarted += HandleGameTimeStarted;

        timelineController.OnGameTimeFinished += HandleGameTimeFinished;

        timelineController.StartGameTime();
    }

    public void HandleGameTimeStarted()
    {

    }

    public void HandleGameTimeFinished()
    {


    }
}
