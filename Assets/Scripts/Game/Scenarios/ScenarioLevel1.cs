using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioLevel1 : MonoBehaviour
{
    public List<Door2D> OperDoors;

    public MoveAndZoomCamera MoveAndZoomCamera;
    public NeedExposionDialogue NeedExposionDialogue;
    public TakeDinamit TakeDinamit;

    public static ScenarioLevel1 Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        foreach (var door in OperDoors)
        {
            door.Select();
        }
    }
}
