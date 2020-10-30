using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class Game2D : MonoBehaviour
{
    [Header("Дистанция в минутах")]
    public int DistanceInMinutes;

    public GameObject AgentsContent;

    public int LiverCount;
    public int EnemiesCount;
    public int MaxEnemiesCount = 100;

    public bool CanAgentProduce => EnemiesCount < MaxEnemiesCount;

    public Liver2D[] Livers;
    public Enemy2D[] Enemies;

    public LayerMask AgentMask;

    public int DestroyTime = 60;
    private int lastDestroyTime;
    private float currentDestroyTime;


    [Space]
    public GameUI GameUi;

    private float maxSpeed = 1;

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

    public static Game2D Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // скорость в секундах
        maxSpeed = 1;
        currentSpeed = 0;

        currentDestroyTime = DestroyTime;

        Livers = AgentsContent.GetComponentsInChildren<Liver2D>();
        Enemies = AgentsContent.GetComponentsInChildren<Enemy2D>();

        LiverCount = Livers.Length;
        EnemiesCount = Enemies.Length;

        maxDistanceInSeconds = DistanceInMinutes * 60;

        GameUi.UpdateAgentCount();
        GameUi.UpdateDistance((int)distanceProgress);
        GameUi.UpdateSpeed(currentSpeed);
        GameUi.SetMaxDistance(maxDistanceInSeconds);
        GameUi.UpdateDestroyTimer(DestroyTime, false);
        lastCapitanPlace = CapitanPlace.Visitor == null;
        GameUi.SetSpaceModules(Engines[0].Hp, Engines[1].Hp, Reactor.Hp, CapitanPlace.Visitor == null);

        Time.timeScale = 1;

        // Assert.IsTrue(LiverCount != 0);
        // Assert.IsTrue(EnemiesCount != 0);


      /*  foreach (var liver in Livers)
        {
            liver.mover?.StopMove();
        }

        StartGame();*/
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

        lastEngineHp1 = Engines[0].Hp;
        lastEngineHp2 = Engines[1].Hp;
        lastReactorHp = Reactor.Hp;
        lastCapitanPlace = capitanPlace;

        CheckGameOver();
    }

    private bool EnemiesDeadMessageShown;
    private void CheckGameOver()
    {
        if (LiverCount == 0)
        {
            GameUi.LiverDeadMessage.SetActive(true);

            GameOver();
            return;
        }

        if (currentDestroyTime <= 0)
        {
            // todo
            print("Бабах!");
            GameUi.DefeatMessage.SetActive(true);

            GameOver();
            return;
        }

        if (EnemiesCount == 0 && !EnemiesDeadMessageShown)
        {
            GameUi.EnemiesDeadMessage.SetActive(true);
            //EnemiesDeadMessageShown = true;
            GameOver();
            return;
        }

        if (distanceProgress >= maxDistanceInSeconds)
        {
            if (EnemiesCount == 0)
            {
                GameUi.VictoryMessage.SetActive(true);

                GameOver();
                return;
            }
            else
            {
                GameUi.DefeatMessage.SetActive(true);

                GameOver();
                return;
            }
        }

       
    }

    private void GameOver()
    {
        Time.timeScale = 0;
    }

    public void MoveAgents(Vector3 point, float radius)
    {
        // var result = new Collider[10];

        var res = Physics2D.OverlapCircleAll(point, radius, AgentMask.value);
        // Physics.OverlapSphereNonAlloc(point, radius, result, AgentMask.value);

        //print(res.Length);

        foreach (var item in res)
        {
            var agent = item.GetComponent<Liver2D>();
            // todo
            if (!agent.isBusy)
                agent.MoveTo(point);
        }


        // fore
    }

    internal void LiverDead()
    {
        LiverCount--;

        GameUi.UpdateAgentCount();

        //CheckGameOver();
    }

    internal void EnemyDead()
    {
        EnemiesCount--;

        GameUi.UpdateAgentCount();

        //CheckGameOver();
    }

    internal void AddEnemy()
    {
        EnemiesCount++;

        GameUi.UpdateAgentCount();
    }
}
