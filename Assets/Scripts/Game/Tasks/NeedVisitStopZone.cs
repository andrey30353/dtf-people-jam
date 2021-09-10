using UnityEngine;
using UnityEngine.Events;

public class NeedVisitStopZone : MonoBehaviour
{
    [SerializeField] private StopZone _zone;

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
        if (_zone.Visitor != null)
        {
            OnComplete?.Invoke();

            MessageUI?.SetActive(false);
            gameObject.SetActive(false);
        }
    }

    private void SetMark()
    {
        if (Mark != null)
            Mark.transform.position = _zone.transform.position + MarkOffset;
    }
}
