using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;



public class Game2D : MonoBehaviour
{
    public GameObject AgentsContent;

    public int AgentCount;
  
    public int LiverCount;
    public int EnemiesCount;

    public Liver2D[] Livers;
    public Enemy2D[] Enemies;

    public LayerMask AgentMask;

    [Space]
    public GameUI GameUi;
  

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
        
        GameUi.UpdateUI();

       // Assert.IsTrue(LiverCount != 0);
       // Assert.IsTrue(EnemiesCount != 0);
    }
   

    private void CheckGameOver()
    {
        if(LiverCount == 0)
        {
            print("Поражение!");
            return;
        }

        if (EnemiesCount == 0)
        {
            print("Победа!");
            return;
        }
    }

    public void MoveAgents(Vector3 point, float radius)
    {
       // var result = new Collider[10];

        var res =  Physics2D.OverlapCircleAll(point, radius, AgentMask.value);
        // Physics.OverlapSphereNonAlloc(point, radius, result, AgentMask.value);

        //print(res.Length);

        foreach (var item in res)
        {
           var agent =  item.GetComponent<Liver2D>();
            agent.MoveTo(point);
        }

       
       // fore
    }

    internal void LiverDead()
    {
        LiverCount--;

        GameUi.UpdateUI();

        CheckGameOver();
    }

    internal void EnemyDead()
    {
        EnemiesCount--;

        GameUi.UpdateUI();

        CheckGameOver();
    }
}
