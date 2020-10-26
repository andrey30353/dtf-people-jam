using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VacuumForce : MonoBehaviour
{
    //BoxCollider2D collider;

    public Transform AttractPoint;

    public float Force = 7;

    private void OnTriggerStay2D(Collider2D collision)
    {
        print(collision.gameObject.name);

        var rb = collision.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            var mover = collision.GetComponent<Mover2D>();
            if (mover != null)
            {
                mover.enabled = false;
            }

            var force = (AttractPoint.position - collision.transform.position).normalized * Force;
            //print(direction);
            //rb.velocity = direction * Force;
            rb.AddForce(force);

        }


    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(AttractPoint.position, 1f);
    }
}
