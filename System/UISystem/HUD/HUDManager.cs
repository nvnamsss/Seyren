using System.Collections;
using System.Collections.Generic;
using Base2D.System.UISystem.Value;
using Base2D.System.UnitSystem.Units;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{

    public static HUDManager instance;

    void Awake() {
        if (instance == null){
            instance = this;
        }
    }

    Hero character;
    public Image heart;
    public Image heartEmpty;

    public Image manaImage;

    void Start()
    {  
        character = HeroManager.instance.character;       
    }

    // Update is called once per frame
    void setMana(){
        manaImage.fillAmount = character.CurrentMp/character.Attribute.MaxMp;
    }

    public void updateMana(){
        manaImage.fillAmount = character.CurrentMp/character.Attribute.MaxMp;
    }

    public void setHealth(){
Debug.Log("setHealth");
        if(character.Attribute.MaxHp > 5){
            for (int i =6; i <= character.Attribute.MaxHp;i++){
                var newHeart = Instantiate(heart);
                var newEmptyHeart = Instantiate(heartEmpty);

                newHeart.name= "heart-full"+i;
                newEmptyHeart.name="heart-empty"+i;            

                newHeart.transform.SetParent(GameObject.Find("heart-full").transform);
                newEmptyHeart.transform.SetParent(GameObject.Find("heart-empty").transform);

                newHeart.rectTransform.anchoredPosition = new Vector2(-222f+(45*(i-1)), 157f);
                newEmptyHeart.rectTransform.anchoredPosition = new Vector2(-222f+(45*(i-1)), 157f);
                newEmptyHeart.rectTransform.localScale = new Vector3(1,1,1);
                newHeart.rectTransform.localScale = new Vector3(1,1,1);
            }
        }

        for(int i=1; i <= character.Attribute.MaxHp;i++){
            if(i > character.CurrentHp){
                FindInActiveObjectByName("heart-full"+i).SetActive(false);
            }
        }
    }

    public void increaseMaxHealth(){
        character.CurrentHp = character.Attribute.MaxHp;
        var newHeart = Instantiate(heart);
        var newEmptyHeart = Instantiate(heartEmpty);

        newHeart.name= "heart-full"+character.Attribute.MaxHp;
        newEmptyHeart.name="heart-empty"+character.Attribute.MaxHp;            

        newHeart.transform.parent = GameObject.Find("heart-full").transform;
        newEmptyHeart.transform.parent = GameObject.Find("heart-empty").transform;

        newHeart.rectTransform.anchoredPosition = new Vector2(-222f+(45*(character.Attribute.MaxHp-1)), 157f);
        newEmptyHeart.rectTransform.anchoredPosition = new Vector2(-222f+(45*(character.Attribute.MaxHp-1)), 157f);
        newHeart.rectTransform.localScale = new Vector3(1,1,1);
        newEmptyHeart.rectTransform.anchoredPosition = new Vector2(-222f+(45*(character.Attribute.MaxHp-1)), 157f);
        newEmptyHeart.rectTransform.localScale = new Vector3(1,1,1);

        for (int i = 1;i<= character.Attribute.MaxHp;i++){
            FindInActiveObjectByName("heart-full"+i).SetActive(true);
        }
    }

    void takeDamage(){
        GameObject.Find("heart-full"+(character.CurrentHp+1)).SetActive(false);
    }

    public void heal(){
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