using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


public class Enemy2D : MonoBehaviour
{
    public int Hp;
    public int Damage;

    public Mover2D mover;

    public bool CanKill;
    public bool CanInfect;
    public bool CanUseHatch;

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

    //public bool BreakDoor ;

    // занят ли сейчас
    public bool isBusy;

    SpriteRenderer sr;

    private void Start()
    {
        mover = GetComponent<Mover2D>();

        sr = GetComponent<SpriteRenderer>();
        defaultSprite = sr.sprite;
        defaultScale = transform.localScale;
        defaultColor = sr.color;

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

        var otherAgent = collision.gameObject.GetComponent<Liver2D>();
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


        if (CanInfect && !otherAgent.isInfected)
        {
            StartCoroutine(InfectCor(this, otherAgent, 1));
        }
        // не кушаем инфицированных
        if (CanKill && !otherAgent.isInfected)
        {
            StartCoroutine(KillCor(this, otherAgent, KillTime));
        }
        return;

    }

    public IEnumerator KillCor(Enemy2D killer, Liver2D victim, float time)
    {
        killer.mover.StopMove();
        victim.mover.StopMove();

        killer.isBusy = true;
        victim.isBusy = true;

        yield return new WaitForSeconds(time);

        killer.isBusy = false;
        victim.isBusy = false;

        victim.TakeDamage(Damage);

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

    public void Dead(bool needCorpse = true)
    {
        //gameObject.SetActive(false);

       // if (needCorpse)
       //     Instantiate(DeadSpritePrefab, transform.position, Quaternion.Euler(90, 0, 0));

        if (needCorpse)
        {
            sr.sprite = DeadSprites[UnityEngine.Random.Range(0, DeadSprites.Count)];
            sr.sortingOrder = 0;
            //Instantiate(DeadSpritePrefab, transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360)));
        }

        //if (needCorpse)
        //{
        //    var effect = Instantiate(TakeDamageEffectPrefab, transform.position, Quaternion.Euler(0, 0, 0));
        //    Destroy(effect, 1f);
        //}

        Game2D.Instance.EnemyDead();

        if(mover != null)
        {
            Destroy(mover.animator);
            Destroy(mover.rb);
            Destroy(mover.collider2d);
            Destroy(mover);
        }
       
        Destroy(this);

        //Destroy(gameObject);
    }
    /*
    public void DeadCor(float timer = 0)
    {        
        gameObject.SetActive(false);

        Instantiate(DeadSpritePrefab, transform.position, Quaternion.Euler(90, 0, 0));

        Game2D.Instance.EnemyDead();

        Destroy(gameObject, timer);
    }*/

  

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
        var time = (startPos - targetPos).magnitude / (mover.Speed * HatchList.Instance.SpeedKoefInHatch);

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

    public void TakeDamage(int value)
    {
        //TakeDamageEffect.Play();
        var effect = Instantiate(TakeDamageEffectPrefab, transform.position, Quaternion.Euler(0, 0, 0));
        Destroy(effect, 0.1f);

        Hp -= value;

        if (Hp <= 0)
        {
            Dead();
        }
    }
}
