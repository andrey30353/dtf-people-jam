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
                StartCoroutine(BreakCor(enemy));                
            }
        }
    }

    public void Break()
    {
        // поменять спрайт
        IsOpen = true;

        sr.sprite = BrokenSprite;
    }

    public IEnumerator BreakCor(Enemy2D enemy)
    {
        enemy.mover.StopMove();
        enemy.isBusy = true;

        yield return new WaitForSeconds(HatchList.Instance.BreakTime);

        enemy.mover.RestoreMove();
        enemy.isBusy = false;

        Hp--;
        if (Hp <= 0)
        {
            Break();
        }       
    }
}
