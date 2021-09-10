using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NeedLiveAnybody : MonoBehaviour
{
    public GameObject MessageUI;

    public UnityEvent OnStart;

    public UnityEvent OnComplete;

    public UnityEvent OnFail;

    private void Start()
    {
        if(MessageUI != null)
           MessageUI.SetActive(true);

        OnStart?.Invoke();
    }

    private void Update()
    {
        if (Game2D.Instance.LiverCount == 0)
        {
            OnFail?.Invoke();

            gameObject.SetActive(false);
        }
    }
}
