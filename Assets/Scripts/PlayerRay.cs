using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRay : MonoBehaviour
{
    public int Distance;
    public LayerMask LayerMask;

    Camera camera;
    void Start()
    {
        camera = Camera.main;
    }

    // Update is called once per frame 
    void LateUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {

            var ray = camera.ScreenPointToRay(Input.mousePosition);
            // Debug.DrawRay(camera.transform.position, Vector3.down * 20, Color.yellow);

            if (Physics.Raycast(ray, out var hit, Distance, LayerMask))
            {
                var door = hit.collider.gameObject.GetComponent<Door>();
                if (door == null)
                    return;

                door.Select();
            }
        }

    }
}
