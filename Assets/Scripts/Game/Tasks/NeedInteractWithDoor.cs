using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NeedInteractWithDoor : MonoBehaviour
{
    public Door2D Door;
    public bool NeedOpen;

    public GameObject Mark;
    public Vector3 MarkOffset;

    public GameObject MessageUI;

    public UnityEvent OnStart;

    public UnityEvent OnComplete;

    private void Start()
    {
        SetMark();

        MessageUI?.SetActive(true);

        OnStart?.Invoke();
    }

    private void OnValidate()
    {
        SetMark();
    }

    private void Update()
    {
        if (Door.IsOpen == NeedOpen)
        {
            OnComplete?.Invoke();

            MessageUI?.SetActive(false);
            gameObject.SetActive(false);
        }
    }

    private void SetMark()
    {
        if (Mark != null)
            Mark.transform.position = Door.transform.position + MarkOffset;
    }
}
