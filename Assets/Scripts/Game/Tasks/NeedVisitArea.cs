using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NeedVisitArea : MonoBehaviour
{
    public List<Liver2D> Visiters;

    public BoxCollider2D AreaTrigger;

    public GameObject MessageUI;

    public UnityEvent OnComplete;

    private void Start()
    {
        MessageUI?.SetActive(true);
    }

    private void Update()
    {
        var completed = true;
        foreach (var liver in Visiters)
        {
            if (!AreaTrigger.OverlapPoint(liver.transform.position))
            {
                completed = false;
                break;
            }
        }

        if (completed)
        {
            OnComplete?.Invoke();

            MessageUI?.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}
