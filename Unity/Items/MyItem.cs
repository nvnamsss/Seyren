using Seyren.System.Items;
using Seyren.System.Units;
using Seyren.Examples.Items;
using UnityEngine;

namespace Seyren.Examples.Items
{
    public abstract class MyItem : MonoBehaviour, ICellItem
    {
        public int Width => width();

        public int Height => height();

        public string Name => itemName();

        public string Tooltip => tooltip();

        public int MaxStack => stack();

        protected abstract int width();
        protected abstract int height();
        protected abstract string itemName();
        protected abstract string tooltip();
        protected abstract int stack();

    }

}
