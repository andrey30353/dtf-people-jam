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

    public GameObject HintUI;

    public float Delay;

    public List<Message> Messages;

    public UnityEvent OnComplete;

    private int _messageIndex;

    private void Start()
    {
        _messageUI.gameObject.SetActive(true);

        if (HintUI != null)
            HintUI.SetActive(true);

        ShowNextMessage();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ShowNextMessage();
        }
    }

    private void ShowNextMessage()
    {
        if (_messageIndex >= 0 && _messageIndex < Messages.Count)
        {
            _messageUI.SetMessage(Messages[_messageIndex]);
        }

        if(_messageIndex == Messages.Count)
        {
            OnComplete?.Invoke();

            _messageUI.gameObject.SetActive(false);

            if(HintUI != null)
                HintUI.SetActive(false);

            gameObject.SetActive(false);
        }

        _messageIndex++;
    }
}
