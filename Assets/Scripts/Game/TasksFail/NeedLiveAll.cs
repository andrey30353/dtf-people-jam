using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NeedLiveAll : MonoBehaviour
{
    public GameObject MessageUI;

    public UnityEvent OnStart;

    public UnityEvent OnComplete;

    public UnityEvent OnFail;

    private void Start()
    {
        MessageUI?.SetActive(true);

        OnStart?.Invoke();
    }

    private void Update()
    {
        if (Game2D.Instance.LiverCount < Game2D.Instance.LiverCountStart)
        {
            OnFail?.Invoke();

            gameObject.SetActive(false);
        }
    }
}
