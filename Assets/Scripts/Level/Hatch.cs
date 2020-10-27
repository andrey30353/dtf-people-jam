using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hatch : MonoBehaviour
{  
    public bool IsOpen;

    public int Hp;

    public Sprite BrokenSprite;

    private SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //print(collision.gameObject.name);
                
        var enemy = collision.GetComponent<Enemy2D>();
        if (enemy != null && enemy.CanUseHatch)
        {
            if (IsOpen)
            {
                if (HatchList.Instance.CanUse)
                {
                    enemy.UseHatch(this);
                } 
            }
            else
            {
                Hp--;
                if(Hp <= 0)
                {
                    Break();
                }
            }
        }
    }

    public void Break()
    {
        // поменять спрайт
        IsOpen = true;

        sr.sprite = BrokenSprite;
    }
}
