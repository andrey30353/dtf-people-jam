using TMPro;
using UnityEngine;

public class CloudMessageUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Liver2D Owner;

    [SerializeField] private float _up;

    private RectTransform _rectTransform;

    void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _rectTransform.anchorMin = new Vector2(0.5f, 0);
        _rectTransform.anchorMax = new Vector2(0.5f, 0);
        _rectTransform.pivot = new Vector2(0.5f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if(Owner != null)
        {
            var position = Owner.transform.localPosition;
            position.y += _up;
            var screenPoint = Camera.main.WorldToScreenPoint(position);
            _rectTransform.position = screenPoint;
        }
    }

    public void SetMessage(Message message)
    {
        _text.text = message.Text;
        Owner = message.Owner;
    }
}
