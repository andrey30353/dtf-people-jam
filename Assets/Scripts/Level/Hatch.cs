using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hatch : MonoBehaviour
{
    public Sprite BrokenSprite;

    /*
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


    }*/

    public void Broke()
    {

    }

}
