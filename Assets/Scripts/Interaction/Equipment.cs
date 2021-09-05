using UnityEngine;
using UnityEngine.Events;

public enum EquipmentType
{
    Weapon = 1,
    RepairKit = 2
}


public class Equipment : MonoBehaviour
{
    public EquipmentType Type;

    public bool ShowOnWear;
    public float WearingScaleRatio;

    public SpriteRenderer AuraRender;

    public Liver2D Carrier { get; private set; }
    public bool IsCarried => Carrier != null;

    public UnityEvent OnThrowEvent;

    private CircleCollider2D collider2d;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private Transform defaultParent;
    private Vector3 throwForce;

    private Vector3 startScale;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collider2d = GetComponent<CircleCollider2D>();
        sr = GetComponent<SpriteRenderer>();

        defaultParent = transform.parent;

        startScale = transform.localScale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //print("OnCollisionEnter2D " + collision.gameObject.name);

        var liver = collision.gameObject.GetComponent<Liver2D>();
        if (liver != null)
        {
            if (liver.Equipment == null && liver.TakeDelay <= 0)
            {

                Take(liver);
            }
        }
    }

    /*private void FixedUpdate()
    {
        if(throwForce != Vector3.zero)
        {
            rb.AddForce(throwForce, ForceMode2D.Impulse);
            throwForce = Vector3.zero;
        }
    }*/

    private void Take(Liver2D liver)
    {
        //print("Take " + Type);
        liver.Equip(this);

        rb.simulated = false;
        collider2d.enabled = false;

        transform.SetParent(liver.transform);
        // держим немного впереди
        transform.localPosition = Vector3.zero + Vector3.down * 0.2f;

        if (ShowOnWear)
        {
            transform.localRotation = Quaternion.identity;
            transform.localScale = startScale * WearingScaleRatio;
            AuraRender.enabled = false;
        }
        else
        {
            sr.enabled = false;
            AuraRender.enabled = false;
        }


        Carrier = liver;

        //sr.sortingOrder = 0;
    }

    public void TakeOff(Liver2D owner, bool throwing)
    {
        //print("TakeOff " + Type);

        Carrier = null;

        collider2d.enabled = true;
        rb.simulated = true;

        if (throwing)
        {
            var force = (transform.position - owner.transform.position).normalized * 7f;
            throwForce = force;
            rb.velocity = force;
            /*  print(force);
              rb.AddForce(force, ForceMode2D.Impulse);*/

            OnThrowEvent?.Invoke();
        }


        sr.enabled = true;

        transform.SetParent(defaultParent);

        if (ShowOnWear)
        {
            transform.localScale = startScale;
        }
        AuraRender.enabled = true;
    }
}
