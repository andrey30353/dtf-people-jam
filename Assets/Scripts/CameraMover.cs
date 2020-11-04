using UnityEngine;

public class CameraMover : MonoBehaviour
{
    public bool SetPositionOnStart;

    public float MinZoom, MaxZoom;
       
    public float MinX_MinZoom, MaxX_MinZoom;
    public float MinX_MaxZoom, MaxX_MaxZoom;

    public float MinY_MinZoom, MaxY_MinZoom;
    public float MinY_MaxZoom, MaxY_MaxZoom;
      
    public float MoveSpeed = 20f;

   /* public float MoveSpeedOnMinZoom = 10f;
    public float MoveSpeedOnMaxZoom = 20f;*/

    public float ZoomSpeed = 100f;
          
    public Liver2D FolowLiver;

    public float Zoom => zoom;
    public Vector2 CameraPosition => cameraMain.transform.position;

    private Camera cameraMain;

    private float zPosition;

    private float zoom;  

    private void Start()
    {
        cameraMain = Camera.main;

        zPosition = cameraMain.transform.localPosition.z;

        if(SetPositionOnStart)
            SetStartPosition();
    }

    public void SetStartPosition()
    {
        //print("SetStartPosition");
        //float distance = Mathf.Lerp(MaxZoom, MinZoom, zoom);
        cameraMain.orthographicSize = MinZoom;
        cameraMain.transform.localPosition = new Vector3(-4, -4, zPosition);
        zoom = 1;
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




        if (FolowLiver != null)
        {
            var dir = (FolowLiver.transform.position - cameraMain.transform.position).normalized;
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
        delta *= ZoomSpeed * Time.deltaTime;
        zoom = Mathf.Clamp01(zoom + delta);
               
        float distance = Mathf.Lerp(MaxZoom, MinZoom, zoom);
        cameraMain.orthographicSize = distance;
    }

    public void SetZoom(float zoom)
    {
        //delta *= SpeedZoom * Time.deltaTime;
        zoom = Mathf.Clamp01(zoom /*+ delta*/);

        float distance = Mathf.Lerp(MaxZoom, MinZoom, zoom);
        cameraMain.orthographicSize = distance;
    }


    void AdjustPosition(float xDelta, float yDelta)
    {
        Vector3 direction = new Vector2(xDelta, yDelta).normalized;
        float damping = Mathf.Max(Mathf.Abs(xDelta), Mathf.Abs(yDelta));

        var minX = Mathf.Lerp(MinX_MinZoom, MinX_MaxZoom, zoom);
        var maxX = Mathf.Lerp(MaxX_MinZoom, MaxX_MaxZoom, zoom);

        var minY = Mathf.Lerp(MinY_MinZoom, MinY_MaxZoom, zoom);
        var maxY = Mathf.Lerp(MaxY_MinZoom, MaxY_MaxZoom, zoom);

        //var speed = Mathf.Lerp(MoveSpeedOnMinZoom, MoveSpeedOnMaxZoom, zoom);
        var distance = MoveSpeed * damping * Time.deltaTime;

        Vector3 position = cameraMain.transform.localPosition;
        position += direction * distance;

        var x = Mathf.Clamp(position.x, minX, maxX);
        var y = Mathf.Clamp(position.y, minY, maxY);

        cameraMain.transform.localPosition = new Vector3(x, y, zPosition);
    }
}
