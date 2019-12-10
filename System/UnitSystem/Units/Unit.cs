using Base2D.Init.DamageModification;
using Base2D.System.DamageSystem;
using Base2D.System.DamageSystem.Critical;
using Base2D.System.DamageSystem.Evasion;
using Base2D.System.UnitSystem.EventData;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

namespace Base2D.System.UnitSystem.Units
{
    public class Unit : MonoBehaviour, IObject, IAttribute
    {
        public delegate void DyingHandler(Unit sender, UnitDyingEventArgs e);
        public delegate void DiedHandler(Unit sender, UnitDiedEventArgs e);
        public delegate void TakeDamageHandler(Unit sender, TakeDamageEventArgs e);
        public delegate void StateChangedHandler(Unit sender, StateChangedEventArgs e);
        public delegate void StatusChangedHandler(Unit sender, StatusChangedEventArgs e);
        public event StateChangedHandler StateChanged;
        public event StatusChangedHandler StatusChanged;
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
        public Unit Owner { get; set; }
        public ModificationInfos Modification { get; set; }
        public IAttachable Attach { get; set; }

        public Attribute Attribute { get; set; }
        public float TimeScale;
        public bool IsFly
        {
            get
            {
                return _isFly;
            }
            set
            {
                _isFly = value;
            }
        }
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

        public float CurrentShield
        {
            get
            {
                return _currentShield;
            }
            set
            {
                StateChangedHandler state = StateChanged;
                StateChangedEventArgs sce = new StateChangedEventArgs(UnitState.Shield, _currentShield, value);
                if (state != null)
                {
                    state.Invoke(this, sce);
                }

                _currentShield = sce.NewValue;
            }
        }

        public float CurrentMShield
        {
            get
            {
                return CurrentMShield;
            }
            set
            {
                StateChangedHandler state = StateChanged;
                StateChangedEventArgs sce = new StateChangedEventArgs(UnitState.MagicalShield, CurrentMShield, value);
                if (state != null)
                {
                    state.Invoke(this, sce);
                }

                CurrentMShield = sce.NewValue;
            }
        }
        public float CurrentPShield
        {
            get
            {
                return _currentPShield;
            }
            set
            {
                StateChangedHandler state = StateChanged;
                StateChangedEventArgs sce = new StateChangedEventArgs(UnitState.PhysicalShield, _currentPShield, value);
                if (state != null)
                {
                    state.Invoke(this, sce);
                }

                _currentPShield = sce.NewValue;
            }
        }
        protected Unit _owner;
        [SerializeField]
        protected bool _isFly;
        [SerializeField]
        protected float _currentShield;
        [SerializeField]
        protected float _currentMShield;
        [SerializeField]
        protected float _currentPShield;
        [SerializeField]
        protected float _currentHp;
        [SerializeField]
        protected float _currentMp;
        

        public Unit()
        {
            Attribute = new Attribute();
            Attribute.AttackDamage = 51;
            Attribute.HpRegen = 1;
            CurrentHp = Attribute.MaxHp = 100;
            CurrentPShield = 52;
            CurrentShield = 22;
            Modification = new ModificationInfos();
            Modification.Critical.AddModification(Critical.CriticalStrike());
        }


        void Start()
        {
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
        /// Order source unit to deal damage to this unit
        /// </summary>
        /// <param name="source"></param>
        /// <param name="type"></param>
        public void Damage(Unit source, DamageType type)
        {
            Damage(source, type, TriggerType.All);
        }

        public void Damage(Unit source, DamageType type, TriggerType trigger)
        {
            Damage(source, Attribute.AttackDamage, type, trigger);
        }

        public void Damage(Unit source, float damage, DamageType type, TriggerType trigger)
        {
            DamageInfo damageInfo = new DamageInfo(this, source);

            damageInfo.TriggerType = trigger;
            damageInfo.PrePassive = source.Modification.PrePassive;
            damageInfo.Critical = source.Modification.Critical;
            damageInfo.Evasion = Modification.Evasion;
            damageInfo.Reduction = Modification.Reduction;
            damageInfo.PostPassive = source.Modification.PostPassive;

            damageInfo.DamageAmount = damage;
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
            if (CurrentShield > 0 || CurrentPShield > 0 || CurrentMShield > 0)
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
                    float min = Mathf.Min(CurrentPShield, damageInfo.DamageAmount);
                    CurrentPShield -= min;
                    damageInfo.DamageAmount -= min;

                    UnityEngine.Debug.Log("Physical shield prevent: " + min);
                }

                if (damageInfo.DamageType == DamageType.Magical)
                {
                    float min = Mathf.Min(CurrentMShield, damageInfo.DamageAmount);
                    CurrentMShield -= min;
                    damageInfo.DamageAmount -= min;
                }

                if (damageInfo.DamageAmount > 0)
                {
                    float min = Mathf.Min(CurrentShield, damageInfo.DamageAmount);
                    CurrentShield -= min;
                    damageInfo.DamageAmount -= min;

                    UnityEngine.Debug.Log("Shield prevent: " + min);

                }
            }

            CurrentHp = CurrentHp - damageInfo.DamageAmount;
            TakeDamage?.Invoke(this, new TakeDamageEventArgs(damageInfo));
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
       