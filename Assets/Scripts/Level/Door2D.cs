using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Door2D : MonoBehaviour
{
    public Sprite DamagedSprite;
    public Sprite BrokenSprite;

    public BoxCollider2D BaseCollider;
    public List<BoxCollider2D> BrokenColliders;

    public bool IsOpen;

    public KeyCardType RequireKey;
    private bool blocked;

    public bool IsClosed => !IsOpen;

    private bool InProcess;

    public Vector3 OpenPosition;
    public Vector3 ClosePosition;

    public float time = 0.5f;

    public int Hp = 5;
    private int startHp;

    public bool Broken => Hp <= 0;
    public bool Damaged => Hp <= startHp / 2;

    public List<Door2D> OpeningDepends;

    //public List<Door2D> CloseDoorIfOpen;

    public Door2D LinkedDoor;

    public UnityEvent OpenEvent;
    public UnityEvent CloseEvent;

    private SpriteRenderer sr;

    private void OnValidate()
    {
        if (LinkedDoor != null)
            LinkedDoor.LinkedDoor = this;

        //var settings = Settings.Instance;
        //if (settings == null)
        //{
        //    settings = GameObject.Find("Settings").GetComponent<Settings>();
        //}      
        //sr.color = settings.GetDoorColor(RequireKey);

        if (OpeningDepends != null)
        {
            foreach (var door in OpeningDepends)
            {
                if (door == null)
                    continue;

                if (door.OpeningDepends == null)
                    continue;

                if (!door.OpeningDepends.Contains(this))
                    door.OpeningDepends.Add(this);
                //OpenEvent += Close;
            }
        }
    }

    private void Start()
    {
        startHp = Hp;
        sr = GetComponent<SpriteRenderer>();
        if (RequireKey != KeyCardType.None)
        {
            blocked = true;

            sr.color = Settings.Instance.GetDoorColor(RequireKey);
        }

        foreach (var item in OpeningDepends)
        {
            //OpenEvent += Close;
        }
    }

    public void Select()
    {
        if (blocked || Broken || InProcess)
            return;

        if (IsOpen)
            Close();
        else
            Open();
    }

    private void Open()
    {
        // нельзя, если они в процессе открывания
        foreach (var door in OpeningDepends)
        {
            if (door.IsClosed && door.InProcess)
                return;
        }
        // закрываем зависимые открытые двери
        foreach (var door in OpeningDepends)
        {
            if (door.IsOpen)
                door.Close();
        }

        OpenEvent?.Invoke();

        StartCoroutine(OpenCor());
        if (LinkedDoor != null)
        {
            StartCoroutine(LinkedDoor.OpenCor());
        }
    }

    private IEnumerator OpenCor()
    {
        InProcess = true;

        var elapsedTime = 0f;

        while (elapsedTime < time)
        {
            transform.localPosition = Vector3.Lerp(ClosePosition, OpenPosition, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        IsOpen = true;

        InProcess = false;
    }

    private void Close()
    {
        CloseEvent?.Invoke();

        StartCoroutine(CloseCor());
        if (LinkedDoor != null)
        {
            StartCoroutine(LinkedDoor.CloseCor());
        }
    }

    private IEnumerator CloseCor()
    {
        InProcess = true;

        var elapsedTime = 0f;

        while (elapsedTime < time)
        {
            transform.localPosition = Vector3.Lerp(OpenPosition, ClosePosition, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        IsOpen = false;

        InProcess = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (Broken)
            return;

        // Kill()


        if (InProcess)
        {
            // только закрывающаяся дверь может прищемить
            if (IsClosed)
                return;

            var agent = collision.gameObject.GetComponent<Liver2D>();
            if (agent != null)
                agent.Dead();
        }
        else
        {   // ломается только закрытая дверь
            if (IsClosed)
            {
                var agent = collision.gameObject.GetComponent<Enemy2D>();
                if (agent != null)
                {
                    Hp--;
                    CheckDoorHp();
                }

                if (blocked)
                {
                    var liver = collision.gameObject.GetComponent<Liver2D>();
                    if (liver != null && liver.Key != null && liver.Key.Type == RequireKey)
                        Unblock(liver);
                }
            }
        }

    }

    private void CheckDoorHp()
    {
        if (Broken)
        {
            Break();
            return;
        }

        if (Damaged)
        {
            sr.sprite = DamagedSprite;
        }
    }

    private void Break()
    {
        OpenEvent?.Invoke();

        BaseCollider.enabled = false;

        foreach (var brokenColl in BrokenColliders)
        {
            brokenColl.enabled = true;
        }

        sr.sprite = BrokenSprite;

        //gameObject.SetActive(false);
    }

    private void Unblock(Liver2D liver, bool withLinked = true)
    {
        blocked = false;

        if (liver != null)
        {
            liver.UseKey();
        }

        // withLinked - чтобы убрать зацикливание
        if (LinkedDoor != null && withLinked)
        {
            LinkedDoor.Unblock(null, false);
        }

        sr.color = Settings.Instance.GetDoorColor(KeyCardType.None);
    }

    [ContextMenu("Reset")]
    private void Reset()
    {
        OpenPosition = transform.localPosition;
        ClosePosition = transform.localPosition;
    }
}
