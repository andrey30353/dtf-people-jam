using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NeedTakeEquipment : MonoBehaviour
{
    public Equipment Equipment;

    public GameObject MessageUI;

    public UnityEvent OnComplete;

    private void Start()
    {
        MessageUI?.SetActive(true);
    }

    private void Update()
    {
        if (Equipment.IsCarried)
        {
            OnComplete?.Invoke();

            MessageUI?.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}
