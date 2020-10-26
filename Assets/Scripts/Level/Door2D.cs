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

    public bool IsClosed => !IsOpen;

    private bool InProcess;

    public Vector3 OpenPosition;
    public Vector3 ClosePosition;

    public float time = 0.5f;

    public int Hp = 5;

    public bool Broken => Hp <= 0;

    public List<Door2D> OpeningDepends;

    public Door2D LinkedDoor;

    public UnityEvent OpenEvent;
    public UnityEvent CloseEvent;

  
    private SpriteRenderer sr;

    private void OnValidate()
    {
        if (LinkedDoor != null)
            LinkedDoor.LinkedDoor = this;
    }

    private void Start()
    {
        //Hp       
        sr = GetComponent<SpriteRenderer>();
    }

    public void Select()
    {
        if (Broken)
            return;

        if (InProcess)
            return;

        if (IsOpen)
            Close();
        else
            Open();
    }

    private void Open()
    {
       /* var canOpen = true;
        foreach (var door in OpeningDepends)
        {
            if (door.IsOpen)
                return;
        }
        */
        //if()

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

            //if (collision.gameObject.tag != "Agent")
            //    return;

            var agent = collision.gameObject.GetComponent<Liver2D>();
            if (agent == null)
                return;

            agent.Dead();
        }
        else
        {   // ломается только закрытая дверь
            if (IsClosed)
            {
                var agent = collision.gameObject.GetComponent<Enemy2D>();
                if (agent == null)
                    return;

                Hp--;
                CheckDoorHp();
            }
        }

    }

    private void CheckDoorHp()
    {
        if (Broken)
        {
            Break();
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

    [ContextMenu("Reset")]
    private void Reset()
    {
        OpenPosition = transform.localPosition;
        ClosePosition = transform.localPosition;
    }
}
