using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCameraControl : AbstractCamera
{
    // Start is called before the first frame update
     public Transform target;
     public float xSpeed = 5.0f;
     public float ySpeed = 5.0f;
     public float yMinLimit = -90f;
     public float yMaxLimit = 90f;
     public float smoothTime = 10;
     public float zoomValue = 1f;
     float rotationYAxis = 0.0f;
     float rotationXAxis = 0.0f;
     float velocityX = 0.0f;
     float velocityY = 0.0f;
     float currentZoom = 0f;

     float distanceOffset;
     // Use this for initialization
     void Start()
     {
         Debug.Log("startCamera");
         Vector3 angles = transform.eulerAngles;
         rotationYAxis = angles.y;
         rotationXAxis = angles.x;

         if(target == null){
         
            GameObject gObject = GameObject.FindWithTag("Player");
            if(gObject != null){
                Debug.Log("found player");
                target = gObject.transform;
            }
         }
         if(target == null){
             return;
         }
         transform.position = target.position;
     }
     void Update(){
        
     }
     void LateUpdate()
     {
         if (target)
         {
             if(Input.GetMouseButton(1)){
                 currentZoom = Mathf.MoveTowards(currentZoom, zoomValue, Time.deltaTime/0.2f);
             }else{
                currentZoom = Mathf.MoveTowards(currentZoom, 0f, Time.deltaTime/0.2f);
             }
            //  if (Input.GetMouseButton(0))
            //  {
                 velocityX += xSpeed * Input.GetAxis("Mouse X") * Time.deltaTime;
                 velocityY += ySpeed * Input.GetAxis("Mouse Y") * Time.deltaTime;
            //  }
             rotationYAxis += velocityX;
             rotationXAxis -= velocityY;
             rotationXAxis = ClampAngle(rotationXAxis, yMinLimit, yMaxLimit);

             //Quaternion fromRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
             Quaternion toRotation = Quaternion.Euler(rotationXAxis, rotationYAxis, 0);
             Quaternion rotation = toRotation;
 
             Vector3 negDistance = new Vector3(0.0f, 0.0f, currentZoom);
             Vector3 position = rotation * negDistance + target.position;

            target.transform.Rotate(Vector3.up * velocityX);
            
            transform.position = position;
             transform.rotation = rotation;
            
             velocityX = Mathf.Lerp(velocityX, 0, Time.deltaTime * smoothTime);
             velocityY = Mathf.Lerp(velocityY, 0, Time.deltaTime * smoothTime);
             
             //Debug.Log(Time.deltaTime * smoothTime);
            // velocityX = 0f;
            // velocityY = 0f;
            //transform.LookAt(target);
         }
     }
     public static float ClampAngle(float angle, float min, float max)
     {
         if (angle < -360F)
             angle += 360F;
         if (angle > 360F)
             angle -= 360F;
         return Mathf.Clamp(angle, min, max);
     }
     public override void CameraController(){

     }
 }
