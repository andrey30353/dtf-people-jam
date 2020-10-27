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

    void Start()
    {
        AgenContent = Settings.Instance.AgentsContent;
        timeoutProcess = Timeout;

        //StartCoroutine(ProduceCor());
    }

    
    void Update()
    {
        timeoutProcess -= Time.deltaTime;
        if (timeoutProcess <= 0)
            Produce();
    }

    private IEnumerator ProduceCor()
    {
        while (true)
        {
            yield return new WaitForSeconds(Timeout);

            var randomAgent = AgenPrefab[Random.Range(0, AgenPrefab.Length)];
            var newAgent = Instantiate(randomAgent, AgenContent);
            newAgent.transform.position = transform.position;
            newAgent.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));

            if (Once)
                break;
        }

        if (Once)
        {
            var agentCarrier = GetComponent<Liver2D>();
            if (agentCarrier != null)
            {
                agentCarrier.Dead();
            }
            else
            {
                Destroy(gameObject);
            }
        }
        //Ma
    }

    private void Produce()
    {
       
            var randomAgent = AgenPrefab[Random.Range(0, AgenPrefab.Length)];
            var newAgent = Instantiate(randomAgent, AgenContent);
            newAgent.transform.position = transform.position;
            newAgent.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));

            timeoutProcess = Timeout;
          

        if (Once)
        {
            var agentCarrier = GetComponent<Liver2D>();
            if (agentCarrier != null)
            {
                agentCarrier.Dead();
            }
            else
            {
                Destroy(gameObject);
            }
        }
     
    }
}
