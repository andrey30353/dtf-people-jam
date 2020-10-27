using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class HatchList : MonoBehaviour
{ 
    public bool Using;

    public float SpeedKoefInHatch = 0.7f;

    // сколько времени нельзя пользоваться вентиляцией
    public float HatchTimeout = 10f;
    private float hatchTimeoutProcess;

    public float BreakTime = 1f;

    public bool CanUse => !Using && hatchTimeoutProcess > HatchTimeout;

    public Hatch[] Hatches;

    public static HatchList Instance;
  

    private void Awake()
    {
        Assert.IsNull(Instance);
        Instance = this;
    }

    private void Start()
    {
        Hatches = GetComponentsInChildren<Hatch>();
    }

    private void Update()
    {
        if (Using)
            return;
       
        hatchTimeoutProcess += Time.deltaTime;
    }

    public Hatch GetRandomHatch(Hatch enter)
    {
        var result = Hatches[UnityEngine.Random.Range(0, Hatches.Length)];

        while (result == enter)
        {
            result = Hatches[UnityEngine.Random.Range(0, Hatches.Length)];
        }
        return result;       
    }

    internal void Use(bool value)
    {
        Using = value;

        if (value == false)
            hatchTimeoutProcess = 0;
    }
}
