using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRay2D : MonoBehaviour
{
    public int Distance;

    public LayerMask InteractionMask;

    public LayerMask FloorLayerMask;  

    public float Radius;
    public Vector3 point;
 
    public Liver2D selectedAgent;

    private CameraMover cameraMover;

    Camera camera;
    void Start()
    {
        camera = Camera.main;
        point = Vector3.up;

        cameraMover = GetComponent<CameraMover>();
    }

    // Update is called once per frame 
    void LateUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {           
            var origin = camera.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.zero, Distance, InteractionMask);            
           
            if (hit.collider != null)
            {               
                var door = hit.collider.gameObject.GetComponent<Door2D>();
                if (door != null)
                {
                    door.Select();
                }

                var agent = hit.collider.gameObject.GetComponent<Liver2D>();
                if (agent != null)
                {
                    if (selectedAgent != null && selectedAgent != agent)
                    {
                        selectedAgent.Manage(false);                      
                    }                    

                    selectedAgent = agent;                    
                    agent.Manage(true);

                    //cameraMover.SetZoom(0.5f);
                }
            }
            else
            {
                if (selectedAgent != null )
                {
                    selectedAgent.Manage(false);
                    selectedAgent = null;
                }
            }
        }

        if (Input.GetMouseButton(1))
        {
            var origin = camera.ScreenToWorldPoint(Input.mousePosition);
            origin.z = 0;
            //print(origin);
            
            Game2D.Instance.MoveAgents(origin, Radius);           
        }

        cameraMover.folowLiver = selectedAgent;
    }

    private void OnDrawGizmos()
    {
        if (point == Vector3.zero)
            return;

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(point, Radius);
    }
}
