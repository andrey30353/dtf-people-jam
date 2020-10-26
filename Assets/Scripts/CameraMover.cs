using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    public float MinZoom, MaxZoom;

    public float MinX_MinZoom, MaxX_MinZoom;
    public float MinX_MaxZoom, MaxX_MaxZoom;

    public float MinY_MinZoom, MaxY_MinZoom;
    public float MinY_MaxZoom, MaxY_MaxZoom;
    
    private float zoom;

    public float Speed = 1f;

    public float Threshold = 0.2f;

    Camera camera;

    float z;

    private void Start()
    {
        camera = Camera.main;

        z = camera.transform.localPosition.z;

        AdjustZoom(0);
    }

    // Update is called once per frame
    void Update()
    {
        float zoomDelta = Input.GetAxis("Mouse ScrollWheel");
        if (zoomDelta != 0f)
        {
            AdjustZoom(zoomDelta);
        }



        //float xDelta = Input.GetAxis("Horizontal");
        //float yDelta = Input.GetAxis("Vertical");

        float xDelta = 0f;
        float yDelta = 0f;

        //print(Input.mousePosition.y);

        if (Input.mousePosition.x < Screen.width * Threshold) 
            xDelta = -1;
       
        if (Input.mousePosition.x > Screen.width - Screen.width * Threshold)        
            xDelta = 1;
       
        if (Input.mousePosition.y < Screen.height * Threshold)
                yDelta = -1;
        
        if (Input.mousePosition.y > Screen.height - Screen.height * Threshold)
                yDelta = 1;        

        if (xDelta != 0f || yDelta != 0f)
        {
            AdjustPosition(xDelta, yDelta);
        }
    }

    void AdjustZoom(float delta)
    {

        zoom = Mathf.Clamp01(zoom + delta);

        float distance = Mathf.Lerp(MaxZoom, MinZoom, zoom);
        camera.orthographicSize = distance;
    }


    void AdjustPosition(float xDelta, float yDelta)
    {
        Vector3 direction = new Vector2(xDelta, yDelta).normalized;
        float damping = Mathf.Max(Mathf.Abs(xDelta), Mathf.Abs(yDelta));
              
        var minX = Mathf.Lerp(MinX_MinZoom, MinX_MaxZoom, zoom);
        var maxX = Mathf.Lerp(MaxX_MinZoom, MaxX_MaxZoom, zoom);

        var minY = Mathf.Lerp(MinY_MinZoom, MinY_MaxZoom, zoom);
        var maxY = Mathf.Lerp(MaxY_MinZoom, MaxY_MaxZoom, zoom);

        var distance = /*Mathf.Lerp(MinZoom, MaxZoom, zoom) * damping **/Speed * Time.deltaTime;

        Vector3 position = camera.transform.localPosition;
        position += direction * distance;
      
        var x = Mathf.Clamp(position.x, minX, maxX);
        var y = Mathf.Clamp(position.y, minY, maxY);

        camera.transform.localPosition = new Vector3(x, y, z);
    }
}
