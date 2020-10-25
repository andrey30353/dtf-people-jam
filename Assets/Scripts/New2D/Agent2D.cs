using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


public class Agent2D : MonoBehaviour
{
    public bool Live;

    public bool Dead => !Live;
      
    public Color Color;  

    
    public Mover mover;
   
    public bool CanKill;
  
    public bool CanInfect;

    public float KillTime;
    public float InfectTime;

    public GameObject killSpritePrefab;

    //public bool BreakDoor ;

        // занят ли сейчас
    public bool isBusy;

    public bool isInfected;

    SpriteRenderer sr;
   
    private void Start()
    {
        mover = GetComponent<Mover>();

        sr = GetComponent<SpriteRenderer>();

        sr.material.color = Color ;

        // Assert.IsTrue(Live || (Dead && kill) || (Dead && infect));

        // Assert.IsTrue(Live && !kill && !infect);

       
    }    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (mover.isStoped)
            return;

        if (isBusy)
            return;

        if (collision.gameObject.tag != "Agent")
            return;

        var otherAgent = collision.gameObject.GetComponent<Agent2D>();
        if (otherAgent == null)
            return;

        if (otherAgent.mover.isStoped)
            return;

        if (otherAgent.isBusy)
            return;

        // Debug.Log(collision.gameObject.name);

      /*  if (Live && otherAgent.Dead)
        {
            if (otherAgent.CanInfect)
            {
                StartCoroutine(InfectCor(otherAgent, this, 1));                
            }

            if (otherAgent.CanKill)
            {
                StartCoroutine(KillCor(otherAgent, this, KillTime));               
            }

            return;
        }*/

        if(Dead && otherAgent.Live)
        {
            if (CanInfect && !otherAgent.isInfected)
            {
                StartCoroutine(InfectCor(this, otherAgent, 1));               
            }
            // не кушаем инфицированных
            if (CanKill &&  !otherAgent.isInfected)
            {
                StartCoroutine(KillCor(this, otherAgent, KillTime));               
            }
            return;
        }              
    }

    public IEnumerator KillCor(Agent2D killer, Agent2D victim, float time)
    {
        killer.mover.StopMove();
        victim.mover.StopMove();

        killer.isBusy = true;
        victim.isBusy = true;

        yield return new WaitForSeconds(time);

        killer.isBusy = false;
        victim.isBusy = false;

        victim.Kill();

        killer.mover.RestoreMove();       
    }

   
    private IEnumerator InfectCor(Agent2D infector, Agent2D victim, float time)
    {
        // todo
        //victim.mover.Speed = infector.mover.Speed;

        infector.mover.StopMove();
        victim.mover.StopMove();

        infector.isBusy = true;
        victim.isBusy = true;

        yield return new WaitForSeconds(time);

        infector.isBusy = false;
        victim.isBusy = false;

        victim.Infect(infector);

        victim.mover.RestoreMove();
        infector.mover.RestoreMove();        
    }

    internal void Manage(bool manage)
    {
        mover.Manage(manage);
    }

    private void Infect(Agent2D infector)
    {
        if (Dead)
            return;

        //Live = false;

        isInfected = true;

        sr.material.color = Settings.Instance.InfectedColor;//infector.Color;
                       
        // todo
        var producer = gameObject.AddComponent<AgentProducer>();
        var templ = infector.GetComponent<AgentProducer>();
        producer.AgenContent = templ.AgenContent;
        producer.AgenPrefab = templ.AgenPrefab;
        producer.Time = templ.Time;
        producer.Once = templ.Once;
        producer.enabled = true;

        // print($"deadSpeed / mover.startSpeed => {deadSpeed}/{mover.startSpeed} = {deadSpeed / mover.startSpeed}")
        // todo
        //mover.Speed = infector.mover.Speed;
        //CanKill = infector.CanKill;
        //CanInfect = infector.CanInfect;

        // это не нужно: оставляем живым
        /*KillTime = infector.KillTime;
        InfectTime = infector.InfectTime;

        gameObject.layer = infector.gameObject.layer;
        gameObject.tag = infector.gameObject.tag;*/
    }


    public void Kill()
    {
        if (Dead)
            return;

        gameObject.SetActive(false);

        Instantiate(killSpritePrefab, transform.position, Quaternion.Euler(90, 0 ,0));

        Game2D.Instance.LiverDead();
       

        Destroy(gameObject);
    }

    public void MoveTo(Vector3 point)
    {
        if(mover!=null)
            mover.rb.velocity = (point - transform.position).normalized * mover.Speed;
    }

    
}
