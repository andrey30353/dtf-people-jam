using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentProducer : MonoBehaviour
{
    public Transform AgenContent;

    public GameObject[] AgenPrefab;
    [UnityEngine.Serialization.FormerlySerializedAs("Time")]
    public float Timeout;
    private float timeoutProcess;

    // один раз?   
    public bool Once;

    //public bool CheckCount;

    void Start()
    {
        AgenContent = Settings.Instance.AgentsContent;
        timeoutProcess = Timeout;

        //StartCoroutine(ProduceCor());
    }


    void Update()
    {
        timeoutProcess -= Time.deltaTime;
        // todo
        if (timeoutProcess <= 0 && Game2D.Instance.CanAgentProduce)        
            Produce();
                  
    }

    private void Produce()
    {        
        var randomAgent = AgenPrefab[Random.Range(0, AgenPrefab.Length)];       
        var newAgent = Instantiate(randomAgent, AgenContent);
        newAgent.transform.position = transform.position;
        newAgent.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));

        timeoutProcess = Timeout;

        Game2D.Instance.AddEnemy();

        if (Once)
        {           
            var agentCarrier = GetComponent<Liver2D>();
            if (agentCarrier != null)
            {                
                agentCarrier.Dead();
            }
            else
            {
                Game2D.Instance.EnemyDead();
                Destroy(gameObject);
            }
        }

    }
}
