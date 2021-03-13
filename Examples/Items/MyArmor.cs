using System.Collections;
using System.Collections.Generic;
using Seyren.Examples.Items;
using Seyren.System.Items;
using Seyren.System.Units;
using UnityEngine;

namespace Seyren.Examples.Items
{
    public class MyArmor : MyItem, IEquipable
{
    public int armorWidth;
    public int armorHeight;
    public string armorName;
    public string armorTooltip;
    public float defense;

    public Unit EquipBy => equipBy;
    private Unit equipBy;
    public bool Equip(Unit by)
    {
        // by.Attribute.Defense += defense;
        equipBy = by;
        // working on animation
        return true;
    }

    public bool UnEquip()
    {
        equipBy = null;
        return true;
    }

    protected override int height()
    {
        return armorHeight;
    }

    protected override string itemName()
    {
        return armorName;
    }

    protected override int stack()
    {
        return 1;
    }

    protected override string tooltip()
    {
        return armorTooltip;
    }

    protected override int width()
    {
        return armorWidth;
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

}
