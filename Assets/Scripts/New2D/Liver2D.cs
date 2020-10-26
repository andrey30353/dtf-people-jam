using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


public class Liver2D : MonoBehaviour
{       
    // todo del
    public Color Color;  
    
    public Mover2D mover;
   
    public bool CanKill;  
   
    public float KillTime;
    public float InfectTime;

    public GameObject DeadSpritePrefab;
    public GameObject DeadEffectPrefab;

    public List<Sprite> DeadSprites;
   // public GameObject DeadEffectPrefab;

    //public bool BreakDoor ;

    // занят ли сейчас
    public bool isBusy;

    public bool isInfected;

    SpriteRenderer sr;    
   
    private void Start()
    {
        mover = GetComponent<Mover2D>();

        sr = GetComponent<SpriteRenderer>();

       // sr.material.color = Color ;

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
        /*
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
        }    */          
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

   
    private IEnumerator InfectCor(Enemy2D infector, Liver2D victim, float time)
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

    public void Infect(Enemy2D infector)
    {     

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


    public void Dead( bool needCorpse = true)
    {     
       // gameObject.SetActive(false);

        if (needCorpse)
        {
            sr.sprite = DeadSprites[Random.Range(0, DeadSprites.Count)];
            //Instantiate(DeadSpritePrefab, transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360)));
        }

        if (needCorpse)
        {
            var effect = Instantiate(DeadEffectPrefab, transform.position, Quaternion.Euler(0, 0, 0));
            Destroy(effect, 1f);
        }

        Game2D.Instance.LiverDead();

        Destroy(mover.rb);
        Destroy(mover.collider2d);      
        Destroy(mover);
        Destroy(this);
        // Destroy(gameObject);
    }

    public void MoveTo(Vector3 point)
    {
        if(mover!=null)
            mover.rb.velocity = (point - transform.position).normalized * mover.Speed;
    }

    
}
