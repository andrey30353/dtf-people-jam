using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Agent : MonoBehaviour
{
    public bool Live;

    public bool Dead => !Live;

    public Color LiveColor;
    public Color DeadColor;

    MeshRenderer mr;
    public Mover mover;

    public bool kill;
    public bool infect;

    [ContextMenu("Start")]
    private void Start()
    {
        mover = GetComponent<Mover>();      

        mr = GetComponent<MeshRenderer>();

        mr.material.color = Live ? LiveColor : DeadColor;

       // Assert.IsTrue(Live || (Dead && kill) || (Dead && infect));

       // Assert.IsTrue(Live && !kill && !infect);
    }

 
    private void OnCollisionEnter(Collision collision)
    {
        if (mover.isStoped)
            return;       

        if (collision.gameObject.tag != "Agent")
            return;

        var otherAgent = collision.gameObject.GetComponent<Agent>();
        if (otherAgent == null)
            return;

        if (otherAgent.mover.isStoped)
            return;

        // Debug.Log(collision.gameObject.name);

        if(Live && otherAgent.Dead)
        {
            if(otherAgent.infect)
                Infect(otherAgent);

            if(otherAgent.kill)
                Kill();
        }

        if(Dead && otherAgent.Live)
        {
            if (infect)
                otherAgent.Infect(this);

            if (kill)
                otherAgent.Kill();
        }              
    }

    private void Infect(Agent infector)
    {
        if (Dead)
            return;

        Live = false;

        mr.material.color = DeadColor;

       // print($"deadSpeed / mover.startSpeed => {deadSpeed}/{mover.startSpeed} = {deadSpeed / mover.startSpeed}")
        mover.startSpeed = infector.mover.startSpeed;
        kill = infector.kill;
        infect = infector.infect;
    }

    private void Kill()
    {
        if (Dead)
            return;

        gameObject.SetActive(false);
    }

    public void MoveTo(Vector3 point)
    {
        mover.rb.velocity = (point - transform.position).normalized * mover.startSpeed;
    }
}
