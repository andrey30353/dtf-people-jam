using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Message
{
    public Liver2D Owner;
    public string Text;
}

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private CloudMessageUI _messageUI;

    public GameObject ClickToNextMessage;

    public float Delay;

    public List<Message> Messages;


    public List<Liver2D> DialogueLivers;

    public UnityEvent OnComplete;

    private int messageIndex;

    private bool canClick;

    private IEnumerator Start()
    {
        /*foreach (var message in Messages)
        {
            message.SetActive(false);
        }*/

        yield return new WaitForSeconds(0.1f);

        canClick = true;

        //ClickToNextMessage.SetActive(true);

        ShowNextMessage();
    }

    private void Update()
    {
        if (canClick && Input.GetMouseButtonDown(0))
        {
            ShowNextMessage();
        }

    }

    private void ShowNextMessage()
    {
        if (messageIndex >= 0 && messageIndex < Messages.Count)
        {
            _messageUI.SetMessage(Messages[messageIndex]);
        }

        if (messageIndex == Messages.Count-1)
        {

        }

        if(messageIndex == Messages.Count)
        {
            print("OnComplete");
            OnComplete?.Invoke();

            gameObject.SetActive(false);
        }

        messageIndex++;
    }
}
