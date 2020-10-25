﻿
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover2D : MonoBehaviour
{
    public Rigidbody2D rb;
    public CircleCollider2D collider2d;  

   // public float startSpeed;

    public float Speed;
       
    float speedThreshold;

    public bool isStoped;

    private bool managed = false;   

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collider2d = GetComponent<CircleCollider2D>();

        SetRandomVelocity();      

        speedThreshold = Speed * 0f;
    }

    private void SetRandomVelocity()
    {
        var randomDirection = Random.insideUnitCircle.normalized;
        //print(randomDirection);
        rb.velocity = new Vector2(randomDirection.x, randomDirection.y) * Speed;
    }

    internal void Stop()
    {
        rb.velocity = Vector2.zero;

        enabled = false;

        isStoped = true;

        rb.isKinematic = true;
        collider2d.enabled = false;
    }

    internal void StopMove()
    {
        rb.velocity = Vector2.zero;

       // enabled = false;

        isStoped = true;       
    }

    internal void RestoreMove()
    {
        SetRandomVelocity();

      //  enabled = true;

        isStoped = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isStoped)
        {
            rb.velocity = Vector2.zero;
            return;
        }           

        if (managed)
        {
            var inputX = Input.GetAxis("Horizontal");
            var inputY = Input.GetAxis("Vertical");

            var velocity = new Vector2(inputX, inputY).normalized * Speed;
            rb.velocity = velocity;
        }
        else
        {           
           
            var rel = 1f;
            var xVal = Mathf.Abs(rb.velocity.x);
            var yVal = Mathf.Abs(rb.velocity.y);
            /*if (xVal < yVal)
            {
                rel = rb.velocity.x / rb.velocity.y;
            }
            else
            {
                rel = rb.velocity.y / rb.velocity.x;
            }
            
            if (Mathf.Abs(rel) < 0.1f)
            {
                Vector2 additional;
                if (rb.velocity.x < rb.velocity.y)
                {
                    additional = rb.velocity.x < 0 ? Vector2.left : Vector2.right;
                }
                else
                {
                    additional = rb.velocity.y < 0 ? Vector2.down : Vector2.up;
                }
                rb.velocity += additional*0.5f;
            }
            */
                        
            var currentSpeed = rb.velocity.magnitude;
            if(currentSpeed == 0)
            {
                SetRandomVelocity();
            }
            else
            {
                if (currentSpeed < Speed + speedThreshold)
                {
                    rb.velocity *= Speed / currentSpeed;
                }

                if (currentSpeed > Speed + speedThreshold)
                {
                    rb.velocity *= Speed / currentSpeed;
                }
            }

            if (xVal < 0.05f || yVal < 0.05f)
            {
                SetRandomVelocity();
            }
        }
    }

    internal void Manage(bool manage)
    {
        managed = manage;

        if(!manage)
        {
            SetRandomVelocity();
        }
    }
}
