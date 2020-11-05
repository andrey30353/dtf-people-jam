using System.Collections.Generic;
using UnityEngine;

public class Destroible : MonoBehaviour
{   
    public List<Sprite> BrokenSprite;   

    public float Hp = 1;

    public float BrokenScaleMin = 1f;
    public float BrokenScaleMax = 2f;

    public bool Broken => Hp <= 0;

    private BoxCollider2D boxCollider;
    private SpriteRenderer sr;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var enemy = collision.gameObject.GetComponent<Enemy2D>();
        if (enemy != null)
        {
            Break();
        }              
    }

    public void Break()
    {
        if(BrokenSprite != null && BrokenSprite.Count != 0)
        {
            var randomSprite = BrokenSprite[Random.Range(0, BrokenSprite.Count)];
            sr.sprite = randomSprite;

            transform.localScale *= Random.Range(BrokenScaleMin, BrokenScaleMax);
            transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));

            Destroy(boxCollider);
            Destroy(this);
        }
        else
            Destroy(gameObject);
    }
}
