using Crom.System.DamageSystem;
using Crom.System.UnitSystem;
using Crom.System.UnitSystem.Projectile;
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
        go1.AddComponent(typeof(Crom.System.UnitSystem.Unit));
        go2.AddComponent(typeof(Crom.System.UnitSystem.Unit));
        Crom.System.UnitSystem.Unit unit1 = go1.GetComponent<Crom.System.UnitSystem.Unit>();
        Crom.System.UnitSystem.Unit unit2 = go2.GetComponent<Crom.System.UnitSystem.Unit>();
        unit1.Damage(unit2, DamageType.Physical);
        //DamageStatus status = 0 | DamageStatus.Reduced | DamageStatus.Evade;
        //Debug.Log((int)status);
        //Debug.Log((status | DamageStatus.Reduced) == status);
        Unit.CreateUnit();
        string path = System.IO.Path.Combine(Application.dataPath, "Resources", "Knight_attack_01.png").Replace(@"\", "/");
        Debug.Log(path);
        ProjectileInterface.CreateProjectile("a", path);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
