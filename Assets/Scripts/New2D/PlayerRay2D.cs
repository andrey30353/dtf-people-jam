﻿using System.Collections;
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
            
            Game2D.Instance.MoveAgents(origin, Radius, selectedAgent);           
        }

        cameraMover.folowLiver = selectedAgent;
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

    private void OnDrawGizmos()
    {
        if (point == Vector3.zero)
            return;

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(point, Radius);
    }
}
