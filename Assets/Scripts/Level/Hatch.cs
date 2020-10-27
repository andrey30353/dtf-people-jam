using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hatch : MonoBehaviour
{
    public Sprite BrokenSprite;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //print(collision.gameObject.name);

        if (!HatchList.Instance.CanUse)
            return;

        var enemy = collision.GetComponent<Enemy2D>();
        if (enemy != null && enemy.CanUseHatch)
        {
            enemy.UseHatch(this);
        }
    }

    public void Broke()
    {
        // поменять спрайт
    }
}
