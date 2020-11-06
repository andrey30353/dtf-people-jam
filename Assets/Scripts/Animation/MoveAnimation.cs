using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAnimation : MonoBehaviour
{  
    public Vector3 TargetPosition;

    public float Speed;

    private float elapsedTime = 0f;
    private float time = 0f;

    private Vector3 startPosition;

    private Mover2D mover;

    private void Start()
    {
        startPosition = transform.position;
        mover = GetComponent<Mover2D>();

        Speed = mover.CurrentSpeed;

        time = (TargetPosition - startPosition).magnitude / Speed;  
    }

    // Update is called once per frame
    private void Update()
    {
        elapsedTime += Time.deltaTime;

        transform.position = Vector3.Lerp(startPosition, TargetPosition, elapsedTime / time);

        mover.RotateTo(TargetPosition);

        if(transform.position == TargetPosition)
        {
            mover.StopMove();
            Destroy(this);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, TargetPosition);
       // Gizmos.color = Color.yellow;
    }
}
