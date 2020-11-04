using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NeedExposionDialogue : MonoBehaviour
{
    public GameObject ClickToNextMessage;

    public float Delay;

    public List<GameObject> Messages;

    public List<Liver2D> DialogueLivers;

    public UnityEvent OnComplete;

    private int messageIndex;

    private bool canClick;

    private IEnumerator Start()
    {
        foreach (var message in Messages)
        {
            message.SetActive(false);
        }

        yield return new WaitForSeconds(0.1f);

        canClick = true;

        ClickToNextMessage.SetActive(true);

        ShowNextMessage();
    }

    private void Update()
    {
        if (canClick && Input.GetMouseButtonDown(0))
        {
            print("Click");
            ShowNextMessage();
        }

    }

    private void ShowNextMessage()
    {
        if (messageIndex > 0)
        {
            Messages[messageIndex - 1].SetActive(false);
        }

        if (messageIndex < Messages.Count)
        {
            Messages[messageIndex].SetActive(true);
        }

        if (messageIndex >= Messages.Count-1)
        {
            print("OnComplete");

            foreach (var liver in DialogueLivers)
            {
                liver.mover.enabled = true;
                liver.mover.animator.enabled = true;
            }

            OnComplete?.Invoke();
        }

        messageIndex++;
    }
}
