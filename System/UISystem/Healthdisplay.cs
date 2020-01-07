using System.Collections;
using System.Collections.Generic;
using Base2D.System.UISystem.Value;
using Base2D.System.UnitSystem.Units;
using UnityEngine;
using UnityEngine.UI;

public class Healthdisplay : MonoBehaviour
{
    public Hero character;
    private UIValue ui;
    public Image heart;
    public Image heartEmpty;
    
    // Start is called before the first frame update
    void Start()
    {
        ui.Init(character);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void drawHeart() {

    }
}