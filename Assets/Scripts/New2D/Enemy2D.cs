﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

// todo наследовать общий класс с Liver2D
public class Enemy2D : MonoBehaviour
{
    public int Hp;
    public int Damage;

    public Mover2D mover;

    public bool CanKill;
    public bool CanInfect;
    public bool DeadAfterInfect;
    public bool CanUseHatch;

    public bool AggressionToDamage;

    public float KillTime;
    public float InfectTime;
    public float HatchTime = 1f;

    public Sprite HatchSprite;
    public float HatchSizeKoef;
    public Color HatchColor;

    private Sprite defaultSprite;
    private Vector3 defaultScale;
    private Color defaultColor;

    [UnityEngine.Serialization.FormerlySerializedAs("DeadEffectPrefab")]
    public GameObject TakeDamageEffectPrefab;

    public ParticleSystem TakeDamageEffect;

    public List<Sprite> DeadSprites;
    public GameObject CorpsePrefab;
    public List<GameObject> DeadPrefabs;
      
   // public AudioSource DeadSound;

    //public bool BreakDoor ;

    // занят ли сейчас
    public bool isBusy;

    SpriteRenderer sr;
    AgentProducer producer;
    private CircleCollider2D collider2d;

    public Liver2D LiverInteract;

    private void Start()
    {
        mover = GetComponent<Mover2D>();
        collider2d = GetComponent<CircleCollider2D>();
        producer = GetComponent<AgentProducer>();

        sr = GetComponent<SpriteRenderer>();
        defaultSprite = sr.sprite;
        defaultScale = transform.localScale;
        defaultColor = sr.color;

        // Assert.IsTrue(Live || (Dead && kill) || (Dead && infect));

        // Assert.IsTrue(Live && !kill && !infect);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if (mover.isStoped)
        //    return;

        if (isBusy)
            return;

        if (collision.gameObject.tag != "Agent")
            return;

        var otherAgent = collision.gameObject.GetComponent<Liver2D>();
        if (otherAgent == null)
            return;

        //if (otherAgent.mover.isStoped)
        //    return;

        //if (otherAgent.isBusy)
        //    return;      

        if(CanInfect && CanKill)
        {
            var infect = UnityEngine.Random.value > 0.5f;
            if (infect)
            {
                if (!otherAgent.isInfected)
                {
                    StartCoroutine(InfectCor(otherAgent, 1));
                }
            }
            else
            {
                if (!otherAgent.isInfected)
                {
                    StartCoroutine(KillCor(otherAgent, KillTime));
                }
            }
        }
        else
        {

            if (CanInfect && !otherAgent.isInfected)
            {
                StartCoroutine(InfectCor(otherAgent, 1));
            }
            // не кушаем инфицированных ?
            if (CanKill && !otherAgent.isInfected)
            {
                StartCoroutine(KillCor(otherAgent, KillTime));
            }
            return;
        }  
    }

    public IEnumerator KillCor(Liver2D victim, float time)
    {
        LiverInteract = victim;
        victim.EnemyInteract = this;

        mover.StopMove();
        victim.mover.StopMove();

        isBusy = true;
        victim.isBusy = true;

        // направление 
        mover.RotateTo(victim.transform.position);
       
        yield return new WaitForSeconds(time);

        isBusy = false;

        if (victim != null)
        {
            victim.isBusy = false;
            victim.TakeDamage(Damage);           
        }

        mover.RestoreMove();
    }


    private IEnumerator InfectCor(Liver2D victim, float time)
    {
        // todo
        //victim.mover.Speed = infector.mover.Speed;

        LiverInteract = victim;
        victim.EnemyInteract = this;

        mover.StopMove();
        victim.mover.StopMove();

        isBusy = true;
        victim.isBusy = true;

        // направление 
        mover.RotateTo(victim.transform.position);
        //var dir = victim.transform.position - transform.position;
        //float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90;
        //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);      

        yield return new WaitForSeconds(time);

        isBusy = false;

        if (victim != null)
        {
            victim.isBusy = false;

            victim.InfectedBy(this);
        }

        victim.mover.RestoreMove();

        if (DeadAfterInfect)
            this.Dead(false);
        else
            this.mover.RestoreMove();
    }

    internal void Manage(bool manage)
    {
        mover.Manage(manage);
    }

    public void Dead(bool needCorpse = true)
    {
        //gameObject.SetActive(false);

        // if (needCorpse)
        //     Instantiate(DeadSpritePrefab, transform.position, Quaternion.Euler(90, 0, 0));

        if (needCorpse)
        {
            /*sr.sprite = DeadSprites[UnityEngine.Random.Range(0, DeadSprites.Count)];
            sr.sortingOrder = 0;*/
            var randomDeadPrefab = DeadPrefabs[UnityEngine.Random.Range(0, DeadPrefabs.Count)];
            var blood = Instantiate(randomDeadPrefab, transform.position, Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 360)));
            blood.transform.localScale = transform.localScale;

            if (CorpsePrefab != null)
            {
                var corpse = Instantiate(CorpsePrefab, transform.position, Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 360)));
                corpse.transform.localScale = transform.localScale;
                corpse.GetComponent<SpriteRenderer>().color = sr.color;
            }
        }

        //if (needCorpse)
        //{
        //    var effect = Instantiate(TakeDamageEffectPrefab, transform.position, Quaternion.Euler(0, 0, 0));
        //    Destroy(effect, 1f);
        //}

        Game2D.Instance.EnemyDead();

        if (needCorpse)
            Settings.Instance.EnemyDeadSound.Play();
        /*
                Destroy(collider2d); 
                if (mover != null)
                {
                    Destroy(mover.animator);
                    Destroy(mover.rb);
                    //Destroy(mover.collider2d);
                    Destroy(mover);
                }

                if (producer != null)
                {
                    Destroy(producer);
                }

                Destroy(this);*/

        if (LiverInteract != null && LiverInteract.EnemyInteract == this)
        {
            LiverInteract.EnemyInteract = null;
            LiverInteract.isBusy = false;
            LiverInteract.mover.RestoreMove();
        }

        Destroy(gameObject);
    }

    internal void UseHatch(Hatch enter)
    {
        StartCoroutine(UseHatchCor(enter));
    }


    internal IEnumerator UseHatchCor(Hatch enter)
    {
        HatchList.Instance.Use(true);

        isBusy = true;
        mover.UseHatch(true);

        sr.sprite = HatchSprite;
        transform.localScale = Vector3.one * HatchSizeKoef;
        
        sr.color = HatchColor;

        var targetHatch = HatchList.Instance.GetRandomHatch(enter);
        var targetPos = targetHatch.transform.position;
        var startPos = enter.transform.position;
        var dir = startPos - targetPos;
        var time = dir.magnitude / (mover.MaxSpeed * HatchList.Instance.SpeedKoefInHatch);

        // для направления значка в решетке
        mover.rb.velocity = dir;        

        var elapsedTime = 0f;
        while (elapsedTime < time)
        {
            transform.position = Vector3.Lerp(startPos, targetPos, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        sr.sprite = defaultSprite;
        transform.localScale = defaultScale;
        sr.color = defaultColor;

        targetHatch.Break();

        isBusy = false;
        mover.UseHatch(false);

        HatchList.Instance.Use(false);
    }

    public void TakeDamage(int value, Vector2 agresstionPoint)
    {
        //TakeDamageEffect.Play();
        var effect = Instantiate(TakeDamageEffectPrefab, transform.position, Quaternion.Euler(0, 0, 0));
        Destroy(effect, 0.1f);

        Hp -= value;

        if (Hp <= 0)
        {
            Dead();
        }

        if (AggressionToDamage)
        {
            mover.rb.velocity = -agresstionPoint.normalized * mover.MaxSpeed;
        }
    }
}
