using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System;
using UnityEngine;

public class ComponentSelector : MonoBehaviour
{
    //public enum Strategy{Strategy0 = 0, Strategy1 = 1};
    //public Strategy stategy;
    System.Type currentCamera;
    System.Type[] possible;
    bool cameraIsSet=false;
    public int selected;
    void Start()
    {
        System.Type[] types = Assembly.GetExecutingAssembly().GetTypes();
        possible = (from System.Type type in types where type.IsSubclassOf(typeof(AbstractCamera)) select type).ToArray();
        if(possible.Length == 0){
            Debug.Log("no subclass");
            return;
        }
        setCamera(); 
    }

    // Update is called once per frame
    void Update()
    {
    //     if (cameras.Length == 0)
    //     {
    //         return;
    //     }
    //     if(currentCamera != null && currentCamera != cameras[(int)stategy]){
    //         Destroy(GetComponent(currentCamera.GetType()));
    //         currentCamera = cameras[(int)stategy];
    //         gameObject.AddComponent(currentCamera.GetType());
    //     }
     }

     public void setCamera(){
         if(!cameraIsSet){
        //Debug.Log("Set camera: "+possible[index]);
        currentCamera = possible[selected];
        gameObject.AddComponent(currentCamera);
        cameraIsSet = true;
         }
     }
}
