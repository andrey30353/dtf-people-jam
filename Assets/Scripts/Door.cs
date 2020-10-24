using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Vector3 Move;

    public bool IsOpen;

    private bool InProcess;

    public Vector3 OpenPosition;
    public Vector3 ClosePosition;

    public float time = 0.5f;


    public void Select()
    {
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

    

}
