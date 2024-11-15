using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class MessageUi : MonoBehaviour
{
    [SerializeField] private TMP_Text messageText;

    private Queue<string> messages = new Queue<string>();

    private bool showingMessage;

    public void ShowMessage(string currentMessage)
    {
        messages.Enqueue(currentMessage);

        if(showingMessage==false)
        {
            StartCoroutine(ShowMessageCoroutine());
        }
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
}
