using UnityEngine;
using UnityEngine.Events;

public class Game2D : MonoBehaviour
{
    public GameObject AgentsContent;

    public int LiverCount;
    public int EnemiesCount;
    public int MaxEnemiesCount = 30;

    public int LiverCountStart { get; private set; }

    public bool CanAgentProduce => EnemiesCount < MaxEnemiesCount;

    public Liver2D[] Livers;
    public Enemy2D[] Enemies;

    public UnityEvent LiverDeadEvent;
    public UnityEvent EnemyDeadEvent;
    public UnityEvent AddEnemyEvent;

    public static Game2D Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Livers = AgentsContent.GetComponentsInChildren<Liver2D>();
        Enemies = AgentsContent.GetComponentsInChildren<Enemy2D>();

        LiverCount = Livers.Length;
        EnemiesCount = Enemies.Length;

        LiverCountStart = LiverCount;
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

        LiverDeadEvent?.Invoke();
    }

    internal void EnemyDead()
    {
        EnemiesCount--;

        EnemyDeadEvent?.Invoke();
    }

    internal void AddEnemy()
    {
        EnemiesCount++;

        AddEnemyEvent?.Invoke();
    }
}
