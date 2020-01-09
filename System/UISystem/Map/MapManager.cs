using System.Collections;
using System.Collections.Generic;
using Base2D.System.UISystem.Map;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager instance;

    void Awake() {
        if(instance ==  null)
            instance = this;
    }
    // Start is called before the first frame update

    public UIMap mapToManage;

    public void UpdateMap(){
        mapToManage.updateMap();
    }
}
