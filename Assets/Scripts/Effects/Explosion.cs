using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float Delay = 1;

    public int Damage = 3;

    public int Radius = 3;

    public int Force = 3;

    public LayerMask Mask;

    public ParticleSystem Effect;

    // Start is called before the first frame update
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(Delay);

        Explode();
    }

    private void Explode()
    {
        var colliders = Physics2D.OverlapCircleAll(transform.position, Radius, Mask.value);
        foreach (var collider in colliders)
        {
            var destroible = collider.GetComponent<Destroible>();
            if (destroible != null)
            { 
                destroible.Break();
                continue;
            }

            var liver = collider.GetComponent<Liver2D>();
            if (liver != null)
            {
                var dist = (liver.transform.position - transform.position).magnitude / Radius * Damage;
                liver.TakeDamage(Mathf.RoundToInt(dist));
                continue;
            }

            var enemy = collider.GetComponent<Enemy2D>();
            if (enemy != null)
            {
                var dist = (liver.transform.position - transform.position).magnitude / Radius * Damage;
                enemy.TakeDamage(Mathf.RoundToInt(dist), transform.position);
                continue;
            }

            var rb = collider.GetComponent<Rigidbody2D>();
            if(rb != null)
            {
                var force = (rb.transform.position - transform.position).normalized * Force;
                rb.AddForce(force, ForceMode2D.Impulse);
            }
           
        }

        Effect.Play();

        Destroy(gameObject, 0.1f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Radius);
    }
}
