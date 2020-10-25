using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AgentProbably
{
    public AgentType Type;
    public float RandomValue;
}

public class Settings : MonoBehaviour
{
    public Color LiveColor;

    public Color HunterColor;
    public Color InfectorColor;
    public Color QueenColor;
    [Space]
    public Color InfectedColor;

    public Transform AgentsContent;

    public static Settings Instance;

    //public List<AgentProbably> AgentProbably;

    private void Awake()
    {
        Instance = this;
    }

    
}
