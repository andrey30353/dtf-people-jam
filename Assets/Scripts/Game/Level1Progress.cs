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

    public GameObject AgentsContent;

    public int LiverCount;
    public int EnemiesCount;
    public int MaxEnemiesCount = 30;

    public bool CanAgentProduce => EnemiesCount < MaxEnemiesCount;

    public Liver2D[] Livers;
    public Enemy2D[] Enemies;

    [Space]
    public Destructible Reactor;

    [Space]
    public Level1UI GameUi;
    
    [Space]
    public float timeProgress;
    public float lastTimeProgress;

    public Level1State lastState;
    public Level1State state;

    public static Level1Progress Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        timeProgress = 0;        

        Livers = AgentsContent.GetComponentsInChildren<Liver2D>();
        Enemies = AgentsContent.GetComponentsInChildren<Enemy2D>();

        LiverCount = Livers.Length;
        EnemiesCount = Enemies.Length;

        GameUi.ShowTopPanel();

        GameUi.UpdateAgentCount(LiverCount, EnemiesCount);
        //GameUi.SetMaxTime(TimeToWin);
        GameUi.UpdateTime(0); 
    }

    public bool started = false;
    public void StartGame()
    {
        started = true;

        foreach (var liver in Livers)
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
        if (LiverCount == 0)
        {
            return Level1State.LiversDiedDefeat;
        }

        if (timeProgress >= TimeToWin)               
        {
            return Level1State.SafersIsArrived;
        }

        return Level1State.Unknown;
    }  

    internal void LiverDead()
    {
        LiverCount--;

        /*if(LiverCount == 1)
        {
            var livers = AgentsContent.GetComponentsInChildren<Liver2D>();
            if(livers.Length == 1)
            {
                livers[0].Manage(true);
            }               
        }*/

        GameUi.UpdateAgentCount(LiverCount, EnemiesCount);

        //CheckGameOver();
    }

    internal void EnemyDead()
    {
        EnemiesCount--;

        GameUi.UpdateAgentCount(LiverCount, EnemiesCount);

        //CheckGameOver();
    }

    internal void AddEnemy()
    {
        EnemiesCount++;

        GameUi.UpdateAgentCount(LiverCount, EnemiesCount);
    }
}
