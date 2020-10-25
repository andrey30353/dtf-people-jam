using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentProducer : MonoBehaviour
{
    public Transform AgenContent;

    public GameObject[] AgenPrefab;
    public float Time;

    // один раз?
    public bool OnTime;

    void Start()
    {
        if (AgenContent == null)
            AgenContent = Settings.Instance.AgentsContent;

       // if (queen)
        StartCoroutine(ProduceCor());
    }

    //// Update is called once per frame
    //void Update()
    //{
        
    //}

    private IEnumerator ProduceCor()
    {
        while (true)
        {
            yield return new WaitForSeconds(Time);

            var randomAgent = AgenPrefab[Random.Range(0, AgenPrefab.Length)];
            var newAgent = Instantiate(randomAgent, AgenContent);
            newAgent.transform.position = transform.position;

            if (OnTime)
                break;
        }

        if (OnTime)
        {
            var agentCarrier = GetComponent<Agent>();
            if (agentCarrier != null)
            {
                agentCarrier.Kill();
            }
            else
            {
                Destroy(gameObject);
            }           
        }
        //Ma
    }
}
