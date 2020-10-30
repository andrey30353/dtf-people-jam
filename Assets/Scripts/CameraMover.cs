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
    public float SpeedZoom = 100f;

    public float Threshold = 0.2f;

    public Liver2D folowLiver;

    Camera camera;

    float z;

    private void Start()
    {
        camera = Camera.main;

        z = camera.transform.localPosition.z;

        SetStartPosition();
    }

    public void SetStartPosition()
    {
        //print("SetStartPosition");
        //float distance = Mathf.Lerp(MaxZoom, MinZoom, zoom);
        camera.orthographicSize = MinZoom;
        camera.transform.localPosition = new Vector3(-4, -4, z);
    }

    // Update is called once per frame
    void Update()
    {


        float zoomDelta = Input.GetAxis("Mouse ScrollWheel");
        if (zoomDelta != 0f)
        {
            AdjustZoom(zoomDelta);


            //  AdjustPosition(camera.ScreenToWorldPoint(Input.mousePosition));
        }





        float xDelta = 0f;
        float yDelta = 0f;

        /*
        if (Input.mousePosition.x < Screen.width * Threshold) 
            xDelta = -1;
       
        if (Input.mousePosition.x > Screen.width - Screen.width * Threshold)        
            xDelta = 1;
       
        if (Input.mousePosition.y < Screen.height * Threshold)
                yDelta = -1;
        
        if (Input.mousePosition.y > Screen.height - Screen.height * Threshold)
                yDelta = 1;
        */




        if (folowLiver != null)
        {
            var dir = (folowLiver.transform.position - camera.transform.position).normalized;
            xDelta = dir.x;
            yDelta = dir.y;
        }
        else
        {
            xDelta = Input.GetAxis("Horizontal");
            yDelta = Input.GetAxis("Vertical");
        }

        if (xDelta != 0f || yDelta != 0f)
        {
            AdjustPosition(xDelta, yDelta);
        }



    }

    void AdjustZoom(float delta)
    {
        delta *= SpeedZoom * Time.deltaTime;
        zoom = Mathf.Clamp01(zoom + delta);
               
        float distance = Mathf.Lerp(MaxZoom, MinZoom, zoom);
        camera.orthographicSize = distance;
    }

    public void SetZoom(float zoom)
    {
        //delta *= SpeedZoom * Time.deltaTime;
        zoom = Mathf.Clamp01(zoom /*+ delta*/);

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

        var speed = Mathf.Lerp(MinZoom, MaxZoom, zoom);
        var distance = speed * damping * Time.deltaTime;

        Vector3 position = camera.transform.localPosition;
        position += direction * distance;

        var x = Mathf.Clamp(position.x, minX, maxX);
        var y = Mathf.Clamp(position.y, minY, maxY);

        camera.transform.localPosition = new Vector3(x, y, z);
    }

    void AdjustPosition(Vector2 position)
    {
        //Vector3 direction = new Vector2(xDelta, yDelta).normalized;
        //float damping = Mathf.Max(Mathf.Abs(xDelta), Mathf.Abs(yDelta));

        var minX = Mathf.Lerp(MinX_MinZoom, MinX_MaxZoom, zoom);
        var maxX = Mathf.Lerp(MaxX_MinZoom, MaxX_MaxZoom, zoom);

        var minY = Mathf.Lerp(MinY_MinZoom, MinY_MaxZoom, zoom);
        var maxY = Mathf.Lerp(MaxY_MinZoom, MaxY_MaxZoom, zoom);

        //var distance = /*Mathf.Lerp(MinZoom, MaxZoom, zoom) * damping **/Speed * Time.deltaTime;

        //Vector3 position = camera.transform.localPosition;
        //position += direction * distance;

        var x = Mathf.Clamp(position.x, minX, maxX);
        var y = Mathf.Clamp(position.y, minY, maxY);

        camera.transform.localPosition = new Vector3(x, y, z);
    }

    //void AdjustPosition2(Vector2 tagetPosition)
    //{
    //    var xDelta = camera.transform.position.x - tagetPosition.x;
    //    var yDelta = camera.transform.position.y - tagetPosition.y;

    //    AdjustPosition(xDelta, yDelta);

    //    Vector3 direction = new Vector2(xDelta, yDelta).normalized;
    //    float damping = Mathf.Max(Mathf.Abs(xDelta), Mathf.Abs(yDelta));

    //    var minX = Mathf.Lerp(MinX_MinZoom, MinX_MaxZoom, zoom);
    //    var maxX = Mathf.Lerp(MaxX_MinZoom, MaxX_MaxZoom, zoom);

    //    var minY = Mathf.Lerp(MinY_MinZoom, MinY_MaxZoom, zoom);
    //    var maxY = Mathf.Lerp(MaxY_MinZoom, MaxY_MaxZoom, zoom);

    //    var distance = /*Mathf.Lerp(MinZoom, MaxZoom, zoom) * damping **/Speed * Time.deltaTime;

    //    Vector3 position = camera.transform.localPosition;
    //    position += direction * distance;

    //    var x = Mathf.Clamp(position.x, minX, maxX);
    //    var y = Mathf.Clamp(position.y, minY, maxY);

    //    camera.transform.localPosition = new Vector3(x, y, z);
    //}
}
