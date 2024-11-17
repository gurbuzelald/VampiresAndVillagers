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

        playerEntity.heroPlayerController.FlashLight.OnFlashLightActivateStateChanged += gamePlayUiController.itemUi.SetStateLigtingUi;

        playerEntity.heroPlayerController.FlashLight.OnFlashLightAmountChanged += gamePlayUiController.itemUi.HandleLightingAmount;

        PlayerBagUi playerBagUi = gamePlayUiController.playerBagUi;

        BagComponent bagComponent = playerEntity.heroPlayerController.GetComponent<BagComponent>();

        playerBagUi.InitiliaPlayerBagUi(bagComponent.items);

        bagComponent.OnItemAdded += playerBagUi.AddToSlot;

        bagComponent.OnItemRemoved += playerBagUi.RemoveToSlot;

        ItemUi itemUi = gamePlayUiController.itemUi;

        ItemCollectorComponent itemCollectorComponent = playerEntity.GetComponentInChildren<ItemCollectorComponent>();

        itemCollectorComponent.OnTakeItem += gamePlayUiController.itemUi.SetItemUi;
        itemCollectorComponent.DiscardItem += gamePlayUiController.itemUi.DiscardItemUi;
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