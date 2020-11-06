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

    public float pointSize = 1f;
       
    public LayerMask AgentMask;

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
        if (Time.timeScale == 0)
            return;

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
                    SelectLiver(agent);
                }
            }
            else
            {
                var colliders = Physics2D.OverlapCircleAll(origin, pointSize, InteractionMask);
                if(colliders == null || colliders.Length == 0)
                {
                    // todo копипаст 
                    if (selectedAgent != null)
                    {
                        selectedAgent.Manage(false);
                        selectedAgent = null;
                    }
                }
                else
                {
                    float minDist = int.MaxValue;
                    Liver2D agent = null;
                    foreach (var col in colliders)
                    {
                        var currAgen = col.gameObject.GetComponent<Liver2D>();
                        if (currAgen == null)
                            continue;

                        var dist = (origin - transform.position).sqrMagnitude;
                        if(dist < minDist)
                        {
                            agent = currAgen;
                            minDist = dist;
                        }
                    }

                    if(agent != null)
                    {
                        SelectLiver(agent);
                    }
                    else
                    {
                        // todo копипаст 
                        if (selectedAgent != null)
                        {
                            selectedAgent.Manage(false);
                            selectedAgent = null;
                        }
                    }
                }

               
            }
        }

        if (Input.GetMouseButton(1))
        {
            var origin = camera.ScreenToWorldPoint(Input.mousePosition);
            origin.z = 0;
            //print(origin);
            
            MoveAgents(origin, Radius, selectedAgent);           
        }

        // бросить предмет
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (selectedAgent != null /*&& selectedAgent.Equipment != null*/)
            {
                selectedAgent.ThrowEquipment();
            }
        }  

        cameraMover.FolowLiver = selectedAgent;
    }

    private void SelectLiver(Liver2D agent)
    {
        if (selectedAgent != null && selectedAgent != agent)
        {
            selectedAgent.Manage(false);
        }

        selectedAgent = agent;
        agent.Manage(true);

        //cameraMover.SetZoom(0.5f);
    }

    public void MoveAgents(Vector3 point, float radius, Liver2D except)
    {
        // var result = new Collider[10];

        var res = Physics2D.OverlapCircleAll(point, radius, AgentMask.value);
        // Physics.OverlapSphereNonAlloc(point, radius, result, AgentMask.value);

        //print(res.Length);

        foreach (var item in res)
        {
            var agent = item.GetComponent<Liver2D>();

            if (except != null && agent == except)
                continue;

            // todo
            if (!agent.isBusy)
                agent.MoveTo(point);
        }


        // fore
    }

    private void OnDrawGizmos()
    {
        if (point == Vector3.zero)
            return;

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(point, Radius);
    }
}
