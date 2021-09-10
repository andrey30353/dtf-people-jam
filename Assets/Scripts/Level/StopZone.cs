using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopZone : MonoBehaviour
{
    public Liver2D Visitor;
    public BoxCollider2D collider2d;

    public bool IsVisited => Visitor != null;

    public bool NotStop;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Visitor != null)
            return;

        if (!collision.gameObject.TryGetComponent<Liver2D>(out var liver))
            return;

        Visitor = liver;
        if (NotStop)
            return;

        liver.mover.StopMove();
        liver.isBusy = true;
        liver.ManageObject = this;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (Visitor == null || NotStop)
            return;

        if (!collision.gameObject.TryGetComponent<Liver2D>(out var liver))
            return;

        if (Visitor != liver)
            return;

        Visitor.mover.RestoreMove();
        // todo
        Visitor.isBusy = false;
        Visitor.ManageObject = null;

        Visitor = null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(collider2d.bounds.center, collider2d.bounds.size);
    }
}
