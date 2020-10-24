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
       

    public void Select()
    {      
        if (IsOpen)
            Close();
        else
            Open();
    }
    
    private void Open()
    {
        transform.localPosition = OpenPosition;
      
        IsOpen = true;

    }

    //private IEnumerator OpenCor()
    //{
    //    transform.localPosition -= Move * 0.5f;
    //    IsOpen = true;
    //}

    private void Close()
    {
        transform.localPosition = ClosePosition;
       
        IsOpen = false;
    }
    
}
