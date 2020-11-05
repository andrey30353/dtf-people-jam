using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum KeyCardType
{   
    None = 0,
    WorkerKey = 1,
    LabKey = 2,
    ArmoryKey = 3
}

// todo можно выделить общий класс с Equipment
public class KeyCard : MonoBehaviour
{    
    public KeyCardType Type;

    public Color Color => sr.color;

    private CircleCollider2D collider2d;
    private SpriteRenderer sr;

    private Transform defaultParent;

    private void Start()
    {      
        collider2d = GetComponent<CircleCollider2D>();
        sr = GetComponent<SpriteRenderer>();

        defaultParent = transform.parent;
    }   

    private void OnTriggerEnter2D(Collider2D collision)
    {
       // print("OnTriggerEnter2D " + collision.gameObject.name);

        var liver = collision.gameObject.GetComponent<Liver2D>();
        if (liver != null)
        {
            if (liver.Key == null && liver.TakeDelay <= 0)
            {
                Take(liver);
            }
        }       
    }

    public void Take(Liver2D liver)
    {
       // print("Take " + Type);
       
        collider2d.enabled = false;        

        transform.SetParent(liver.transform);
        transform.localPosition = Vector3.zero;

        sr.enabled = false;

        liver.TakeKey(this);       
    }

    public void TakeOff()
    {
       // print("TakeOff key");

        collider2d.enabled = true;

        sr.enabled = true;

        transform.SetParent(defaultParent);
    }

    public void UseCard()
    {
        Destroy(gameObject);
    }
}
