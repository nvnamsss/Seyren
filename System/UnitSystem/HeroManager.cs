using System.Collections;
using System.Collections.Generic;
using Base2D.System.UnitSystem.Units;
using UnityEngine;

public class HeroManager : MonoBehaviour
{
    public static HeroManager instance;

    public Hero character;

    public void Awake(){
        if(instance == null)
            instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
