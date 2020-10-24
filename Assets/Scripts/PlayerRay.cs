using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRay : MonoBehaviour
{
    public int Distance;
    public LayerMask LayerMask;

    public LayerMask FloorLayerMask;  

    public float Radius;
    private Vector3 point;

    public Agent selectedAgent;

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
                if (door != null)
                {
                    door.Select();
                }

                var agent = hit.collider.gameObject.GetComponent<Agent>();
                if (agent != null)
                {
                    if (selectedAgent != null && selectedAgent != agent)
                    {
                        selectedAgent.Manage(false);
                    }

                    selectedAgent = agent;                    
                    agent.Manage(true);                   
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            var ray = camera.ScreenPointToRay(Input.mousePosition);                     
          
            if (Physics.Raycast(ray, out var hit, Distance, FloorLayerMask))
            {
                point = hit.point;

                Game.Instance.MoveAgents(point, Radius);
                Debug.DrawLine(point, point + Vector3.up * 5, Color.yellow, 10);              
            }
        }

    }

    private void OnDrawGizmos()
    {
        if (point == Vector3.zero)
            return;

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(point, Radius);
    }
}
