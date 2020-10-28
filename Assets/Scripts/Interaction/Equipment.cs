using UnityEngine;

public enum EquipmentType
{
    Weapon = 1,
    RepairKit = 2   
}


public class Equipment : MonoBehaviour
{
    public EquipmentType Type;  

    CircleCollider2D collider2d;
    Rigidbody2D rb;
    SpriteRenderer sr;

    Transform defaultParent;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collider2d = GetComponent<CircleCollider2D>();
        sr = GetComponent<SpriteRenderer>();

        defaultParent = transform.parent;
    }
        
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //print("OnCollisionEnter2D " + collision.gameObject.name);

        var liver = collision.gameObject.GetComponent<Liver2D>();
        if (liver != null)
        {           
            if (liver.Equipment == null)
            {   
                Take(liver);
            }
        }
    }

    public void Take(Liver2D liver)
    {
        //print("Take " + Type);

        rb.simulated = false;
        collider2d.enabled = false;       

        transform.SetParent(liver.transform);
        transform.localPosition = Vector3.zero;

        sr.enabled = false;

        liver.Equip(this);

        //sr.sortingOrder = 0;
    }

    public void TakeOff()
    {
        //print("TakeOff " + Type);

        collider2d.enabled = true;
        rb.simulated = true;

        sr.enabled = true;

        transform.SetParent(defaultParent);

    }
}
