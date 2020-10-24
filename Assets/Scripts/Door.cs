using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Vector3 Move;

    public bool IsOpen;

    private bool InProcess;

    private void Start()
    {
        
    }

    [ContextMenu("Select")]
    public void Select()
    {
        //print("Select " + name);

        if (IsOpen)
            Close();
        else
            Open();
    }
    
    private void Open()
    {
        transform.localPosition -= Move * 0.5f;
        IsOpen = true;

    }

    //private IEnumerator Open()
    //{
    //    transform.localPosition -= Move * 0.5f;
    //    IsOpen = true;
    //}

    private void Close()
    {
        transform.localPosition += Move * 0.5f;
        IsOpen = false;
    }

    
}
