// using System.Collections;
// using System.Collections.Generic;
// using Seyren.System.Units;
// using UnityEngine;
// using UnityEngine.UI;

// public class StatusUIManager : MonoBehaviour
// {
//     #region  Singleton
//     public static StatusUIManager instance;

//     private void Awake() {
//         instance = this;
//     }


//     #endregion

//     public Hero character;
//     public Button increaseStr;
//     public Button increaseAgi;
//     public Button increaseInt;
//     public Button increaseHp;
//     public Button increaseMp;

//     public Button extendStatsButton;

//     public Text strText;
//     public Text agiText;
//     public Text intText;
//     public Text hpText;
//     public Text mpText;

//     public GameObject extendContainer;

//     public Text atkText;
//     public Text defText;
//     public Text mDefText;
//     public Text mpRegenText;

//     void Start()
//     {

//     }

//     // Update is called once per frame
//     void Update()
//     {
//     }

//     public void setStats(){
//         Debug.Log((int)character.Attribute.Agility);
//         strText.text = ((int)character.Attribute.Strength).ToString();
//         agiText.text = ((int)character.Attribute.Agility).ToString();
//         intText.text = ((int)character.Attribute.Intelligent).ToString();
//         hpText.text = ((int)character.Attribute.MaxHp).ToString();
//         mpText.text = ((int)character.Attribute.MaxMp).ToString();
//         atkText.text = ((int)character.Attribute.AttackDamage).ToString();
//         defText.text = ((int)character.Attribute.Defense).ToString();
//         mDefText.text = ((int)character.Attribute.MArmor).ToString();
//         mpRegenText.text = (character.Attribute.MpRegen).ToString();
//     }

//     public void extendStats(){
//         extendContainer.SetActive(!extendContainer.activeSelf);
//         if(!extendContainer.activeSelf)
//             extendStatsButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(30, -90);
//         else
//             extendStatsButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(170, -90);
//     }

//     public void increaseAgiFunc(){
//         character.Attribute.Agility++;
//         agiText.text = ((int)character.Attribute.Agility).ToString();
//     }

//     public void increaseStrFunc(){
//         character.Attribute.Strength++;
//         strText.text = ((int)character.Attribute.Strength).ToString();
//     }

//     public void increaseIntFunc(){
//         character.Attribute.Intelligent++;
//         intText.text = ((int)character.Attribute.Intelligent).ToString();
//     }

//     public void increaseHpFunc(){
//         character.Attribute.MaxHp++;
//         hpText.text = ((int)character.Attribute.MaxHp).ToString();

//         HUDManager.instance.increaseMaxHealth();
//     }

//     public void increaseMpFunc(){
//         character.Attribute.MaxMp+=100;
//         mpText.text = ((int)character.Attribute.MaxMp).ToString();
//     }
// }
