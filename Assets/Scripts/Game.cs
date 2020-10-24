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

    private void Start()
    {
        var agents = AgentsContent.GetComponentsInChildren<Agent>();
        Assert.IsTrue(agents.Length != 0);

        foreach (var agent in agents)
        {
            if (agent.Live)
                LiveCount++;
            else
                DeadCount++;
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
}
