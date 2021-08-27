using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NeedRepair : MonoBehaviour
{
    public Destructible RepairObject;

    public UnityEvent OnComplete;

    private void Update()
    {
        if (!RepairObject.NeedRepair)
        {
            OnComplete?.Invoke();
            gameObject.SetActive(false);
        }
    }
}
