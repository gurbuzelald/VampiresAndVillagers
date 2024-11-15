using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class MessageUi : MonoBehaviour
{
    [SerializeField] private TMP_Text messageText;

    [SerializeField] private TMP_Text itemMessage;

    private static Queue<string> messages = new Queue<string>();

    private static bool showingMessage;

    public static Action OnMessageAdded;

    public static Action<string> OnItemMessageAdded;

    public static Action OnItemMessageHide;

    private void Awake()
    {
        OnMessageAdded += () =>
         {
             if(showingMessage==false)
                StartCoroutine(ShowMessageCoroutine());
         };

        OnItemMessageAdded += (string message) =>
         {
             ItemMessage(message);
         };

        OnItemMessageHide += () =>
         {
             CloseItemMessage();   
         };
    }

    public static void ShowMessage(string currentMessage)
    {
        messages.Enqueue(currentMessage);

        OnMessageAdded?.Invoke();
    }

    private IEnumerator ShowMessageCoroutine()
    {
        showingMessage = true;
        messageText.text = messages.Dequeue();
        messageText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        showingMessage = false;

        if (messages.Count > 0)
        {
            StartCoroutine(ShowMessageCoroutine());
        }
        else
        {
            messageText.gameObject.SetActive(false);
        }
    }

    public static void ShowItemMessage(string message)
    {
        OnItemMessageAdded?.Invoke(message);
    }

    public void ItemMessage(string message)
    {
        itemMessage.text = message;
        itemMessage.gameObject.SetActive(true);
    }

    public void CloseItemMessage()
    {
        itemMessage.gameObject.SetActive(false);
    }

    public static void HideItemMessage()
    {
        OnItemMessageHide?.Invoke(); 
    }
}
