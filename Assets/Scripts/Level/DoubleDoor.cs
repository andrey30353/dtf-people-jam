using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoubleDoor2D : MonoBehaviour
{

    public Door2D Door1;
    public Door2D Door2;
       
    public UnityEvent OpenEvent;
    public UnityEvent CloseEvent;
     
    private void Start()
    {
        //Hp
    }
    /*
    public void Select()
    {
        if (Door1.Broken && Door2.Broken)
            return;

        if (Door1.InProcess || Door2.InProcess)
            return;

        if (IsOpen)
            Close();
        else
            Open();
    }

    private void Open()
    {
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

        gameObject.SetActive(false);
    }

    [ContextMenu("Reset")]
    private void Reset()
    {
        OpenPosition = transform.localPosition;
        ClosePosition = transform.localPosition;
    }*/
}
