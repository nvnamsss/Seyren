using Base2D.System.UnitSystem;
using Base2D.System.DamageSystem;
using UnityEngine;

namespace Base2D.System.ItemSystem
{
    public partial class Item : MonoBehaviour, IAttribute
    {
        #if UNITY_EDITOR
        [Header("Attack Settings")]
        public float Strength;
        public float Agility;
        public float Intelligent;
        public float AttackDamage;
        public float MDamageAmplified;
        [Header("State Settings")]
        public float MaxHp;
        public float MaxMp;
        public float HpRegen;
        public float MpRegen;
        public float ShieldRegen;
        public float MShieldRegen;
        public float PShield;
        public float HpRegenPercent;
        public float MpRegenPercent;
        public float Armor;
        public float MArmor;
        [Header("Range Settings")]
        public float AttackRange;
        public float CastRange;
        [Header("Speed Settings")]
        public float MovementSpeed;
        public float AttackSpeed;
        public float JumpSpeed;
#endif
    }
}
