using Seyren.System.Items;
using Seyren.System.Units;
using UnityEngine;

public class CellItem : MonoBehaviour {
    
}
namespace Seyren.Examples.Items {
    public abstract class Item : ICellItem
    {
        public int Width => width();

        public int Height => height();

        public string Name => name();

        public int MaxStack => stack();

        public string Tooltip => tooltip();

        // Image here

        protected abstract string name();
        protected abstract string tooltip();
        protected abstract int width();
        protected abstract int height();
        protected abstract int stack();
    }

    public abstract class Armor : Item, IEquipable
    {
        public float Defense => defense();

        public Unit EquipBy => throw new global::System.NotImplementedException();

        public bool Equip(Unit by)
        {
            return false;
        }

        protected abstract float defense();
        protected abstract bool equip(Unit by);
        protected override int width()
        {
            return 2;
        }

        protected override int height()
        {
            return 3;
        }

        protected override int stack()
        {
            return 1;
        }

        public bool UnEquip()
        {
            throw new global::System.NotImplementedException();
        }
    }

    public class DragonArmor : Armor
    {
        protected override float defense()
        {
            return 67.3f;
        }

        protected override bool equip(Unit by)
        {
            // by.Attribute.Defense += defense();
            return true;
        }

        protected override string name()
        {
            return "Dragon Armor";
        }

        protected override string tooltip()
        {
            throw new global::System.NotImplementedException();
        }
    }

    public class PlatinumArmor : Armor
    {
        protected override float defense()
        {
            return 24f;
        }

        protected override bool equip(Unit by)
        {
            // by.Attribute.Defense += defense();
            return true;
        }

        protected override string name()
        {
            return "Platinum Armor";
        }

        protected override string tooltip()
        {
            throw new global::System.NotImplementedException();
        }
    }
}