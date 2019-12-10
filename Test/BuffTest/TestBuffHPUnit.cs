using Base2D.System.UnitSystem;
using Base2D.System.BuffSystem;
using Base2D.System.BuffSystem.ScriptableObject;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Base2D.System.UnitSystem.Units;

class TestBuffHPUnit : MonoBehaviour
{
    private Unit unit1;
    private Unit unit2;
    public float seconds;


    private void Start()
    {
        GameObject go1 = new GameObject();
        GameObject go2 = new GameObject();
        go1.AddComponent(typeof(Unit));
        go2.AddComponent(typeof(Unit));
        unit1 = go1.GetComponent<Unit>();
        unit2 = go2.GetComponent<Unit>();


        //Generate base Value
        ScriptableHPBuff hpBuff = new ScriptableHPBuff();
        hpBuff.HPIncrease = 10;
        unit1.Attribute.MaxHp = 30;
        unit2.Attribute.MaxHp = 50;

        //add Component
        TimedHPBuff buff1 = new TimedHPBuff(10f, hpBuff, go1);
        go1.AddComponent(typeof(Base2D.System.BuffSystem.BuffableEntity));
        go2.AddComponent(typeof(Base2D.System.BuffSystem.BuffableEntity));
        go1.GetComponent<Base2D.System.BuffSystem.BuffableEntity>().AddBuff(buff1);

        go1.GetComponent<Base2D.System.BuffSystem.BuffableEntity>().UpdateTime = 1f;
        go2.GetComponent<Base2D.System.BuffSystem.BuffableEntity>().UpdateTime = 2f;

        go1.GetComponent<Base2D.System.BuffSystem.BuffableEntity>().AddBuff(buff1);
        go1.GetComponent<Base2D.System.BuffSystem.BuffableEntity>().AddBuff(new TimedHPBuff(15f, hpBuff, go2));

        
        StartCoroutine(LogBySeconds());
        Destroy(this, 20f);
    }

    private void Update()
    {
    }




    IEnumerator LogBySeconds()
    {
        while (true)
        {
            yield return new WaitForSeconds(seconds);
            Debug.Log(unit1.Attribute.MaxHp);
            Debug.Log(unit2.Attribute.MaxHp);
        }
    }

}

