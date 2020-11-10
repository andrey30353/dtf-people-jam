using System.Linq;
using UnityEngine;

public enum Level1State
{   
    LiversDiedDefeat,

    WaitSafers,
       
    NeedGoShip,
     
    NeedGoShipTimeIsOver,

    SafeLiverOnShip
}

public class Level1Progress : MonoBehaviour
{
    [Header("Время ожидания спасателей")]
    public int WaitSafersTime;

    [Header("Время эвакуации")]
    public int NeedGoShipTime;

    [Space]
    public GameObject Ship;
    public StopZone ShipPlace;

    [Space]
    public Level1UI GameUi;
    
    [Space]
    public float timeProgress;
    public float lastTimeProgress;

    public float evacuationTimeProgress;
    public float lastEvacuationTimeProgress;

    public Level1State lastState;
    public Level1State state;  

    public void Start()
    {
        timeProgress = 0;        

        GameUi.ShowTopPanel();

        GameUi.UpdateAgentCount();
        //GameUi.SetMaxTime(TimeToWin);
        GameUi.UpdateTime(0); 
    }

    public bool started = false;
    public void StartGame()
    {
        started = true;

        foreach (var liver in Game2D.Instance.Livers)
        {
            liver.mover.RestoreMove();
        }

        state = Level1State.WaitSafers;
    }

    private void Update()
    {
        timeProgress += Time.deltaTime;

        // todo
        var fullTime = timeProgress;
        if(state == Level1State.WaitSafers)
        {
            fullTime = WaitSafersTime;
        }
        if (state == Level1State.NeedGoShip)
        {
            fullTime = NeedGoShipTime;
        }      

        var timeProgressRel = timeProgress / fullTime;

        //if ((int)timeProgress != lastTimeProgress)
            GameUi.UpdateTime(timeProgressRel);

        lastTimeProgress = timeProgress;

        CheckGameOver();
    }

    private void CheckGameOver()
    {      
        state = DefineStatus();

        if (state != lastState)
            GameUi.UpdateMessage(state);

        lastState = state;
    }

    private Level1State DefineStatus()
    {
        // поражение
        if (Game2D.Instance.LiverCount == 0)
        {
            return Level1State.LiversDiedDefeat;
        }

        if (state == Level1State.WaitSafers && timeProgress >= WaitSafersTime)               
        {
            timeProgress = 0;
            Ship.SetActive(true);
            return Level1State.NeedGoShip;
        }

        if (state == Level1State.NeedGoShip )
        {
            if(ShipPlace.Visitor != null)            
                return Level1State.SafeLiverOnShip;
            
            if(timeProgress >= NeedGoShipTime)
                return Level1State.NeedGoShipTimeIsOver;

            return Level1State.NeedGoShip;
        }           

        return Level1State.WaitSafers;
    }      
}
