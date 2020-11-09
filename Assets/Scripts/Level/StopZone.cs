using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopZone : MonoBehaviour
{
    public Liver2D Visitor;
    public BoxCollider2D collider2d;

    public bool NotStop;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Visitor == null)
        {
            var liver = collision.gameObject.GetComponent<Liver2D>();
            if (liver != null)
            {
                Visitor = liver;
                if (NotStop)
                    return;
                
                liver.mover.StopMove();
                liver.isBusy = true;
                liver.ManageObject = this;                          
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;        
        Gizmos.DrawWireCube(collider2d.bounds.center, collider2d.bounds.size);
    }
}
