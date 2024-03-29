﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NeedRepair : MonoBehaviour
{
    public Destructible RepairObject;

    public GameObject MessageUI;

    public UnityEvent OnComplete;

    private void Start()
    {
        MessageUI?.SetActive(true);
    }

    private void Update()
    {
        if (!RepairObject.NeedRepair)
        {
            OnComplete?.Invoke();

            MessageUI?.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}
