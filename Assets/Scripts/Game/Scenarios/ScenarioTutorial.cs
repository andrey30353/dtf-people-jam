using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioTutorial : MonoBehaviour
{
    public List<Door2D> OpenDoors;

    public MoveAndZoomCamera MoveAndZoomCamera;
    public NeedExposionDialogue NeedExposionDialogue;
    public TakeDinamit TakeDinamit;

    public List<Liver2D> DialogueLivers;
    public Liver2D ScenarionLiver;

    [Space]
    public GameObject NobodyGoToShelterMessage;

    public List<Door2D> UblockDoors;

    public static ScenarioTutorial Instance;

    private void Awake()
    {
        Instance = this;

        MoveAndZoomCamera.gameObject.SetActive(false);
        //NeedExposionDialogue.gameObject.SetActive(false);
        //TakeDinamit.gameObject.SetActive(false);
    }

    public void Start()
    {
        MoveAndZoomCamera.gameObject.SetActive(true);

        //foreach (var liver in DialogueLivers)
        //{
        //    liver.mover.StopMove();
        //}

        // открыть двери сразу
        foreach (var door in OpenDoors)
        {
            door.Select();
        }
    }
}
