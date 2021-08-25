using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CloudMessage : MonoBehaviour
{
    public SpriteRenderer Sr;
    public TMP_Text Text;

    public Liver2D Owner;

    //private void OnValidate()
    //{
    //    //print(Sr.size);
    //    Text.rectTransform.sizeDelta = Sr.size;
    //}

    void Start()
    {
        var position = Owner.transform.localPosition;
        var screenPoint = Camera.main.WorldToScreenPoint(position);
              
        //_rectTransform.position = screenPoint;

        transform.localPosition = Owner.transform.localPosition;
        transform.LookAt(Camera.main.transform.position);
    }

    // Update is called once per frame
    void Update()
    {

    }

    [ContextMenu("UpdateTextSize")]
    private void UpdateTextSize()
    {
        Text.rectTransform.sizeDelta = Sr.size;
    }
}
