using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatewayVacuum : MonoBehaviour
{
    public BoxCollider2D[] triggers;

    public bool Enabled;

    private void Start()
    {
        Switch(Enabled);
    }

    public void Switch(bool enable)
    {
        foreach (var item in triggers)
        {
            item.enabled = enable;
        }
    }
}
