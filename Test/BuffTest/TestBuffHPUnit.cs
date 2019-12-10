using Base2D.System.BuffSystem;
using Base2D.System.BuffSystem.ScriptableObject;
using Base2D.System.UnitSystem.Units;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
        go1.AddComponent(typeof(BuffableEntity));
        go2.AddComponent(typeof(BuffableEntity));
        go1.GetComponent<BuffableEntity>().AddBuff(buff1);

        go1.GetComponent<BuffableEntity>().UpdateTime = 1f;
        go2.GetComponent<BuffableEntity>().UpdateTime = 2f;

        go1.GetComponent<BuffableEntity>().AddBuff(buff1);
        go1.GetComponent<BuffableEntity>().AddBuff(new TimedHPBuff(15f, hpBuff, go2));

        
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

