using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Destructible : MonoBehaviour
{
    public Sprite DamagedSprite;
    public Sprite BrokenSprite;
    private Sprite defaultSprite;

    public float StartHp = 5;
    public float Hp = 5;

    public bool Repaired;

    //public UnityEvent OnBrokenEvent;
    //public UnityEvent OnRepairedEvent;

    public bool NeedRepair => Repaired && Hp < StartHp;
    public float RepairSpeedKoef= 0.1f;
    private float RepairSpeed => StartHp * RepairSpeedKoef;

    public bool Broken => Hp <= 0;
    public bool Damaged => Hp <= StartHp / 2;
    public bool HasFullHp => Hp >= StartHp;

    public float CurrentState => (float) Hp / StartHp;

    private Liver2D Worker;

    private SpriteRenderer sr;
    private Animator animator;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        defaultSprite = sr.sprite;
    }

    private void Update()
    {
        if(Worker != null)
        {
            Hp += RepairSpeed * Time.deltaTime;
            if(Hp >= StartHp)
            {
                Hp = StartHp;
                Worker.mover.RestoreMove();
                Worker.isBusy = false;
                Worker = null;

                sr.sprite = defaultSprite;
                if (animator != null)
                    animator.enabled = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var enemy = collision.gameObject.GetComponent<Enemy2D>();
        if (enemy != null)
        {
            Hp--;
            CheckHp();
        }

        if (NeedRepair && Worker == null)
        {
            var agent = collision.gameObject.GetComponent<Liver2D>();
            if (agent != null && agent.HasRepairKit)
            {
                Worker = agent;
                agent.mover.StopMove();
                agent.isBusy = true;
            }
        }

    }

    private void CheckHp()
    {
        if (Broken)
        {
            Hp = 0;
            Break();
            return;
        }

        if (Damaged)
        {
            if (DamagedSprite != null)
                sr.sprite = DamagedSprite;
        }
    }

    private void Break()
    {
        if (animator != null)
            animator.enabled = false;

        if(BrokenSprite != null)
            sr.sprite = BrokenSprite;
    }
}
