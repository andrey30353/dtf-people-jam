using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public enum AgentType
{
    Team,

    // враги:
    // просто убивает
    Hunter,

    // заразитель, вылупляются разные с разной вероятностью
    Infector,

    // королева - производит новых
    Queen,

    // из него получаются новые враги
    Egg
}

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
    public bool queen;

    public GameObject killSpritePrefab;

    //public bool BreakDoor ;

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
            if (otherAgent.infect)
            {
                StartCoroutine(InfectCor(otherAgent, this, 0.5f));                
            }

            if (otherAgent.kill)
            {
                StartCoroutine(KillCor(otherAgent, this, 1f));               
            }

        }

        if(Dead && otherAgent.Live)
        {
            if (infect)
            {
                StartCoroutine(InfectCor(this, otherAgent, 0.5f));               
            }

            if (kill)
            {
                StartCoroutine(KillCor(this, otherAgent, 1f));               
            }              
        }              
    }

    public IEnumerator KillCor(Agent killer, Agent victim, float time)
    {
        killer.mover.StopMove();

        victim.mover.StopMove();
        yield return new WaitForSeconds(time);

        victim.Kill();

        killer.mover.RestoreMove();
    }

    internal void Manage(bool manage)
    {
        mover.Manage(manage);
    }

    private void Infect(Agent infector)
    {
        if (Dead)
            return;

        Live = false;

        mr.material.color = DeadColor;

       // print($"deadSpeed / mover.startSpeed => {deadSpeed}/{mover.startSpeed} = {deadSpeed / mover.startSpeed}")
        // todo
        //mover.Speed = infector.mover.Speed;
        kill = infector.kill;
        infect = infector.infect;

        gameObject.layer = infector.gameObject.layer;
        gameObject.tag = infector.gameObject.tag;
    }

    private IEnumerator InfectCor(Agent infector, Agent victim, float time)
    {
        // todo
        victim.mover.Speed = infector.mover.Speed;

        infector.mover.StopMove();

        victim.mover.StopMove();
        yield return new WaitForSeconds(time);

        victim.Infect(infector);

        victim.mover.RestoreMove();
        infector.mover.RestoreMove();        
    }

    public void Kill()
    {
        if (Dead)
            return;

        gameObject.SetActive(false);

        Instantiate(killSpritePrefab, transform.position, Quaternion.Euler(90, 0 ,0));

        Game.Instance.LiveCount--;
    }

    public void MoveTo(Vector3 point)
    {
        mover.rb.velocity = (point - transform.position).normalized * mover.Speed;
    }

    
}
