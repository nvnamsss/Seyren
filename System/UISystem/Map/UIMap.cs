using System.Collections.Generic;
using UnityEngine;

namespace Base2D.System.UISystem.Map
{
    public class UIMap: MonoBehaviour
    {
        public List<MapSegment> segment;

        void Start(){

        }
        public void addSegment(MapSegment s)
        {
            s.isActive = true;
            segment.Add(s);
        }

        public void updateMap()
        {
            foreach(MapSegment i in segment) {
                if (i.isActive == true)
                {
                    FindInActiveObjectByName(i.mapName).SetActive(true);
                }
            }
        }
        GameObject FindInActiveObjectByName(string name)
{
    Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
    for (int i = 0; i < objs.Length; i++)
    {
        if (objs[i].hideFlags == HideFlags.None)
        {
            if (objs[i].name == name)
            {
                return objs[i].gameObject;
            }
        }
    }
    return null;
}
    }
    
}