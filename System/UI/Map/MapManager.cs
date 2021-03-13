using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Seyren.System.UI
{
    public class MapManager : MonoBehaviour
    {
        public static MapManager instance;

        void Awake()
        {
            if (instance == null)
                instance = this;
        }
        // Start is called before the first frame update

        public void setPlayerPosition(float newX, float newY, float newZ)
        {
            // HeroManager.instance.character.transform.localPosition = new Vector3(newX,newY,newZ);
        }

        public UIMap mapToManage;

        public void UpdateMap()
        {
            mapToManage.updateMap();
        }
    }
}

