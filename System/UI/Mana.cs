﻿// using System.Collections;
// using System.Collections.Generic;
// using Seyren.System.Units;
// using UnityEngine;
// using UnityEngine.UI;

// public class Mana : MonoBehaviour
// {
//     // Start is called before the first frame update

//     public Image manaImage;
//     public Hero character;

//     void Start()
//     {
//         character.Attribute.MaxMp = 100;
//         setMana();
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         updateMana();
//     }

//     void setMana(){
//         manaImage.fillAmount = character.CurrentMp/character.Attribute.MaxMp;
//     }

//     void updateMana(){
//         manaImage.fillAmount = character.CurrentMp/character.Attribute.MaxMp;
//     }
// }