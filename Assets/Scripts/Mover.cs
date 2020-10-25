
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    public Rigidbody rb;
    public Collider collider;  

    public Vector2 startSpeedVector;

    public float Speed;
       
    float speedThreshold;

    public bool isStoped;

    private bool managed = false;

    //Vector3 lastVelo

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();

        SetRandomVelocity();       
        //rb.AddForce(new Vector3(3, 0, 3)*100);

        speedThreshold = Speed * 0f;
    }

    private void SetRandomVelocity()
    {
        var randomDirection = Random.onUnitSphere;
        rb.velocity = new Vector3(randomDirection.x, 0, randomDirection.z) * Speed;
    }

    internal void Stop()
    {
        rb.velocity = Vector3.zero;

        enabled = false;

        isStoped = true;

        rb.isKinematic = true;
        collider.enabled = false;
    }

    internal void StopMove()
    {
        rb.velocity = Vector3.zero;

        enabled = false;

        isStoped = true;       
    }

    internal void RestoreMove()
    {
        SetRandomVelocity();

        enabled = true;

        isStoped = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isStoped)
            return;

        if (managed)
        {
            var inputX = Input.GetAxis("Horizontal");
            var inputY = Input.GetAxis("Vertical");

            var velocity = new Vector3(inputX, 0, inputY).normalized * Speed;
            rb.velocity = velocity;
        }
        else
        {
            var rel = 1f;
            var xVal = Mathf.Abs(rb.velocity.x);
            var zVal = Mathf.Abs(rb.velocity.z);
            if (xVal < zVal)
            {
                rel = rb.velocity.x / rb.velocity.z;
            }
            else
            {
                rel = rb.velocity.z / rb.velocity.x;
            }

            if (Mathf.Abs(rel) < 0.1f)
            {
                Vector3 additional;
                if (rb.velocity.x < rb.velocity.z)
                {
                    additional = rb.velocity.x < 0 ? Vector3.left : Vector3.right;
                }
                else
                {
                    additional = rb.velocity.z < 0 ? Vector3.back : Vector3.forward;
                }
                rb.velocity += additional;
            }

            var currentSpeed = rb.velocity.magnitude;
            if (currentSpeed < Speed + speedThreshold)
            {
                rb.velocity *= Speed / currentSpeed;
            }

            if (currentSpeed > Speed + speedThreshold)
            {
                rb.velocity *= Speed / currentSpeed;
            }
        }


        
    }

    internal void Manage(bool manage)
    {
        managed = manage;

        if(!manage)
        {
           rb.velocity = new Vector3(startSpeedVector.x, 0, startSpeedVector.y);
        }
    }
}
