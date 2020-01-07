using System.Collections;
using System.Collections.Generic;
using Base2D.System.UnitSystem.Units;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    // Start is called before the first frame update

    public Hero character;
    public Image heart;
    public Image heartEmpty;

    void Start()
    {
        character.Attribute.MaxHp = 5;
        setHealth();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void setHealth(){
Debug.Log("setHealth");
        if(character.Attribute.MaxHp > 5){
            for (int i =6; i <= character.Attribute.MaxHp;i++){
                var newHeart = Instantiate(heart);
                var newEmptyHeart = Instantiate(heartEmpty);

                newHeart.name= "heart-full"+i;
                newEmptyHeart.name="heart-empty"+i;            

                newHeart.transform.parent = GameObject.Find("heart-full").transform;
                newEmptyHeart.transform.parent = GameObject.Find("heart-empty").transform;

                newHeart.rectTransform.anchoredPosition = new Vector2(-222f+(45*(i-1)), 157f);
                newEmptyHeart.rectTransform.anchoredPosition = new Vector2(-222f+(45*(i-1)), 157f);
            }
        }

        for(int i=1; i <= character.Attribute.MaxHp;i++){
            if(i > character.CurrentHp){
                FindInActiveObjectByName("heart-full"+i).SetActive(false);
            }
        }
    }

    void increaseMaxHealth(){
        var newHeart = Instantiate(heart);
        var newEmptyHeart = Instantiate(heartEmpty);

        newHeart.name= "heart-full"+character.Attribute.MaxHp;
        newEmptyHeart.name="heart-empty"+character.Attribute.MaxHp;            

        newHeart.transform.parent = GameObject.Find("heart-full").transform;
        newEmptyHeart.transform.parent = GameObject.Find("heart-empty").transform;

        newHeart.rectTransform.anchoredPosition = new Vector2(-222f+(45*(character.Attribute.MaxHp-1)), 157f);
        newEmptyHeart.rectTransform.anchoredPosition = new Vector2(-222f+(45*(character.Attribute.MaxHp-1)), 157f);

        for (int i = 1;i<= character.Attribute.MaxHp;i++){
            FindInActiveObjectByName("heart-full"+i).SetActive(true);
        }
    }

    void takeDamage(){
        GameObject.Find("heart-full"+(character.CurrentHp+1)).SetActive(false);
    }

    void heal(){
        for (int i = 1;i<= character.CurrentHp;i++){
            FindInActiveObjectByName("heart-full"+i).SetActive(true);
        }
    }

    GameObject FindInActiveObjectByName(string name)
{
    Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
    for (int i = 0; i < objs.Length; i++)
    {
        if (objs[i].hideFlags == HideFlags.None)
        {
            if (objs[i].name == name)
            {
                return objs[i].gameObject;
            }
        }
    }
    return null;
}
}