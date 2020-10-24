using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    public Rigidbody rb;
    public Collider collider;  

    public Vector2 startSpeedVector;

    public float startSpeed;

    float speedThreshold;

    public bool isStoped;

    private bool managed = false;

    //Vector3 lastVelo

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();

        rb.velocity = new Vector3(startSpeedVector.x, 0, startSpeedVector.y);
        startSpeed = rb.velocity.magnitude;
        //rb.AddForce(new Vector3(3, 0, 3)*100);

        speedThreshold = startSpeed * 0f;
    }

    internal void Stop()
    {
        rb.velocity = Vector3.zero;

        enabled = false;

        isStoped = true;

        rb.isKinematic = true;
        collider.enabled = false;
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

            var velocity = new Vector3(inputX, 0, inputY).normalized * startSpeed;
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
            if (currentSpeed < startSpeed + speedThreshold)
            {
                rb.velocity *= startSpeed / currentSpeed;
            }

            if (currentSpeed > startSpeed + speedThreshold)
            {
                rb.velocity *= startSpeed / currentSpeed;
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
