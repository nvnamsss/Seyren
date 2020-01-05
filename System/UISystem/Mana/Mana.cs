using System.Collections;
using System.Collections.Generic;
using Base2D.System.UnitSystem.Units;
using UnityEngine;
using UnityEngine.UI;

public class mana : MonoBehaviour
{
    // Start is called before the first frame update

    public Image manaImage;
    public Hero character;

    void Start()
    {
        character.Attribute.MaxMp = 100;
        setMana();
    }

    // Update is called once per frame
    void Update()
    {
        updateMana();
    }

    void setMana(){
        manaImage.fillAmount = character.CurrentMp/character.Attribute.MaxMp;
    }

    void updateMana(){
        manaImage.fillAmount = character.CurrentMp/character.Attribute.MaxMp;
    }
}
