using Crom.Init.DamageModification;
using Crom.System.DamageSystem;
using Crom.System.DamageSystem.Critical;
using Crom.System.DamageSystem.Evasion;
using Crom.System.UnitSystem.EventData;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

namespace Crom.System.UnitSystem
{
    public class Unit : MonoBehaviour, IUnit, IObject, IAttribute
    {
        public delegate void DyingHandler(Unit sender, UnitDyingEventArgs e);
        public delegate void DiedHandler(Unit sender, UnitDiedEventArgs e);
        public delegate void StateChangedHandler(Unit sender, StateChangedEventArgs e);
        public delegate void TakeDamageHandler(Unit sender, TakeDamageEventArgs e);
        public event StateChangedHandler StateChanged;
        public event DyingHandler Dying;
        public event DiedHandler Died;
        public event TakeDamageHandler TakeDamage;
        public int CustomValue { get; set; }
        public bool Targetable { get; set; }
        public bool Invulnerable { get; set; }
        public float Size { get; set; }
        public float Height { get; set; }
        public float AnimationSpeed { get; set; }
        public float TurnSpeed { get; set; }
        public Color VertexColor { get; set; }
        public int DataType { get; set; }
        public float Strength { get; set; }
        public float Agility { get; set; }
        public float Intelligent { get; set; }
        public float AttackDamage { get; set; }
        public float MDamageAmplified { get; set; }
        public float MaxHp { get; set; }
        public float MaxMp { get; set; }
        public float CurrentHp { get; set; }
        public float CurrentMp { get; set; }
        public float HpRegen { get; set; }
        public float MpRegen { get; set; }
        public float HpRegenPercent { get; set; }
        public float MpRegenPercent { get; set; }
        public float Armor { get; set; }
        public float MArmor { get; set; }
        public float Shield { get; set; }
        public float MagicShield { get; set; }
        public float PhysicalShield { get; set; }
        public float LifeSteal { get; set; }
        public float MagicLifeSteal { get; set; }
        public float AttackSpeed { get; set; }
        public float CDReduction { get; set; }
        public float AttackRange { get; set; }
        public float CastRange { get; set; }

        Unit IUnit.Owner { get; set; }
        public ModificationInfos Modification { get; set; }
        public IAttachable Attach { get; set; }

        public Unit()
        {
            Modification = new ModificationInfos();
            AttackDamage = 51;
            CurrentHp = MaxHp = 100;
            PhysicalShield = 52;
            Shield = 22;
            Modification.Critical.AddModification(Critical.CriticalStrike());
        }

        void Start()
        {
            UnityEngine.Debug.Log("Mana regen: " + MpRegen);
        }

        void Update()
        {
        }

        public static GameObject CreateUnit()
        {
            GameObject go = new GameObject();
            SpriteRenderer render = go.AddComponent(typeof(SpriteRenderer)) as SpriteRenderer;
            go.AddComponent(typeof(Unit));
            Texture2D texture = new Texture2D(512, 256);
            byte[] data = File.ReadAllBytes(Path.Combine(Application.dataPath, "Knight Files", "Knight PNG", "Knight_attack_01.png"));
            texture.LoadImage(data);
            render.sprite = Sprite.Create(texture, new Rect(new Vector2(0, 0), new Vector2(512, 256)), new Vector2(0, 0));
            return go;
        }

        public void Damage(Unit target, DamageType type)
        {
            DamageInfo damageInfo = new DamageInfo(this, target);

            damageInfo.TriggerType = TriggerType.All;
            damageInfo.PrePassive = Modification.PrePassive;
            damageInfo.Critical = Modification.Critical;
            damageInfo.Evasion = target.Modification.Evasion;
            damageInfo.Reduction = target.Modification.Reduction;
            damageInfo.PostPassive = Modification.PostPassive;

            damageInfo.DamageAmount = AttackDamage;
            damageInfo.DamageType = type;
            damageInfo.CalculateDamage();
            /* Damage pipeline:
             * calculate raw damage target will receive
               apply effect, buff, modification, etc to raw damage
               apply damage to target
               checking shield
               steal life
             */

            UnityEngine.Debug.Log(damageInfo);
            if (Shield > 0 || PhysicalShield > 0 || MagicShield > 0)
            {
                if (damageInfo.DamageType == DamageType.Physical)
                {
                    float min = Mathf.Min(PhysicalShield, damageInfo.DamageAmount);
                    PhysicalShield -= min;
                    damageInfo.DamageAmount -= min;

                    UnityEngine.Debug.Log("Physical shield prevent: " + min);
                }

                if (damageInfo.DamageType == DamageType.Magical)
                {
                    float min = Mathf.Min(MagicShield, damageInfo.DamageAmount);
                    MagicShield -= min;
                    damageInfo.DamageAmount -= min;
                }

                if (damageInfo.DamageAmount > 0)
                {
                    float min = Mathf.Min(Shield, damageInfo.DamageAmount);
                    Shield -= min;
                    damageInfo.DamageAmount -= min;

                    UnityEngine.Debug.Log("Shield prevent: " + min);

                }
            }

            target.CurrentHp = target.CurrentHp - damageInfo.DamageAmount;
            TakeDamage?.Invoke(this, new TakeDamageEventArgs(damageInfo));
        }

        public void Damage(Unit target, float damage, DamageType type)
        {

        }

        public void Damage(Unit target, DamageInfo damageInfo)
        {
            
        }
    }

}
       