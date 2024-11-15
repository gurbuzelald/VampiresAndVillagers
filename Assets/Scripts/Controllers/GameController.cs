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
        gamePlayUiController.messageUi.ShowMessage(MessageConstants.GameStarted);
    }

    public void HandleGameTimeFinished()
    {
        gamePlayUiController.messageUi.ShowMessage(MessageConstants.GameFinished);
    }
}

public class MessageConstants
{
    public static string GameStarted = "Game Started";

    public static string GameFinished = "Game Finished";

}