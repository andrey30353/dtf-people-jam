using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentProducer : MonoBehaviour
{
    public Transform AgenContent;

    public GameObject AgenPrefab;
    public float Time;

    void Start()
    {
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

            var newAgent = Instantiate(AgenPrefab, AgenContent);
            newAgent.transform.position = transform.position;
        }       
        //Ma
    }
}
