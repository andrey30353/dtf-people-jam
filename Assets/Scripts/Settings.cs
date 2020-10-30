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

    [Header("Двери")]
    public Color DefaultDoorColor;
    public Color WorkerDoorColor;
    public Color LabDoorColor;
    public Color ArmoryDoorColor;

    [Header("Ключ карточка")]
    public Color DefaultKeyColor;
    public Color WorkerKeyColor;
    public Color LabKeyColor;
    public Color ArmoryKeyColor;

    [Header("Звуки")]  
    public AudioSource EnemyDeadSound;
    public AudioSource LiverDeadSound;
    public AudioSource DoorSound;
    public AudioSource ShotSound;

    public Transform AgentsContent;

    public static Settings Instance;

    //public List<AgentProbably> AgentProbably;

    private void Awake()
    {
        Instance = this;
    }

    public Color GetDoorColor(KeyCardType key)
    {
        switch (key)
        {
            case KeyCardType.None:
                return DefaultDoorColor;
                
            case KeyCardType.WorkerKey:
                return WorkerDoorColor;
                
            case KeyCardType.LabKey:
                return LabDoorColor;

            case KeyCardType.ArmoryKey:
                return ArmoryDoorColor;

            default:
                return DefaultDoorColor;
        }
    }

    public Color GetMarkColor(KeyCardType key)
    {
        switch (key)
        {
            case KeyCardType.None:
                return DefaultKeyColor;

            case KeyCardType.WorkerKey:
                return WorkerKeyColor;

            case KeyCardType.LabKey:
                return LabKeyColor;

            case KeyCardType.ArmoryKey:
                return ArmoryKeyColor;

            default:
                return DefaultKeyColor;
        }
    }

}
