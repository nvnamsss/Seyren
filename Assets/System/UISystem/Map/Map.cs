using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.System.UISystem.Map
{
    class Map : UIComponent
    {
        List<MapSegment> segment;

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
                    //draw map
                }
            }
        }
    }
}
