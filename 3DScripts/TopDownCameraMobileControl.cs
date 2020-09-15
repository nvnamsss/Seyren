using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCameraMobileControl : AbstractCamera
{
    public float initX = 0f;
    public float initZ = 0f;
    public float maxHeight = 5f;
    public float minHeight = 1f;
    public float initHeight = 5f;
    public float zoomStep = 1f;
    public float zoomSpeed = 0.5f;
    public float initAngle = 45f;
    public float speed = 5f;
    public float edgeValue = 0.95f; //bigger mean closer to the edge
    public float topBound = 5f;
    public float bottomBound = -5f;
    public float rightBound = 5f;
    public float leftBound = -5f;
    public bool inverted = true;
    
    float currentHeight;
    float currentAngle;
    Vector3 touchStart;
    Camera cam;

    // Start is called before the first frame update
    
    void Start()
    {
        currentAngle = initAngle;
        if(maxHeight< minHeight){
            maxHeight = minHeight;
        }
        if(initHeight < minHeight){
            initHeight = minHeight;
        }
        if(initHeight > maxHeight){
            initHeight = maxHeight;
        }
        currentHeight = initHeight;
        transform.position = new Vector3(initX,currentHeight,initZ);
        Quaternion rotation = Quaternion.AngleAxis(initAngle,Vector3.right);
        transform.rotation = rotation;
        Cursor.lockState = CursorLockMode.Confined;
        if(gameObject.GetComponent<Camera>() != null){
            cam = gameObject.GetComponent<Camera>();
        }
    }

    // Update is called once per frame

    private void Update()
    {
        CameraController();
    }

    public override void CameraController()
    {
        if (Input.GetMouseButtonDown(0))
        {
            touchStart = Input.mousePosition;
            return;
        }
        if (Input.GetMouseButton(0))
        {
            if(cam != null){
            Vector3 pos = cam.ScreenToViewportPoint(Input.mousePosition - touchStart);
            Vector3 move = new Vector3(pos.x * speed * Time.deltaTime, 0, pos.y * speed * Time.deltaTime);

            if (inverted)
            {
                move = -move;
            }

            transform.Translate(move, Space.World);
            }
        }
    }

    void LateUpdate()
    {
        // if(transform.position.y > currentHeight || transform.position.y < currentHeight){
        //     float height = Mathf.MoveTowards(transform.position.y, currentHeight, Time.deltaTime/zoomSpeed);
        //     Vector3 pos = new Vector3(transform.position.x, height, transform.position.z);
        //     transform.position = pos;
        // }
        // if(transform.eulerAngles.x > currentAngle || transform.eulerAngles.x < currentAngle){
        //     Debug.Log(transform.eulerAngles.x);
        //     float rot = Mathf.MoveTowards(transform.eulerAngles.x, currentAngle, 10f * Time.deltaTime / zoomSpeed);
        //     Quaternion q = Quaternion.Euler(rot, transform.eulerAngles.y, transform.eulerAngles.z);
        //     transform.rotation = q;
        // }
        if(transform.position.z > topBound){
            Vector3 pos = new Vector3(transform.position.x,transform.position.y, topBound);
            transform.position = pos;
        }
        if(transform.position.z < bottomBound){
            Vector3 pos = new Vector3(transform.position.x,transform.position.y, bottomBound);
            transform.position = pos;
        }
        if(transform.position.x > rightBound){
            Vector3 pos = new Vector3(rightBound,transform.position.y, transform.position.z);
            transform.position = pos;
        }
        if(transform.position.x < leftBound){
            Vector3 pos = new Vector3(leftBound,transform.position.y, transform.position.z);
            transform.position = pos;
        }
        // if(Input.mousePosition.y >= Screen.height*0.95 && transform.position.z < topBound)
        // {
        //     transform.Translate(Vector3.forward * Time.deltaTime * speed, Space.World);
        // }
        // if(Input.mousePosition.y <= (Screen.height - Screen.height * 0.95) && transform.position.z > bottomBound)
        // {
        //     transform.Translate(-(Vector3.forward * Time.deltaTime * speed), Space.World);
        // }
        // if(Input.mousePosition.x >= Screen.width*0.95 && transform.position.x < rightBound)
        // {
        //     transform.Translate(Vector3.right * Time.deltaTime * speed, Space.World);
        // }
        // if(Input.mousePosition.x <= (Screen.width - Screen.width * 0.95) && transform.position.x > leftBound)
        // {
        //     transform.Translate(-(Vector3.right * Time.deltaTime * speed), Space.World);
        // }
        
    }
}
