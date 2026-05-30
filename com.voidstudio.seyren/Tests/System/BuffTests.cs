using NUnit.Framework;
using Seyren.System.Buffs;
using Seyren.System.Units;
using Seyren.System.States;
using Seyren.System.Actions;
using Seyren.System.Abilities;
using Seyren.System.Forces;
using Seyren.System.Damages;
using Seyren.System.Common;
using Seyren.Universe;
using Seyren.Payment;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Seyren.Tests.System
{
    [TestFixture]
    public class BuffTests
    {
        private BuffSystem buffSystem;
        private MockUnit testUnit;
        private MockUnit anotherUnit;

        [SetUp]
        public void Setup()
        {
            buffSystem = new BuffSystem();
            testUnit = new MockUnit("test-unit-1");
            anotherUnit = new MockUnit("test-unit-2");
        }

        [TearDown]
        public void TearDown()
        {
            buffSystem.ClearAllBuffs();
        }

        #region BuffSystem Basic Tests

        [Test]
        public void ApplyBuffToUnit_ValidBuff_AppliesSuccessfully()
        {
            // Arrange
            var buff = new MockBuff("test_buff", 10f);

            // Act
            buffSystem.ApplyBuffToUnit(testUnit, buff);

            // Assert
            Assert.IsTrue(buffSystem.IsUnitHasBuff(testUnit, "test_buff"));
            Assert.IsTrue(buff.WasApplied);
            Assert.AreEqual(1, buffSystem.GetBuffsFromUnit(testUnit).Count);
        }

        [Test]
        public void ApplyBuffToUnit_NullUnit_DoesNothing()
        {
            // Arrange
            var buff = new MockBuff("test_buff", 10f);

            // Act & Assert (should not throw)
            buffSystem.ApplyBuffToUnit(null, buff);
            Assert.IsFalse(buff.WasApplied);
        }

        [Test]
        public void ApplyBuffToUnit_NullBuff_DoesNothing()
        {
            // Act & Assert (should not throw)
            buffSystem.ApplyBuffToUnit(testUnit, null);
            Assert.AreEqual(0, buffSystem.GetBuffsFromUnit(testUnit).Count);
        }

        [Test]
        public void ApplyBuffToUnit_SameBuffTwice_ReplacesExistingBuff()
        {
            // Arrange
            var firstBuff = new MockBuff("test_buff", 10f);
            var secondBuff = new MockBuff("test_buff", 15f);

            // Act
            buffSystem.ApplyBuffToUnit(testUnit, firstBuff);
            buffSystem.ApplyBuffToUnit(testUnit, secondBuff);

            // Assert
            Assert.IsTrue(firstBuff.WasExpired, "First buff should have been expired");
            Assert.IsTrue(secondBuff.WasApplied, "Second buff should have been applied");
            Assert.AreEqual(1, buffSystem.GetBuffsFromUnit(testUnit).Count, "Should only have one buff");
            Assert.AreEqual(15f, buffSystem.GetBuffsFromUnit(testUnit)[0].GetDuration());
        }

        [Test]
        public void RemoveBuffFromUnit_ExistingBuff_RemovesSuccessfully()
        {
            // Arrange
            var buff = new MockBuff("test_buff", 10f);
            buffSystem.ApplyBuffToUnit(testUnit, buff);

            // Act
            buffSystem.RemoveBuffFromUnit(testUnit, "test_buff");

            // Assert
            Assert.IsFalse(buffSystem.IsUnitHasBuff(testUnit, "test_buff"));
            Assert.IsTrue(buff.WasExpired);
            Assert.AreEqual(0, buffSystem.GetBuffsFromUnit(testUnit).Count);
        }

        [Test]
        public void RemoveBuffFromUnit_NonExistentBuff_DoesNothing()
        {
            // Act & Assert (should not throw)
            buffSystem.RemoveBuffFromUnit(testUnit, "non_existent_buff");
            Assert.AreEqual(0, buffSystem.GetBuffsFromUnit(testUnit).Count);
        }

        [Test]
        public void IsUnitHasBuff_ExistingBuff_ReturnsTrue()
        {
            // Arrange
            var buff = new MockBuff("test_buff", 10f);
            buffSystem.ApplyBuffToUnit(testUnit, buff);

            // Act & Assert
            Assert.IsTrue(buffSystem.IsUnitHasBuff(testUnit, "test_buff"));
        }

        [Test]
        public void IsUnitHasBuff_NonExistentBuff_ReturnsFalse()
        {
            // Act & Assert
            Assert.IsFalse(buffSystem.IsUnitHasBuff(testUnit, "non_existent_buff"));
        }

        [Test]
        public void GetBuffsFromUnit_MultipleBuffs_ReturnsAllBuffs()
        {
            // Arrange
            var buff1 = new MockBuff("buff1", 10f);
            var buff2 = new MockBuff("buff2", 15f);
            buffSystem.ApplyBuffToUnit(testUnit, buff1);
            buffSystem.ApplyBuffToUnit(testUnit, buff2);

            // Act
            var buffs = buffSystem.GetBuffsFromUnit(testUnit);

            // Assert
            Assert.AreEqual(2, buffs.Count);
            Assert.IsTrue(buffs.Exists(b => b.GetId() == "buff1"));
            Assert.IsTrue(buffs.Exists(b => b.GetId() == "buff2"));
        }

        [Test]
        public void GetBuffsFromUnit_NoBuffs_ReturnsEmptyList()
        {
            // Act
            var buffs = buffSystem.GetBuffsFromUnit(testUnit);

            // Assert
            Assert.IsNotNull(buffs);
            Assert.AreEqual(0, buffs.Count);
        }

        [Test]
        public void Update_ExpiredBuff_RemovesAutomatically()
        {
            // Arrange
            var buff = new MockBuff("test_buff", 1f);
            buffSystem.ApplyBuffToUnit(testUnit, buff);

            // Act
            buffSystem.Update(2f); // Advance time past buff duration

            // Assert
            Assert.IsFalse(buffSystem.IsUnitHasBuff(testUnit, "test_buff"));
            Assert.IsTrue(buff.WasExpired);
            Assert.AreEqual(0, buffSystem.GetBuffsFromUnit(testUnit).Count);
        }

        [Test]
        public void ClearAllBuffs_MultipleUnitsWithBuffs_ClearsAll()
        {
            // Arrange
            var buff1 = new MockBuff("buff1", 10f);
            var buff2 = new MockBuff("buff2", 10f);
            buffSystem.ApplyBuffToUnit(testUnit, buff1);
            buffSystem.ApplyBuffToUnit(anotherUnit, buff2);

            // Act
            buffSystem.ClearAllBuffs();

            // Assert
            Assert.AreEqual(0, buffSystem.GetBuffsFromUnit(testUnit).Count);
            Assert.AreEqual(0, buffSystem.GetBuffsFromUnit(anotherUnit).Count);
            Assert.IsTrue(buff1.WasExpired);
            Assert.IsTrue(buff2.WasExpired);
        }

        [Test]
        public void ClearBuffs_SpecificUnit_ClearsOnlyThatUnit()
        {
            // Arrange
            var buff1 = new MockBuff("buff1", 10f);
            var buff2 = new MockBuff("buff2", 10f);
            buffSystem.ApplyBuffToUnit(testUnit, buff1);
            buffSystem.ApplyBuffToUnit(anotherUnit, buff2);

            // Act
            buffSystem.ClearBuffs(testUnit);

            // Assert
            Assert.AreEqual(0, buffSystem.GetBuffsFromUnit(testUnit).Count);
            Assert.AreEqual(1, buffSystem.GetBuffsFromUnit(anotherUnit).Count);
            Assert.IsTrue(buff1.WasExpired);
            Assert.IsFalse(buff2.WasExpired);
        }

        [Test]
        public void GetTotalBuffCount_MultipleUnitsWithBuffs_ReturnsCorrectCount()
        {
            // Arrange
            buffSystem.ApplyBuffToUnit(testUnit, new MockBuff("buff1", 10f));
            buffSystem.ApplyBuffToUnit(testUnit, new MockBuff("buff2", 10f));
            buffSystem.ApplyBuffToUnit(anotherUnit, new MockBuff("buff3", 10f));

            // Act & Assert
            Assert.AreEqual(3, buffSystem.GetTotalBuffCount());
        }

        [Test]
        public void GetAffectedUnitCount_MultipleUnitsWithBuffs_ReturnsCorrectCount()
        {
            // Arrange
            buffSystem.ApplyBuffToUnit(testUnit, new MockBuff("buff1", 10f));
            buffSystem.ApplyBuffToUnit(testUnit, new MockBuff("buff2", 10f));
            buffSystem.ApplyBuffToUnit(anotherUnit, new MockBuff("buff3", 10f));

            // Act & Assert
            Assert.AreEqual(2, buffSystem.GetAffectedUnitCount());
        }

        #endregion

        #region Common Buff Tests

        [Test]
        public void DevotionAuraBuff_ApplyAndExpire_ModifiesDefense()
        {
            // Arrange
            var buff = new DevotionAuraBuff(5f, 30f);
            float originalDefense = testUnit.Attribute.GetBaseFloat(AttributeName.DEFENSE).Total;

            // Act - Apply
            buffSystem.ApplyBuffToUnit(testUnit, buff);
            float buffedDefense = testUnit.Attribute.GetBaseFloat(AttributeName.DEFENSE).Total;

            // Assert - Apply
            Assert.AreEqual(originalDefense + 5f, buffedDefense, 0.01f);

            // Act - Expire
            buffSystem.RemoveBuffFromUnit(testUnit, "devotion_aura");
            float finalDefense = testUnit.Attribute.GetBaseFloat(AttributeName.DEFENSE).Total;

            // Assert - Expire
            Assert.AreEqual(originalDefense, finalDefense, 0.01f);
        }

        [Test]
        public void CurseBuff_ApplyAndExpire_ReducesAttackAndSpeed()
        {
            // Arrange
            var buff = new CurseBuff(10f, 2f, 60f);
            float originalAttack = testUnit.Attribute.GetBaseFloat(AttributeName.ATTACK_DAMAGE).Total;
            float originalSpeed = testUnit.Attribute.GetBaseFloat(AttributeName.MOVEMENT_SPEED).Total;

            // Act - Apply
            buffSystem.ApplyBuffToUnit(testUnit, buff);

            // Assert - Apply
            Assert.AreEqual(originalAttack - 10f, testUnit.Attribute.GetBaseFloat(AttributeName.ATTACK_DAMAGE).Total, 0.01f);
            Assert.AreEqual(originalSpeed - 2f, testUnit.Attribute.GetBaseFloat(AttributeName.MOVEMENT_SPEED).Total, 0.01f);

            // Act - Expire
            buffSystem.RemoveBuffFromUnit(testUnit, "curse");

            // Assert - Expire
            Assert.AreEqual(originalAttack, testUnit.Attribute.GetBaseFloat(AttributeName.ATTACK_DAMAGE).Total, 0.01f);
            Assert.AreEqual(originalSpeed, testUnit.Attribute.GetBaseFloat(AttributeName.MOVEMENT_SPEED).Total, 0.01f);
        }

        [Test]
        public void BloodlustBuff_ApplyAndExpire_IncreasesAttackSpeedAndMovement()
        {
            // Arrange
            var buff = new BloodlustBuff(1.5f, 50f, 45f);
            float originalAttackSpeed = testUnit.Attribute.GetBaseFloat(AttributeName.ATTACK_SPEED).Total;
            float originalMovementSpeed = testUnit.Attribute.GetBaseFloat(AttributeName.MOVEMENT_SPEED).Total;

            // Act - Apply
            buffSystem.ApplyBuffToUnit(testUnit, buff);

            // Assert - Apply
            Assert.AreEqual(originalAttackSpeed + 1.5f, testUnit.Attribute.GetBaseFloat(AttributeName.ATTACK_SPEED).Total, 0.01f);
            Assert.AreEqual(originalMovementSpeed + 50f, testUnit.Attribute.GetBaseFloat(AttributeName.MOVEMENT_SPEED).Total, 0.01f);

            // Act - Expire
            buffSystem.RemoveBuffFromUnit(testUnit, "bloodlust");

            // Assert - Expire
            Assert.AreEqual(originalAttackSpeed, testUnit.Attribute.GetBaseFloat(AttributeName.ATTACK_SPEED).Total, 0.01f);
            Assert.AreEqual(originalMovementSpeed, testUnit.Attribute.GetBaseFloat(AttributeName.MOVEMENT_SPEED).Total, 0.01f);
        }

        [Test]
        public void InnerFireBuff_ApplyAndExpire_IncreasesDamageAndArmor()
        {
            // Arrange
            var buff = new InnerFireBuff(15f, 8f, 60f);
            float originalDamage = testUnit.Attribute.GetBaseFloat(AttributeName.ATTACK_DAMAGE).Total;
            float originalDefense = testUnit.Attribute.GetBaseFloat(AttributeName.DEFENSE).Total;

            // Act - Apply
            buffSystem.ApplyBuffToUnit(testUnit, buff);

            // Assert - Apply
            Assert.AreEqual(originalDamage + 15f, testUnit.Attribute.GetBaseFloat(AttributeName.ATTACK_DAMAGE).Total, 0.01f);
            Assert.AreEqual(originalDefense + 8f, testUnit.Attribute.GetBaseFloat(AttributeName.DEFENSE).Total, 0.01f);

            // Act - Expire
            buffSystem.RemoveBuffFromUnit(testUnit, "inner_fire");

            // Assert - Expire
            Assert.AreEqual(originalDamage, testUnit.Attribute.GetBaseFloat(AttributeName.ATTACK_DAMAGE).Total, 0.01f);
            Assert.AreEqual(originalDefense, testUnit.Attribute.GetBaseFloat(AttributeName.DEFENSE).Total, 0.01f);
        }

        [Test]
        public void RegenerationBuff_OnTick_HealsOverTime()
        {
            // Arrange
            var buff = new RegenerationBuff(5f, 10f);
            testUnit.Attribute.SetFloat(AttributeName.CUR_HP, 50f); // Set current HP below max
            buffSystem.ApplyBuffToUnit(testUnit, buff);

            // Act - Simulate 1 second passing
            buffSystem.Update(1f);

            // Assert
            Assert.AreEqual(55f, testUnit.Attribute.GetFloat(AttributeName.CUR_HP), 0.01f);
        }

        [Test]
        public void PoisonBuff_OnTick_DamagesOverTime()
        {
            // Arrange
            var buff = new PoisonBuff(3f, 10f);
            testUnit.Attribute.SetFloat(AttributeName.CUR_HP, 50f);
            buffSystem.ApplyBuffToUnit(testUnit, buff);

            // Act - Simulate 1 second passing
            buffSystem.Update(1f);

            // Assert
            Assert.AreEqual(47f, testUnit.Attribute.GetFloat(AttributeName.CUR_HP), 0.01f);
        }

        [Test]
        public void SlowBuff_ApplyAndExpire_ReducesSpeedAttributes()
        {
            // Arrange
            var buff = new SlowBuff(30f, 0.5f, 15f);
            float originalMovement = testUnit.Attribute.GetBaseFloat(AttributeName.MOVEMENT_SPEED).Total;
            float originalAttackSpeed = testUnit.Attribute.GetBaseFloat(AttributeName.ATTACK_SPEED).Total;

            // Act - Apply
            buffSystem.ApplyBuffToUnit(testUnit, buff);

            // Assert - Apply
            Assert.AreEqual(originalMovement - 30f, testUnit.Attribute.GetBaseFloat(AttributeName.MOVEMENT_SPEED).Total, 0.01f);
            Assert.AreEqual(originalAttackSpeed - 0.5f, testUnit.Attribute.GetBaseFloat(AttributeName.ATTACK_SPEED).Total, 0.01f);

            // Act - Expire
            buffSystem.RemoveBuffFromUnit(testUnit, "slow");

            // Assert - Expire
            Assert.AreEqual(originalMovement, testUnit.Attribute.GetBaseFloat(AttributeName.MOVEMENT_SPEED).Total, 0.01f);
            Assert.AreEqual(originalAttackSpeed, testUnit.Attribute.GetBaseFloat(AttributeName.ATTACK_SPEED).Total, 0.01f);
        }

        [Test]
        public void InvisibilityBuff_ApplyAndExpire_IncreasesMovementSpeed()
        {
            // Arrange
            var buff = new InvisibilityBuff(25f, 20f);
            float originalSpeed = testUnit.Attribute.GetBaseFloat(AttributeName.MOVEMENT_SPEED).Total;

            // Act - Apply
            buffSystem.ApplyBuffToUnit(testUnit, buff);

            // Assert - Apply
            Assert.AreEqual(originalSpeed + 25f, testUnit.Attribute.GetBaseFloat(AttributeName.MOVEMENT_SPEED).Total, 0.01f);

            // Act - Expire
            buffSystem.RemoveBuffFromUnit(testUnit, "invisibility");

            // Assert - Expire
            Assert.AreEqual(originalSpeed, testUnit.Attribute.GetBaseFloat(AttributeName.MOVEMENT_SPEED).Total, 0.01f);
        }

        [Test]
        public void BaseBuff_Expiration_WorksCorrectly()
        {
            // Arrange
            var buff = new MockBuff("expiring_buff", 2f);
            buffSystem.ApplyBuffToUnit(testUnit, buff);

            // Act & Assert - Not expired initially
            Assert.IsFalse(buff.IsExpired());
            Assert.IsTrue(buffSystem.IsUnitHasBuff(testUnit, "expiring_buff"));

            // Act - Advance time partially
            buffSystem.Update(1f);
            Assert.IsFalse(buff.IsExpired());
            Assert.IsTrue(buffSystem.IsUnitHasBuff(testUnit, "expiring_buff"));

            // Act - Advance time to expiration
            buffSystem.Update(1.1f);
            Assert.IsFalse(buffSystem.IsUnitHasBuff(testUnit, "expiring_buff"));
        }

        #endregion

        #region Edge Cases and Integration Tests

        [Test]
        public void MultipleBuffsOnSameAttribute_StackCorrectly()
        {
            // Arrange
            var devotionBuff = new DevotionAuraBuff(5f, 30f);
            var innerFireBuff = new InnerFireBuff(10f, 3f, 60f);
            float originalDefense = testUnit.Attribute.GetBaseFloat(AttributeName.DEFENSE).Total;

            // Act
            buffSystem.ApplyBuffToUnit(testUnit, devotionBuff);
            buffSystem.ApplyBuffToUnit(testUnit, innerFireBuff);

            // Assert
            float expectedDefense = originalDefense + 5f + 3f; // Both buffs should stack
            Assert.AreEqual(expectedDefense, testUnit.Attribute.GetBaseFloat(AttributeName.DEFENSE).Total, 0.01f);
        }

        [Test]
        public void BuffsWithSameAttributeName_OverwriteCorrectly()
        {
            // Arrange
            var firstDefenseBuff = new DevotionAuraBuff(5f, 30f);
            var secondDefenseBuff = new DevotionAuraBuff(10f, 45f);
            float originalDefense = testUnit.Attribute.GetBaseFloat(AttributeName.DEFENSE).Total;

            // Act
            buffSystem.ApplyBuffToUnit(testUnit, firstDefenseBuff);
            buffSystem.ApplyBuffToUnit(testUnit, secondDefenseBuff);

            // Assert - Should only have the second buff's effect
            float expectedDefense = originalDefense + 10f;
            Assert.AreEqual(expectedDefense, testUnit.Attribute.GetBaseFloat(AttributeName.DEFENSE).Total, 0.01f);
            Assert.AreEqual(1, buffSystem.GetBuffsFromUnit(testUnit).Count);
        }

        #endregion
    }

    #region Mock Classes

    /// <summary>
    /// Mock implementation of IUnit for testing
    /// </summary>
    public class MockUnit : IUnit
    {
        private readonly string id;
        private readonly MockAttribute attribute;

        public MockUnit(string unitId)
        {
            id = unitId;
            attribute = new MockAttribute();
        }

        public string ID => id;
        public IAttribute Attribute => attribute;
        public IUnit Owner => null;
        public Force Force { get; set; }
        public ObjectStatus ObjectStatus { get; set; }
        public Vector3 Location => Vector3.zero;
        public Vector3 Size { get; set; } = Vector3.one;
        public Quaternion Rotation => Quaternion.identity;
        public string ReferenceID => id;
        public bool IsActive => true;
        public string UnitType => "MockUnit";
        public ActionQueue ActionQueue => null;
        public Vector3 Forward => Vector3.forward;
        public IResourceManager ResourceManager => null;
        public IPaymentProcessor PaymentProcessor => null;

        public Error Move(Vector3 location) => Error.None;
        public Error Look(Quaternion quaternion) => Error.None;
        public AbilityV2 GetAbility(string abilityID) => null;
        public List<IModifier> GetModifiers() => new List<IModifier>();
        public List<IResistance> GetResistances() => new List<IResistance>();
        public List<IOnHitEffect> GetOnHitEffects() => new List<IOnHitEffect>();
        public void InflictDamage(Damage damage) { }
        public bool IsImmune() => false;
        public void Loop(ITime time) { }
    }

    /// <summary>
    /// Mock implementation of IAttribute for testing
    /// </summary>
    public class MockAttribute : IAttribute
    {
        private readonly Dictionary<string, BaseFloat> floatAttributes;
        private readonly Dictionary<string, float> currentValues;

        public MockAttribute()
        {
            floatAttributes = new Dictionary<string, BaseFloat>
            {
                { AttributeName.STRENGTH, new BaseFloat(10f) },
                { AttributeName.AGILITY, new BaseFloat(10f) },
                { AttributeName.INTELLIGENT, new BaseFloat(10f) },
                { AttributeName.MAX_HP, new BaseFloat(100f) },
                { AttributeName.MAX_MP, new BaseFloat(100f) },
                { AttributeName.HP_REGEN, new BaseFloat(1f) },
                { AttributeName.MP_REGEN, new BaseFloat(1f) },
                { AttributeName.ATTACK_DAMAGE, new BaseFloat(20f) },
                { AttributeName.DEFENSE, new BaseFloat(5f) },
                { AttributeName.ATTACK_RANGE, new BaseFloat(1.5f) },
                { AttributeName.CAST_RANGE, new BaseFloat(6f) },
                { AttributeName.MOVEMENT_SPEED, new BaseFloat(100f) },
                { AttributeName.ATTACK_SPEED, new BaseFloat(1f) }
            };

            currentValues = new Dictionary<string, float>
            {
                { AttributeName.CUR_HP, 100f },
                { AttributeName.CUR_MP, 100f }
            };
        }

        public BaseFloat GetBaseFloat(string name)
        {
            return floatAttributes.TryGetValue(name, out var value) ? value : BaseFloat.Zero;
        }

        public BaseInt GetBaseInt(string name) => BaseInt.Zero;

        public BaseFloat AddBaseFloat(string name, float val)
        {
            var baseFloat = GetBaseFloat(name);
            baseFloat.Increase(val);
            return baseFloat;
        }

        public BaseInt AddBaseInt(string name, int val) => BaseInt.Zero;

        public BaseFloat SetBaseFloat(string name, float val)
        {
            var baseFloat = GetBaseFloat(name);
            baseFloat.Base = val;
            return baseFloat;
        }

        public BaseInt SetBaseInt(string name, int val) => BaseInt.Zero;

        public float GetFloat(string name)
        {
            return currentValues.TryGetValue(name, out var value) ? value : 0f;
        }

        public int GetInt(string name) => (int)GetFloat(name);

        public void SetFloat(string name, float val)
        {
            currentValues[name] = val;
        }

        public void SetInt(string name, int val)
        {
            SetFloat(name, val);
        }

        public void IncreaseFloat(string name, float val)
        {
            if (currentValues.ContainsKey(name))
                currentValues[name] += val;
        }

        public void IncreaseInt(string name, int val)
        {
            IncreaseFloat(name, val);
        }

        public float this[string key]
        {
            get => GetFloat(key);
            set => SetFloat(key, value);
        }
    }

    /// <summary>
    /// Mock implementation of IBuff for testing
    /// </summary>
    public class MockBuff : IBuff
    {
        private readonly string id;
        private readonly float duration;
        private float remainingTime;
        private IUnit target;

        public bool WasApplied { get; private set; }
        public bool WasExpired { get; private set; }
        public int TickCount { get; private set; }

        public MockBuff(string buffId, float buffDuration)
        {
            id = buffId;
            duration = buffDuration;
            remainingTime = buffDuration;
        }

        public string GetId() => id;
        public IUnit GetOwner() => null;
        public IUnit GetTarget() => target;
        public float GetDuration() => duration;
        public float GetRemainingTime() => remainingTime;
        public bool IsExpired() => remainingTime <= 0f;

        /// <summary>
        /// Applies this buff to the specified target unit
        /// </summary>
        public void ApplyBuffToUnit(IUnit buffTarget)
        {
            target = buffTarget;
            OnApply();
        }

        public void OnApply()
        {
            WasApplied = true;
        }

        public void OnExpire()
        {
            WasExpired = true;
        }

        public void OnTick(float deltaTime)
        {
            remainingTime -= deltaTime;
            TickCount++;
        }
    }

    #endregion
}