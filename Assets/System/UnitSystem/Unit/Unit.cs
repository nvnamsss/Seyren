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
    public class Unit : MonoBehaviour, IUnit
    {
        public delegate void DyingHandler(Unit sender, UnitDyingEventArgs e);
        public delegate void DiedHandler(Unit sender, UnitDiedEventArgs e);
        public delegate void TakeDamageHandler(Unit sender, TakeDamageEventArgs e);
        public delegate void StateChangedHandler(Unit sender, StateChangedEventArgs e);
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
        public IUnit Owner { get; set; }
        public ModificationInfos Modification { get; set; }
        public IAttachable Attach { get; set; }

        public Attribute Attribute { get; set; }
        public bool IsFly { get; set; }
        public float TimeScale { get; set; }
        public float CurrentHp
        {
            get
            {
                return _currentHp;
            }
            set
            {
                StateChangedHandler state = StateChanged;
                StateChangedEventArgs sce = new StateChangedEventArgs(UnitState.Hp, _currentHp, value);
                if (state != null)
                {
                    state.Invoke(this, sce);
                }

                _currentHp = sce.NewValue;
            }
        }
        public float CurrentMp
        {
            get
            {
                return _currentMp;
            }
            set
            {
                StateChangedHandler state = StateChanged;
                StateChangedEventArgs sce = new StateChangedEventArgs(UnitState.Mp, _currentMp, value);
                if (state != null)
                {
                    state.Invoke(this, sce);
                }

                _currentMp = sce.NewValue;
            }
        }

        private float _currentHp;
        private float _currentMp;
        
        public Unit()
        {
            Attribute = new Attribute();
            Attribute.AttackDamage = 51;
            CurrentHp = Attribute.MaxHp = 100;
            Attribute.HpRegen = 1;
            Attribute.PhysicalShield = 52;
            Attribute.Shield = 22;
            Modification = new ModificationInfos();
            Modification.Critical.AddModification(Critical.CriticalStrike());
        }


        void Start()
        {
            UnityEngine.Debug.Log("Mana regen: " + Attribute.MpRegen);
        }

        void Update()
        {
            Attribute.Update();
        }

        public static GameObject CreateUnit()
        {
            GameObject go = new GameObject();
            SpriteRenderer render = go.AddComponent(typeof(SpriteRenderer)) as SpriteRenderer;
            Texture2D texture = new Texture2D(512, 256);
            byte[] data = File.ReadAllBytes(Path.Combine(Application.dataPath, "Knight Files", "Knight PNG", "Knight_attack_01.png"));

            go.AddComponent(typeof(Unit));
            texture.LoadImage(data);
            render.sprite = Sprite.Create(texture, new Rect(new Vector2(0, 0), new Vector2(512, 256)), new Vector2(0, 0));
            return go;
        }

        /// <summary>
        /// Order this unit to deal damage to target
        /// </summary>
        /// <param name="target"></param>
        /// <param name="type"></param>
        public void Damage(IUnit target, DamageType type)
        {
            DamageInfo damageInfo = new DamageInfo(this, target);

            damageInfo.TriggerType = TriggerType.All;
            damageInfo.PrePassive = Modification.PrePassive;
            damageInfo.Critical = Modification.Critical;
            damageInfo.Evasion = target.Modification.Evasion;
            damageInfo.Reduction = target.Modification.Reduction;
            damageInfo.PostPassive = Modification.PostPassive;

            damageInfo.DamageAmount = Attribute.AttackDamage;
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
            if (Attribute.Shield > 0 || Attribute.PhysicalShield > 0 || Attribute.MagicShield > 0)
            {
                //float min = 0;
                //switch (damageInfo.DamageType)
                //{
                //    case DamageType.Physical:
                //        min = Mathf.Min(Attribute.PhysicalShield, damageInfo.DamageAmount);
                //        Attribute.PhysicalShield -= min;
                //        damageInfo.DamageAmount -= min;
                //        break;
                //    case DamageType.Magical:
                //        min = Mathf.Min(Attribute.MagicShield, damageInfo.DamageAmount);
                //        Attribute.MagicShield -= min;
                //        damageInfo.DamageAmount -= min;
                //        break;
                //    case DamageType.Pure:
                //        min = Mathf.Min(Attribute.Shield, damageInfo.DamageAmount);
                //        Attribute.Shield -= min;
                //        damageInfo.DamageAmount -= min;
                //        break;
                //    case DamageType.OverTime:
                //        break;
                //    default:
                //        break;
                //}

                if (damageInfo.DamageType == DamageType.Physical)
                {
                    float min = Mathf.Min(Attribute.PhysicalShield, damageInfo.DamageAmount);
                    Attribute.PhysicalShield -= min;
                    damageInfo.DamageAmount -= min;

                    UnityEngine.Debug.Log("Physical shield prevent: " + min);
                }

                if (damageInfo.DamageType == DamageType.Magical)
                {
                    float min = Mathf.Min(Attribute.MagicShield, damageInfo.DamageAmount);
                    Attribute.MagicShield -= min;
                    damageInfo.DamageAmount -= min;
                }

                if (damageInfo.DamageAmount > 0)
                {
                    float min = Mathf.Min(Attribute.Shield, damageInfo.DamageAmount);
                    Attribute.Shield -= min;
                    damageInfo.DamageAmount -= min;

                    UnityEngine.Debug.Log("Shield prevent: " + min);

                }
            }

            target.Attribute.CurrentHp = target.Attribute.CurrentHp - damageInfo.DamageAmount;
            TakeDamage?.Invoke(this, new TakeDamageEventArgs(damageInfo));

        }

        public void Damage(IUnit target, float damage, DamageType type)
        {

        }

        public void Damage(IUnit target, DamageInfo damageInfo)
        {
            
        }

        /// <summary>
        /// Kill this unit
        /// </summary>
        /// <param name="killer"></param>
        public void Kill(Unit killer)
        {
            DyingHandler dying = Dying;
            UnitDyingEventArgs udinge = new UnitDyingEventArgs();
            if (dying != null)
            {
                dying.Invoke(this, udinge);
            }

            if (udinge.Cancel)
            {
                return;
            }

            DiedHandler died = Died;

            UnitDiedEventArgs udede = new UnitDiedEventArgs(killer);
            if (died != null)
            {
                died.Invoke(this, udede);
            }
        }

    }

}
       