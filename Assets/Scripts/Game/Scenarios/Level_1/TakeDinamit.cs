using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TakeDinamit : MonoBehaviour
{
    private enum State
    {
        WaitLiverSelect,
        LiverMove,
        ClickDoor,
        UnblockDoor,
        //TakeDinamit,
        BringDinamit,
        PutDinamit,


        Complete

    }

    public CameraMover CameraMover;

    public GameObject LiverMoveMessage;
    public GameObject ClickDoorMessage;
    public GameObject UnblockDoorMessage;
    public GameObject TakeDinamitMessage;
    public GameObject PutDinamitMessage;

    [Space]
    public BoxCollider2D DoorTrigger;
    public BoxCollider2D BringDinamitTrigger;

    [Space]
    public GameObject ClickDoorMarker;
    public GameObject BringDinamitMarker;

    public float Delay;

    public List<MoveAnimation> MoveLivers;

    public Liver2D ScenarioLiver;

    public PlayerRay2D playerRay;

    public Door2D ClickDoor;
    public Door2D NeedUnblockDoor;

    public UnityEvent OnComplete;

    private Equipment dinamit;

    private State state;

    private void Start()
    {
        LiverMoveMessage.SetActive(true);


        foreach (var moveAnimation in MoveLivers)
        {
            moveAnimation.enabled = true;
        }

        state = State.WaitLiverSelect;

        //ScenarioLevel1.Instance.ScenarionLiver.mover.RestoreMove();

        //ShowNextMessage();
    }


    private void FixedUpdate()
    {
        // выделили чувака
        if (state == State.WaitLiverSelect && playerRay.selectedAgent == ScenarioLiver)
        {
           // print("SelectLiver");
            ScenarioLiver.mover.RestoreMove();
            // ScenarioLiver.mover.Speed = 3;


            state = State.LiverMove;
        }

        // подошли к двери
        if (state == State.LiverMove && DoorTrigger.OverlapPoint(ScenarioLiver.transform.position))
        {
           // print("Visited");
            LiverMoveMessage.SetActive(false);
            ClickDoorMessage.SetActive(true);

            ClickDoorMarker.SetActive(false);

            state = State.ClickDoor;
        }

        // кликнули на дверь
        if (state == State.ClickDoor && ClickDoor.IsOpen)
        {
           // print("Open door");
            ClickDoorMessage.SetActive(false);
            UnblockDoorMessage.SetActive(true);

            state = State.UnblockDoor;
        }

        // принесли карту, открыли дверь
        if (state == State.UnblockDoor && NeedUnblockDoor.IsOpen)
        {
           // print("Ublock door");
            UnblockDoorMessage.SetActive(false);
            TakeDinamitMessage.SetActive(true);

            BringDinamitMarker.SetActive(true);

            CameraMover.MinX_MinZoom = -40;
            CameraMover.MinX_MaxZoom = -60;

            state = State.BringDinamit;
        }

        if (state == State.BringDinamit && ScenarioLiver.Equipment == null)
        {
           // print("Almost Bring Dinamit");
            if (BringDinamitTrigger.OverlapPoint(dinamit.transform.position))
            {
                print("Bring Dinamit");

                TakeDinamitMessage.SetActive(false);
                PutDinamitMessage.SetActive(true);

                BringDinamitMarker.SetActive(false);

                dinamit = ScenarioLiver.Equipment;

                state = State.PutDinamit;
            }

        }

        if (state == State.PutDinamit && ScenarioLiver.Equipment == null)
        {
            // todo мб дистанцию проверять
            if (BringDinamitTrigger.OverlapPoint(ScenarioLiver.transform.position))
            {
                print("PutDinamit");
                LiverMoveMessage.SetActive(false);
                ClickDoorMessage.SetActive(true);

                state = State.Complete;
            }


          
        }
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
