using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NeedSelectLiver : MonoBehaviour
{
    public Liver2D Liver;
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
        if (Liver.Selected)
        {
            OnComplete?.Invoke();

            MessageUI?.SetActive(false);
            gameObject.SetActive(false);
        }
    }

    private void SetMark()
    {
        if (Mark != null)
            Mark.transform.position = Liver.transform.position + MarkOffset;
    }
}
