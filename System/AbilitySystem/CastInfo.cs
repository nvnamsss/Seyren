using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Base2D.System.AbilitySystem
{
    public class CastInfo
    {
        public bool IgnoreCallback { get; }

        public static CastInfo CreateLightweightCast()
        {
            CastInfo info = new CastInfo();

            return info;
        }

        public static CastInfo CreateFullyCast()
        {
            CastInfo info = new CastInfo();

            return info;
        }
    }
}
