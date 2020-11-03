using UnityEngine;

public class Bullet : MonoBehaviour
{   
    public int Damage = 1;
    public int Speed = 5;
   
    // todo private
    public Rigidbody2D rb;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var enemy = collision.gameObject.GetComponent<Enemy2D>();
        if (enemy != null)
        {
            enemy.TakeDamage(Damage, rb.velocity);
        }
      
        Destroy(gameObject);
    }    
}
