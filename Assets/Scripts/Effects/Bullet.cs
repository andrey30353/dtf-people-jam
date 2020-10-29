using UnityEngine;

public class Bullet : MonoBehaviour
{   
    public int Damage = 1;
    public int Speed = 5;
    
    public Vector3 Direction;
     
    public Rigidbody2D rb;
    //private void Start()
    //{
    //    rb = GetComponent<Rigidbody2D>();
    //}

    //private void Update()
    //{
    //    transform.position += Direction * Speed * Time.deltaTime;
    //}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var enemy = collision.gameObject.GetComponent<Enemy2D>();
        if (enemy != null)
        {
            enemy.TakeDamage(Damage, rb.velocity);
        }

        //print(collision.gameObject.name);
        //// игнорируем своих
        //var liver = collision.gameObject.GetComponent<Liver2D>();
        //if (liver != null)
        //    return;

        Destroy(gameObject);
    }
    /*
    private void OnCollisionEnter2D(Collision2D collision)
    {      
        var enemy = collision.gameObject.GetComponent<Enemy2D>();
        if (enemy != null)
        {
            enemy.TakeDamage(Damage);           
        }

        print(collision.gameObject.name);
        //// игнорируем своих
        //var liver = collision.gameObject.GetComponent<Liver2D>();
        //if (liver != null)
        //    return;

        Destroy(gameObject);
    }*/
}
