using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatewayVacuum : MonoBehaviour
{
    public BoxCollider2D[] triggers;

    public LayerMask enviromentMask;
       
    public bool UseEnviroment;

    private void Start()
    {
        foreach (var item in triggers)
        {
            item.enabled = false;
        }
    }


    public void Switch(bool enable)
    {
        foreach (var item in triggers)
        {
            item.enabled = enable;

            if (UseEnviroment )
            {
                var res = Physics2D.OverlapBoxAll(item.bounds.center, item.bounds.size, 0, enviromentMask.value);
                foreach (var coll in res)
                {
                    var rb = coll.gameObject.AddComponent<Rigidbody2D>();
                    rb.gravityScale = 0;
                    // print(coll.name);
                }
            }           
        }       
    }
}
