using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NeedLiveAll : MonoBehaviour
{
    public List<Liver2D> Livers;

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
        var allLive = true;
        for (int i = 0; i < Livers.Count; i++)
        {
            if (Livers[i] == null)
            {
                allLive = false;
                break;
            }
        }

        if (allLive == false)
        {
            OnFail?.Invoke();

            MessageUI?.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}
