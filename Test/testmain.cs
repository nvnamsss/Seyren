using Base2D.System.DamageSystem;
using Base2D.System.UnitSystem;
using Base2D.System.UnitSystem.Projectiles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testmain : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject go1 = new GameObject();
        GameObject go2 = new GameObject();
        go1.AddComponent(typeof(Base2D.System.UnitSystem.Units.Unit));
        go2.AddComponent(typeof(Base2D.System.UnitSystem.Units.Unit));
        Base2D.System.UnitSystem.Units.Unit unit1 = go1.GetComponent<Base2D.System.UnitSystem.Units.Unit>();
        Base2D.System.UnitSystem.Units.Unit unit2 = go2.GetComponent<Base2D.System.UnitSystem.Units.Unit>();
        unit1.Damage(unit2, DamageType.Physical);
        //DamageStatus status = 0 | DamageStatus.Reduced | DamageStatus.Evade;
        //Debug.Log((int)status);
        //Debug.Log((status | DamageStatus.Reduced) == status);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
