using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Rigidbody rb;

    public float startSpeed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = new Vector3(1, 0, 1) * 5;
        startSpeed = rb.velocity.magnitude;
        //rb.AddForce(new Vector3(3, 0, 3)*100);
    }

    // Update is called once per frame
    void Update()
    {
        var currentSpeed = rb.velocity.magnitude;
        if (currentSpeed < startSpeed)
        {
            var difSpeed = startSpeed - currentSpeed;
            print(difSpeed);

            rb.velocity *= startSpeed / currentSpeed;
        }
        
    }
}
