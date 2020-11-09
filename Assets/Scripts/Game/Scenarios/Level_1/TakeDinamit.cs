using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public enum Level1TutorialGameOver
{
    NobodyGoToShelter,

    Complete
}

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

        ExplodeDinamit,

        CaveEnter,
        CaveExplore,

        GoShelter,

        RadioSignal,

        Complete
    }

   
    public ScenarioLevel1 Tutorial;
    public Level1Progress Level1;

    public CameraMover CameraMover;

    public GameObject CaveBlock;

    public GameObject LiverMoveMessage;
    public GameObject ClickDoorMessage;
    public GameObject UnblockDoorMessage;
    public GameObject TakeDinamitMessage;
    public GameObject PutDinamitMessage;
    public GameObject CaveEnterMessage;
    public GameObject CaveExploreMessage;
    public GameObject GoShelterMessage;
    public GameObject RadioSignalMessage;
    public GameObject ScenarioGoalMessage;

    [Space]
    public BoxCollider2D DoorTrigger;
    public BoxCollider2D BringDinamitTrigger;
    public BoxCollider2D CaveEnterTrigger;   
    public BoxCollider2D CaveExploreTrigger;
    public BoxCollider2D GoShelterTrigger;
    public CircleCollider2D RadioSignalTrigger;
    public StopZone RadioSignalZone;

    [Space]
    public GameObject ClickDoorMarker;
    public GameObject BringDinamitMarker;
    public GameObject RadioSignalMarker;

    public List<MoveAnimation> MoveLivers;

    public List<Liver2D> OtherLivers;
    public Liver2D ScenarioLiver;

    [Space]
    public List<Enemy2D> Eggs;
    public Enemy2D Queen;

    [Space]
    public PlayerRay2D playerRay;

    public Door2D ClickDoor;
    public Door2D NeedUnblockDoor;

    public UnityEvent OnComplete;

    private Equipment dinamit;

    private State state;

    private void Start()
    {
        LiverMoveMessage.SetActive(true);


        foreach (var liver in OtherLivers)
        {
            var moveAnimation = liver.GetComponent<MoveAnimation>();
            moveAnimation.enabled = true;
        }

        state = State.WaitLiverSelect;

        //ScenarioLevel1.Instance.ScenarionLiver.mover.RestoreMove();

        //ShowNextMessage();
    }


    private void FixedUpdate()
    {
        CheckGameOver();

        // выделили чувака
        if (state == State.WaitLiverSelect && playerRay.selectedAgent == ScenarioLiver)
        {
            // print("SelectLiver");
            // ScenarioLiver.mover.RestoreMove();
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

        if (state == State.BringDinamit && ScenarioLiver.Equipment != null)
        {
            // print("Almost Bring Dinamit");
            if (BringDinamitTrigger.OverlapPoint(ScenarioLiver.transform.position))
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
            if (BringDinamitTrigger.OverlapPoint(dinamit.transform.position))
            {
                print("PutDinamit");
                PutDinamitMessage.SetActive(false);

                state = State.ExplodeDinamit;
            }
        }


        if (state == State.ExplodeDinamit && dinamit == null)
        {
            print("ExplodeDinamit");
            PutDinamitMessage.SetActive(false);
            CaveEnterMessage.SetActive(true);

            Destroy(CaveBlock);
            //foreach (var liver in OtherLivers)
            //{
            //    liver.mover.RestoreMove();
            //}

            state = State.CaveExplore;
        }
        /*
        if (state == State.CaveEnter)
        {      
            if (CaveEnterTrigger.OverlapPoint(ScenarioLiver.transform.position))
            {
                print("ScenarioLiver in cave enter");
                foreach (var liver in OtherLivers)
                {
                    if (!CaveEnterTrigger.OverlapPoint(liver.transform.position))
                        return;                    
                }
                print("other livers in cave enter");

                CaveEnterMessage.SetActive(false);
                CaveExploreMessage.SetActive(true);

                state = State.CaveExplore;
            }
        }*/

        if (state == State.CaveExplore)
        {
            if (CaveExploreTrigger.OverlapPoint(ScenarioLiver.transform.position))
            {
                print("ScenarioLiver inside cave");
                foreach (var liver in OtherLivers)
                {
                    if (!CaveExploreTrigger.OverlapPoint(liver.transform.position))
                        return;
                }
                print("other livers inside cave");

                CaveExploreMessage.SetActive(false);
                GoShelterMessage.SetActive(true);

                ScenarioLiver.mover.SwitchState(MoverState.Fast);
                foreach (var liver in OtherLivers)
                {
                    liver.mover.SwitchState(MoverState.Fast);
                }

                EnemiesWakeUp();

                state = State.GoShelter;
            }
        }

        if (state == State.GoShelter)
        {
            bool inShelter = false;
            if (ScenarioLiver != null)
            {
                if (GoShelterTrigger.OverlapPoint(ScenarioLiver.transform.position))
                {
                    inShelter = true;
                }
            }

            if (!inShelter)
            {
                foreach (var liver in OtherLivers)
                {
                    if (liver == null)
                        continue;

                    if (GoShelterTrigger.OverlapPoint(liver.transform.position))
                        inShelter = true;
                    break;
                }
            }

            if (inShelter)
            {
                print("In shelter");

                GoShelterMessage.SetActive(false);
                RadioSignalMessage.SetActive(true);

                RadioSignalMarker.SetActive(true);

                state = State.RadioSignal;
            }
        }

        if (state == State.RadioSignal)
        {
            // Physics2D.OverlapCircleAll(RadioSignalTrigger.transform.position, 0.,.)
           // Physics2D.Ove
           // if (RadioSignalTrigger.OverlapPoint())
            if (RadioSignalZone.Visitor != null)
            {
                print("Radio signal");

                RadioSignalMessage.SetActive(false);
                ScenarioGoalMessage.SetActive(true);

                RadioSignalMarker.SetActive(false);



                Level1.enabled = true;

                state = State.Complete;

                Destroy(gameObject);
            }
        }
    }

    private void EnemiesWakeUp()
    {
        foreach (var egg in Eggs)
        {
            var producer = egg.GetComponent<AgentProducer>();
            producer.enabled = true;

            var animator = egg.GetComponent<Animator>();
            animator.enabled = true;
        }
    }

    private void CheckGameOver()
    {
        // выделили чувака
        /*  if (state == State.WaitLiverSelect)
           {
              if(AnyTutorialLiversDied())
                   PauseGame();

               return;
           }*/

        if (state == State.GoShelter)
        {

            var allDied = AllTutorialLiversDied();
            if (allDied)
            {
                //print("Никто не сообщил о происшествии на базу");
                Tutorial.NobodyGoToShelterMessage.SetActive(true);
                Level1.GameUi.PauseGame(true);                              
            }

            //if (ScenarioLiver == null)
            //{
            //    //print
               
            //}           

            //foreach (var liver in OtherLivers)
            //{
            //    if (liver == null)
            //        print("liver safs df ");
            //}

        }
    }

    private bool AnyTutorialLiversDied()
    {
        if (ScenarioLiver == null)
        {
            print("asd");
            return true;
        }

        if (OtherLivers.Any(t => t == null))
            return true;

        return false;
    }

    private bool AllTutorialLiversDied()
    {
        if (ScenarioLiver == null)
        {
            if (OtherLivers.All(t => t == null))
                return true;
        }      

        return false;
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
