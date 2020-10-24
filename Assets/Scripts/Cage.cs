using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cage : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Agent"))
        {
            print(other.gameObject);

            var agent = other.gameObject.GetComponent<Agent>();
            if (agent == null)
                return;

            if (agent.Dead)
            {
                agent.mover.Stop();
                agent.enabled = false;
            }

           
        }
    }

    //void OnTriggerStay(Collider other)
    //{
    //    if (other.CompareTag("Strawberry"))
    //    {
    //        var anim = other.GetComponent<StrawberryAnim>();

    //        if (anim != null)
    //        {
    //            anim.IsEaten = snake.enabled;
    //        }
    //    }
    //}

    //void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Strawberry"))
    //    {
    //        var anim = other.GetComponent<StrawberryAnim>();

    //        if (anim != null)
    //        {
    //            anim.IsEaten = false;
    //        }
    //    }
    //}
}
