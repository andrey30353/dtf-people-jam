using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TakeDinamit : MonoBehaviour
{
    public GameObject LiverMoveMessage;

    public float Delay;

    public List<MoveAnimation> MoveLivers;

    public UnityEvent OnComplete;

    private int messageIndex;

    private void Start()
    {    
        LiverMoveMessage.SetActive(true);


        foreach (var moveAnimation in MoveLivers)
        {
            moveAnimation.enabled  = true;
        }

        //ShowNextMessage();
    }

    private void Update()
    {
        //if(Input.GetMouseButtonDown(0))
        //{
        //    print("Click");
        //    ShowNextMessage();
        //}

    } 

    private void ShowNextMessage()
    {
    //    if (messageIndex > 0)
    //    { 
    //        Messages[messageIndex - 1].SetActive(false);
    //    }

    //    if (messageIndex < Messages.Count)
    //    {
    //        Messages[messageIndex].SetActive(true);
    //    }

    //    if (messageIndex >= Messages.Count)
    //    {
    //        print("OnComplete");
    //        OnComplete?.Invoke();
    //    }

    //    messageIndex++;
    }
}
