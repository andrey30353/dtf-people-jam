using System.Linq;
using UnityEngine;

public enum Level1State
{
    Unknown,

    LiversDiedDefeat,
    SafersIsArrived,  
}

public class Level1Progress : MonoBehaviour
{
    [Header("Время до победы")]
    public int TimeToWin;

    [Space]
    public Destructible Reactor;

    [Space]
    public Level1UI GameUi;
    
    [Space]
    public float timeProgress;
    public float lastTimeProgress;

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
    }

    private void Update()
    {
        timeProgress += Time.deltaTime;

        var timeProgressRel = timeProgress / TimeToWin;

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

        if (timeProgress >= TimeToWin)               
        {
            return Level1State.SafersIsArrived;
        }

        return Level1State.Unknown;
    }      
}
