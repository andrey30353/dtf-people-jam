using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopZone : MonoBehaviour
{
    public Liver2D Visitor;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Visitor == null)
        {
            var liver = collision.gameObject.GetComponent<Liver2D>();
            if (liver != null)
            {
                Visitor = liver;
                liver.mover.StopMove();
                liver.isBusy = true;
                liver.ManageObject = this;
            }
        }
    }
}
