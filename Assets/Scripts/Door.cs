using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{   
    public bool IsOpen;

    public bool IsClosed => !IsOpen;

    private bool InProcess;

    public Vector3 OpenPosition;
    public Vector3 ClosePosition;

    public float time = 0.5f;

    public int Hp = 5;

    public bool Broken => Hp <= 0;

    private void Start()
    {
        //Hp
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
        StartCoroutine(OpenCor());
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
        StartCoroutine(CloseCor());
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

    private void OnCollisionEnter(Collision collision)
    {
        if (Broken)
            return;

        // Kill()

        if (InProcess)
        {
            // только закрывающаяся дверь может прищемить
            if (IsClosed)
                return;

            if (collision.gameObject.tag != "Agent")
                return;

            var agent = collision.gameObject.GetComponent<Agent>();
            if (agent == null)
                return;

            if (agent.Dead)
                return;

            agent.Kill();           
        }
        else
        {   // ломается только закрытая дверь
            if (IsClosed)
            {
                if (collision.gameObject.tag != "Agent")
                    return;

                var agent = collision.gameObject.GetComponent<Agent>();
                if (agent == null)
                    return;

                if (agent.Dead /*&& agent.BreakDoor*/)
                {
                    Hp--;
                    CheckDoorHp();
                }
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
        gameObject.SetActive(false);
    }
}
