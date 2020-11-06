
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoverState
{
    Fast,
    Slow,
    Stop
}

public class Mover2D : MonoBehaviour
{
    public Rigidbody2D rb;
    public CircleCollider2D collider2d;

    // public float startSpeed;
              
    public float MaxSpeed;
    // todo

    private float currentSpeed;
    public float CurrentSpeed => currentSpeed;
              
    [SerializeField] private MoverState state;

    public Animator animator;
          
    private bool managed = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collider2d = GetComponent<CircleCollider2D>();

        animator = GetComponent<Animator>();

        SetRandomVelocity();
        
        SwitchState(state);       
    }

    private void SetRandomVelocity()
    {
        var randomDirection = UnityEngine.Random.insideUnitCircle.normalized;
        //print(randomDirection);
       
        rb.velocity = new Vector2(randomDirection.x, randomDirection.y) * currentSpeed;
    }   

    internal void StopMove()
    {
        SwitchState(MoverState.Stop);
       
        //rb.velocity = Vector2.zero;
    }

    internal void RestoreMove()
    {
        SwitchState(MoverState.Fast);

        if (!managed)
            SetRandomVelocity();       
    }

    // Update is called once per frame
    void Update()
    {     
        if (managed)
        {
            var inputX = Input.GetAxis("Horizontal");
            var inputY = Input.GetAxis("Vertical");

            var velocity = new Vector2(inputX, inputY).normalized * MaxSpeed;
            rb.velocity = velocity;

            if (rb.velocity == Vector2.zero)
            {
                animator.enabled = false;
            }
            else
            {
                animator.enabled = true;
            }
        }
        else
        {
            if (state == MoverState.Stop)
            {
                rb.velocity = Vector2.zero;
                return;
            }
            else
            {
                var xVal = Mathf.Abs(rb.velocity.x);
                var yVal = Mathf.Abs(rb.velocity.y);               

                var speed = rb.velocity.magnitude;
                if (speed == 0)
                {
                    SetRandomVelocity();
                }
                else
                {
                    if (speed < currentSpeed)
                    {
                        rb.velocity *= currentSpeed / speed;
                    }

                    if (speed > currentSpeed)
                    {
                        rb.velocity *= currentSpeed / speed;
                    }
                }

                if (xVal < 0.05f || yVal < 0.05f)
                {
                    SetRandomVelocity();
                }
            }
        }

        if (rb.velocity != Vector2.zero)
        {
            var dir = rb.velocity;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    internal void Manage(bool manage)
    {
        managed = manage;

        if (manage)
            SetAnimationByState(MoverState.Fast);
        else
            SetAnimationByState(state);

        if (!manage)
        {
            SetRandomVelocity();
        }
    }

    internal void UseHatch(bool use)
    {
        collider2d.enabled = !use;
        rb.simulated = !use;
        animator.enabled = !use;
    }

    public void RotateTo(Vector3 point)
    {
        var dir = point - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public void MoveTo(Vector3 point)
    {
        rb.velocity = (point - transform.position).normalized * MaxSpeed;
    }  
    
    public void SwitchState(MoverState state)
    {
        this.state = state;

        switch (state)
        {
            case MoverState.Fast:               
                currentSpeed = MaxSpeed;
                break;
            case MoverState.Slow:              
                currentSpeed = MaxSpeed * 0.5f;
                break;
            case MoverState.Stop:
                currentSpeed = 0;               
                break;
            default:
                break;
        }

        SetAnimationByState(state);
    }

    public void SetAnimationByState(MoverState state)
    {       
        switch (state)
        {
            case MoverState.Fast:
                animator.speed = 1f;              
                break;
            case MoverState.Slow:
                animator.speed = 0.5f;               
                break;
            case MoverState.Stop:
                currentSpeed = 0;
                break;
            default:
                break;
        }

        animator.enabled = state != MoverState.Stop;
    }
}


