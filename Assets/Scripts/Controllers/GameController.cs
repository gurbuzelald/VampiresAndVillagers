using System.Collections;
using System.Collections.Generic;
using AdvancedHorrorFPS;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private GamePlayUiController gamePlayUiController;
    [SerializeField] private TimelineController timelineController;
    [SerializeField] private Entity playerEntity;

    private void Start()
    {
        Cursor.visible = false;
        InitGame();
        SpawnCharacter();
    }

    private void InitGame()
    {
        timelineController.OnTimeUpdated+=gamePlayUiController.timeLineUi.HandleTimeUpdated;

        timelineController.OnGameTimeStarted += HandleGameTimeStarted;

        timelineController.OnGameTimeFinished += HandleGameTimeFinished;

        timelineController.StartGameTime();
    }

    public void SpawnCharacter()
    {
       playerEntity = Instantiate(playerEntity);

       playerEntity.gameObject.SetActive(true);

        playerEntity.heroPlayerController.healthComponent.OnHealthValueChanged += gamePlayUiController.playerHealthUi.HandlePlayerHealthChanged;

        gamePlayUiController.playerHealthUi.HandlePlayerHealthChanged(playerEntity.heroPlayerController.healthComponent.Health);
    }

    public void HandleGameTimeStarted()
    {
        MessageUi.ShowMessage(MessageConstants.GameStarted);
    }

    public void HandleGameTimeFinished()
    {
       MessageUi.ShowMessage(MessageConstants.GameFinished);
    }
}

public class MessageConstants
{
    public static string GameStarted = "Game Started";

    public static string GameFinished = "Game Finished";

}