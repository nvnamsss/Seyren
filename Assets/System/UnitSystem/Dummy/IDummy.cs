    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crom.System.UnitSystem.Dummy
{
    public interface IDummy : IAttribute
    {
        float HitDelay { get; set; }
        float TimeExpired { get; set; }
        IUnit Owner { get; set; }
        void Move();
        void Hit();
    }
}
