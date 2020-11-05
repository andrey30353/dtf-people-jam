using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

// todo наследовать общий класс с Enemy2D
public class Liver2D : MonoBehaviour
{
    public int Hp = 1;
    //public int HpWithWeapon = 2;    
    public float AttackRange = 1;
    public float AttackTimeout = 1;
    public LayerMask AttackMask;
    public LayerMask AttackCheckMask;
    public Bullet BulletPrefab;
    private float attackTimeoutProcess;

    [Space]
    public Equipment Equipment;
    public KeyCard Key;
    public float TakeDelay;

    public bool HasWeapon => Equipment != null && Equipment.Type == EquipmentType.Weapon;
    public bool HasRepairKit => Equipment != null && Equipment.Type == EquipmentType.RepairKit;

    [Space]
    public SpriteRenderer EquipmentMark;
    public SpriteRenderer KeyMark;
    public SpriteRenderer SelectionMark;

    [Space]
    public GameObject DeadEffectPrefab;
    public List<Sprite> DeadSprites;
    public GameObject CorpsePrefab;
    public List<GameObject> DeadPrefabs;

    [Space]
    public Mover2D mover;
    // занят ли сейчас
    public bool isBusy;

    public bool isInfected;

    public StopZone ManageObject;
    public Enemy2D EnemyInteract;

    //public AudioSource ShotSound;
    //public AudioSource DeadSound;

    SpriteRenderer sr;

    private void Start()
    {
        mover = GetComponent<Mover2D>();

        sr = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if (isBusy)
            return;

        attackTimeoutProcess -= Time.deltaTime;
        TakeDelay -= Time.deltaTime;

        if (HasWeapon && attackTimeoutProcess <= 0)
        {
            var targets = Physics2D.OverlapCircleAll(transform.position, AttackRange, AttackMask.value);

            //print(targets.Length);
            if (targets.Length > 0)
            {
                foreach (var target in targets)
                {
                    var targetPosition = target.gameObject.transform.position;
                    var direction = (targetPosition - transform.position).normalized;
                    var hit = Physics2D.Raycast(transform.position, direction, AttackRange, AttackCheckMask.value);
                    //print(hit.collider.name);

                    if (hit.collider == target)
                    {
                        // Debug.DrawLine(transform.position, hit.point, Color.green, 10f);
                        Shoot(targetPosition);
                        return;
                    }
                    /*else
                        Debug.DrawLine(transform.position, hit.point, Color.red, 10f);*/
                }
            }
        }
    }

    private void Shoot(Vector3 targetPosition)
    {
        var bullet = Instantiate(BulletPrefab);
        bullet.transform.position = transform.position;

        //bullet.Direction = (targetPosition - transform.position).normalized;
        Vector3 dir = (targetPosition - transform.position);
        bullet.rb.velocity = dir.normalized * bullet.Speed;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        attackTimeoutProcess = AttackTimeout;


        Settings.Instance.ShotSound.Play();
    }

    internal void Equip(Equipment equipment)
    {
        Equipment = equipment;
        if (equipment.Type == EquipmentType.Weapon)
        {
            //WeaponMark.enabled = true;
            mover.animator.SetBool("Weapon", true);
        }


      /*  if (equipment.Type == EquipmentType.RepairKit)
            RepairKitMark.enabled = true;*/

        EquipmentMark.enabled = true;
        EquipmentMark.color = equipment.AuraRender.color;
        /*if(equipment.Type == EquipmentType.Weapon)
        {
            Hp = HpWithWeapon;
        }*/
    }

    public void ThrowEquipment()
    {
        // Assert.IsNotNull(Equipment);      

        //print("ThrowEquipment");

        LostItems(true);
    }

    public void Manage(bool manage)
    {
        SelectionMark.enabled = manage;
        mover.Manage(manage);
    }

    public void InfectedBy(Enemy2D infector)
    {
        if (infector == null)
            return;

        isInfected = true;

        sr.material.color = Settings.Instance.InfectedColor;//infector.Color;

        // todo
        var producer = gameObject.AddComponent<AgentProducer>();
        var templ = infector.GetComponent<AgentProducer>();
        producer.AgenContent = templ.AgenContent;
        producer.AgenPrefab = templ.AgenPrefab;
        producer.Timeout = templ.Timeout;
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

    public void TakeDamage(int value)
    {
        Hp -= value;
        if (Hp <= 0)
        {
            Dead();
        }
    }

    public void Dead(bool needCorpse = true)
    {
        // gameObject.SetActive(false);

        if (needCorpse)
        {
            // sr.sprite = DeadSprites[UnityEngine.Random.Range(0, DeadSprites.Count)];
            // sr.sortingOrder = 0;
            //Instantiate(DeadSpritePrefab, transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360)));

            var randomDeadPrefab = DeadPrefabs[UnityEngine.Random.Range(0, DeadPrefabs.Count)];
            Instantiate(randomDeadPrefab, transform.position, Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 360)));
            Instantiate(CorpsePrefab, transform.position, Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 360)));
        }

        if (needCorpse)
        {
            var effect = Instantiate(DeadEffectPrefab, transform.position, Quaternion.Euler(0, 0, 0));
            Destroy(effect, 1f);
        }

        LostItems(false);

        Game2D.Instance.LiverDead();

        if (needCorpse)
            Settings.Instance.LiverDeadSound.Play();

        /*
        Destroy(mover.animator);       
        Destroy(mover.rb);
        Destroy(mover.collider2d);      
        Destroy(mover);
        Destroy(this);*/
        if (ManageObject != null && ManageObject.Visitor == this)
        {
            ManageObject.Visitor = null;
        }

        if (EnemyInteract != null && EnemyInteract.LiverInteract == this)
        {
            EnemyInteract.LiverInteract = null;
            EnemyInteract.isBusy = false;
            EnemyInteract.mover.RestoreMove();
        }

        Destroy(gameObject);
    }

    internal void UseKey()
    {
        Destroy(Key.gameObject);
        Key = null;
        KeyMark.enabled = false;
    }

    internal void TakeKey(KeyCard keyCard)
    {
        Key = keyCard;
        KeyMark.enabled = true;
        KeyMark.color = keyCard.Color;// Settings.Instance.GetMarkColor(keyCard.Type);
    }

    private void LostItems(bool throwEquipment)
    {
        if (Equipment != null || Key != null)
        {
            TakeDelay = 1f;
        }

        if (Equipment != null)
        {
            Equipment.TakeOff(this, throwEquipment);

            if(Equipment.Type == EquipmentType.Weapon)
            {
                mover.animator.SetBool("Weapon", false);
            }

            Equipment = null;

            EquipmentMark.enabled = false;

            //WeaponMark.enabled = false;
            //RepairKitMark.enabled = false;
        }

        if (Key != null)
        {
            Key.TakeOff();

            Key = null;
            KeyMark.enabled = false;
        }
    }

    public void MoveTo(Vector3 point)
    {
        if (mover != null)
            mover.MoveTo(point);
    }

    private void OnDrawGizmos()
    {
        if (HasWeapon)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, AttackRange);
        }
    }
}
