using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum Level3State
{
    Unknown,

    LiversDiedDefeat,
    ReactorExplosion,
    ReactorExplosionOnEarth,

    EnemiesDeadVictory,
    InfectEarth,

    ReactorWarning
}


public class Level3Progress : MonoBehaviour
{
    [Header("Дистанция в минутах")]
    public int DistanceInMinutes;

    public int DestroyTime = 60;
    private int lastDestroyTime;
    private float currentDestroyTime;

    [Space]
    public Level3UI GameUi;       

    [Header("Системы корабля")]
    public StopZone CapitanPlace;
    public List<Destructible> Engines;
    public Destructible Reactor;

    [Space]
    public float currentSpeed;
    public float lastCurrentSpeed;

    public float maxDistanceInSeconds;
    private float distanceProgress;
    private int lastDistanceProgress;

    private float lastEngineHp1;
    private float lastEngineHp2;
    private float lastReactorHp;
    private bool lastCapitanPlace;

    public Level3State lastState;
    public Level3State state;   

    private void Start()
    {
        // скорость в секундах       
        currentSpeed = 0;

        currentDestroyTime = DestroyTime;
      
        maxDistanceInSeconds = DistanceInMinutes * 60;

        GameUi.UpdateAgentCount();
        GameUi.UpdateDistance((int)distanceProgress);
        GameUi.UpdateSpeed(currentSpeed);
        GameUi.SetMaxDistance(maxDistanceInSeconds);
        GameUi.UpdateDestroyTimer(DestroyTime, false);
        lastCapitanPlace = CapitanPlace.Visitor == null;
        GameUi.SetSpaceModules(Engines[0].Hp, Engines[1].Hp, Reactor.Hp, CapitanPlace.Visitor == null);        
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
        //if (!started)
        //    return;

        if (CapitanPlace.Visitor == null)
            currentSpeed = 0;
        else
            currentSpeed = Engines.Sum(t => t.CurrentState) / Engines.Count;

        distanceProgress += Time.deltaTime * currentSpeed;
        if ((int)distanceProgress != lastDistanceProgress)
            GameUi.UpdateDistance((int)distanceProgress);

        if (currentSpeed != lastCurrentSpeed)
            GameUi.UpdateSpeed(currentSpeed);

        // самоуничтожение
        if (Reactor.Broken)
        {
            currentDestroyTime -= Time.deltaTime;
        }
        if (Reactor.HasFullHp)
        {
            currentDestroyTime = DestroyTime;
        }

        var roundedTime = Mathf.RoundToInt(currentDestroyTime);
        if (roundedTime != lastDestroyTime)
        {
            GameUi.UpdateDestroyTimer(roundedTime, !Reactor.HasFullHp);
        }

        // модули корабля
        if (Engines[0].Hp != lastEngineHp1)
            GameUi.UpdateEngine1(Engines[0].Hp);

        if (Engines[1].Hp != lastEngineHp2)
            GameUi.UpdateEngine2(Engines[1].Hp);

        if (Reactor.Hp != lastReactorHp)
            GameUi.UpdateReactor(Reactor.Hp);

        var capitanPlace = CapitanPlace.Visitor == null;
        if (capitanPlace != lastCapitanPlace)
        {
            GameUi.UpdateCapitan(capitanPlace);
            GameUi.UpdateSpeed(currentSpeed);
        };

        lastDistanceProgress = (int)distanceProgress;
        lastDestroyTime = roundedTime;

        if (Engines.Count > 1)
        {
            lastEngineHp1 = Engines[0].Hp;
            lastEngineHp2 = Engines[1].Hp;
        }
        lastReactorHp = Reactor.Hp;
        lastCapitanPlace = capitanPlace;

        CheckGameOver();
    }

    private void CheckGameOver()
    {
        state = DefineStatus();

        if (state != lastState)
            GameUi.UpdateMessage(state);

        lastState = state;
    }

    private Level3State DefineStatus()
    {
        // поражение
        if (Game2D.Instance.LiverCount == 0)
        {
            return Level3State.LiversDiedDefeat;
        }

        if (currentDestroyTime <= 0)
        {
            return Level3State.ReactorExplosion;
        }

        if (Game2D.Instance.EnemiesCount == 0)
        {
            // нужно починить реактор хотя бы до половины
            if (Reactor.Hp < Reactor.StartHp / 2)
            {
                return Level3State.ReactorWarning;
            }
            else
            {
                return Level3State.EnemiesDeadVictory;
            }
        }

        // долетели
        if (distanceProgress >= maxDistanceInSeconds)
        {
            if (Reactor.Hp <= 0)
            {
                return Level3State.ReactorExplosionOnEarth;
            }

            if (Game2D.Instance.EnemiesCount == 0)
            {
                return Level3State.EnemiesDeadVictory;
            }
            else
            {
                return Level3State.InfectEarth;
            }
        }

        return Level3State.Unknown;
    }
}
