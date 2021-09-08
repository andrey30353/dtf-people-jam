using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScenarioTutorial : MonoBehaviour
{
    public MoveAndZoomCamera MoveAndZoomCamera;
    public NeedExposionDialogue NeedExposionDialogue;
    public TakeDinamit TakeDinamit;

    public List<Liver2D> DialogueLivers;
    public Liver2D ScenarionLiver;

    [Space]
    public GameObject NobodyGoToShelterMessage;

    public List<Door2D> UblockDoors;

    public UnityEvent OnStart;

    public static ScenarioTutorial Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        MoveAndZoomCamera.gameObject.SetActive(true);

        OnStart?.Invoke();
    }

    public void Complete()
    {
        Debug.Log("Complete");
    }

    public void Fail()
    {
        Debug.Log("Fail");
        PauseGame(true);
    }

    public void PauseGame(bool pause)
    {
        if (pause)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }
}
