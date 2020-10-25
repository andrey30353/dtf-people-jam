using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Game : MonoBehaviour
{
    public GameObject AgentsContent;

    public int AgentCount;
  
    public int LiveCount;
    public int DeadCount;

    public List<Agent> LiveAgents;

    public LayerMask AgentMask;

    public static Game Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        var agents = AgentsContent.GetComponentsInChildren<Agent>();
        Assert.IsTrue(agents.Length != 0);

        foreach (var agent in agents)
        {
            if (agent.Live)
            {
                LiveAgents.Add(agent);
                LiveCount++; 
            }
            else
            { 
                DeadCount++;
            }

        }

        Assert.IsTrue(LiveCount != 0);
        Assert.IsTrue(DeadCount != 0);
    }
   

    private void CheckGameOver()
    {
        if(LiveCount == 0)
        {
            print("Поражение!");
            return;
        }

        if (DeadCount == 0)
        {
            print("Победа!");
            return;
        }
    }

    public void MoveAgents(Vector3 point, float radius)
    {
       // var result = new Collider[10];

        var res =  Physics.OverlapSphere(point, radius, AgentMask.value);
        // Physics.OverlapSphereNonAlloc(point, radius, result, AgentMask.value);

        //print(res.Length);

        foreach (var item in res)
        {
           var agent =  item.GetComponent<Agent>();
            agent.MoveTo(point);
        }

       
       // fore
    }
}
