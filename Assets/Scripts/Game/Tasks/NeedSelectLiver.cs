using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NeedSelectLiver : MonoBehaviour
{
    public Liver2D Liver;

    public GameObject MessageUI;

    public UnityEvent OnComplete;

    private void Start()
    {
        MessageUI?.SetActive(true);
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
}
