using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CloudMessage : MonoBehaviour
{
    public SpriteRenderer Sr;
    public TMP_Text Text;

    //private void OnValidate()
    //{
    //    //print(Sr.size);
    //    Text.rectTransform.sizeDelta = Sr.size;
    //}

    void Start()
    {
        
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
