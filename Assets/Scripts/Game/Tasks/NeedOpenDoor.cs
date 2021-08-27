using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NeedInteractWithDoor : MonoBehaviour
{
    public Door2D Door;

    public bool Open;

    public GameObject MessageUI;

    public UnityEvent OnComplete;

    private void Start()
    {
        MessageUI?.SetActive(true);
    }

    private void Update()
    {
        if (!Door.IsOpen)
        {
            OnComplete?.Invoke();
            gameObject.SetActive(false);
        }
    }
}
