using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Seyren.Examples.Items
{
    public class MyWeapon : MyItem
{
    public int damage = 1;

    public int weaponWidth;
    public int weaponHeight;
    public string weaponName;
    public string weaponTooltip;


    protected override int height()
    {
        return weaponHeight;
    }

    protected override string itemName()
    {
        return weaponName;
    }

    protected override int stack()
    {
        return 1;
    }

    protected override string tooltip()
    {
        return weaponTooltip;
    }

    protected override int width()
    {
        return weaponWidth;
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
