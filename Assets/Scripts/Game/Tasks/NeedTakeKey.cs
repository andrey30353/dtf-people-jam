using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NeedTakeKey : MonoBehaviour
{
    public KeyCard Key;
    public GameObject Mark;
    public Vector3 MarkOffset;

    public GameObject MessageUI;

    public UnityEvent OnComplete;

    private void Start()
    {
        SetMark();

        MessageUI?.SetActive(true);
    }

    private void OnValidate()
    {
        SetMark();
    }

    private void Update()
    {
        if (Key.IsCarried)
        {
            OnComplete?.Invoke();

            MessageUI?.SetActive(false);
            gameObject.SetActive(false);
        }
    }

    private void SetMark()
    {
        if (Mark != null)
            Mark.transform.position = Key.transform.position + MarkOffset;
    }
}
