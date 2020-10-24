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

        if(rb.velocity.x < 1 && rb.velocity.x > -1)
        {
            rb.velocity = new Vector3(rb.velocity.x * 2, 0, rb.velocity.z);
        }

        if (rb.velocity.z < 1 && rb.velocity.z > -1)
        {
            rb.velocity = new Vector3(rb.velocity.x , 0, rb.velocity.z * 2);
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
