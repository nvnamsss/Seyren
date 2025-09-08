using NUnit.Framework;
using Seyren.System.States;
using UnityEngine;

namespace Seyren.Tests.System
{
    [TestFixture]
    public class BaseIntTests
    {
        [Test]
        public void Constructor_SetsBaseAndIncr()
        {
            Debug.Log("[BaseIntTests] Running Constructor_SetsBaseAndIncr");
            var baseInt = new BaseInt(10, 5);
            Debug.Log($"[BaseIntTests] Created BaseInt: Base={baseInt.Base}, Incr={baseInt.Incr}, Total={baseInt.Total}");
            Assert.AreEqual(10, baseInt.Base);
            Assert.AreEqual(5, baseInt.Incr);
            Assert.AreEqual(15, baseInt.Total);
        }

        [Test]
        public void Constructor_WithFloorAndCeiling_SetsAllValues()
        {
            Debug.Log("[BaseIntTests] Running Constructor_WithFloorAndCeiling_SetsAllValues");
            var baseInt = new BaseInt(10, 5, 0, 20);
            Debug.Log($"[BaseIntTests] Created BaseInt: Base={baseInt.Base}, Incr={baseInt.Incr}, Floor={baseInt.Floor}, Ceiling={baseInt.Ceiling}, Total={baseInt.Total}");
            Assert.AreEqual(10, baseInt.Base);
            Assert.AreEqual(5, baseInt.Incr);
            Assert.AreEqual(0, baseInt.Floor);
            Assert.AreEqual(20, baseInt.Ceiling);
            Assert.AreEqual(15, baseInt.Total);
        }

        [Test]
        public void Total_RespectsFloorConstraint()
        {
            Debug.Log("[BaseIntTests] Running Total_RespectsFloorConstraint");
            var baseInt = new BaseInt(-10, -5, 0, 100);
            Debug.Log($"[BaseIntTests] Created BaseInt: Base={baseInt.Base}, Incr={baseInt.Incr}, Floor={baseInt.Floor}, Ceiling={baseInt.Ceiling}, Total={baseInt.Total}");
            Assert.AreEqual(0, baseInt.Total); // Should be clamped to floor
        }

        [Test]
        public void Total_RespectsCeilingConstraint()
        {
            Debug.Log("[BaseIntTests] Running Total_RespectsCeilingConstraint");
            var baseInt = new BaseInt(50, 60, 0, 100);
            Debug.Log($"[BaseIntTests] Created BaseInt: Base={baseInt.Base}, Incr={baseInt.Incr}, Floor={baseInt.Floor}, Ceiling={baseInt.Ceiling}, Total={baseInt.Total}");
            Assert.AreEqual(100, baseInt.Total); // Should be clamped to ceiling
        }

        [Test]
        public void Increase_AddsToIncr()
        {
            Debug.Log("[BaseIntTests] Running Increase_AddsToIncr");
            var baseInt = new BaseInt(10, 5);
            Debug.Log($"[BaseIntTests] Before Increase: Base={baseInt.Base}, Incr={baseInt.Incr}, Total={baseInt.Total}");
            var result = baseInt.Increase(3);
            Debug.Log($"[BaseIntTests] After Increase: Base={baseInt.Base}, Incr={baseInt.Incr}, Total={baseInt.Total}, Result={result}");
            Assert.AreEqual(8, baseInt.Incr);
            Assert.AreEqual(18, result);
            Assert.AreEqual(18, baseInt.Total);
        }

        [Test]
        public void SetFloor_UpdatesFloorAndReturnsTotal()
        {
            Debug.Log("[BaseIntTests] Running SetFloor_UpdatesFloorAndReturnsTotal");
            var baseInt = new BaseInt(5, 0);
            Debug.Log($"[BaseIntTests] Before SetFloor: Floor={baseInt.Floor}, Total={baseInt.Total}");
            var result = baseInt.SetFloor(10);
            Debug.Log($"[BaseIntTests] After SetFloor: Floor={baseInt.Floor}, Total={baseInt.Total}, Result={result}");
            Assert.AreEqual(10, baseInt.Floor);
            Assert.AreEqual(10, result); // Should be clamped to new floor
        }

        [Test]
        public void SetCeiling_UpdatesCeilingAndReturnsTotal()
        {
            Debug.Log("[BaseIntTests] Running SetCeiling_UpdatesCeilingAndReturnsTotal");
            var baseInt = new BaseInt(20, 0);
            Debug.Log($"[BaseIntTests] Before SetCeiling: Ceiling={baseInt.Ceiling}, Total={baseInt.Total}");
            var result = baseInt.SetCeiling(15);
            Debug.Log($"[BaseIntTests] After SetCeiling: Ceiling={baseInt.Ceiling}, Total={baseInt.Total}, Result={result}");
            Assert.AreEqual(15, baseInt.Ceiling);
            Assert.AreEqual(15, result); // Should be clamped to new ceiling
        }

        [Test]
        public void SetRange_UpdatesBothFloorAndCeiling()
        {
            Debug.Log("[BaseIntTests] Running SetRange_UpdatesBothFloorAndCeiling");
            var baseInt = new BaseInt(50, 0);
            Debug.Log($"[BaseIntTests] Before SetRange: Floor={baseInt.Floor}, Ceiling={baseInt.Ceiling}, Total={baseInt.Total}");
            var result = baseInt.SetRange(10, 40);
            Debug.Log($"[BaseIntTests] After SetRange: Floor={baseInt.Floor}, Ceiling={baseInt.Ceiling}, Total={baseInt.Total}, Result={result}");
            Assert.AreEqual(10, baseInt.Floor);
            Assert.AreEqual(40, baseInt.Ceiling);
            Assert.AreEqual(40, result); // Should be clamped to new ceiling
        }

        [Test]
        public void Zero_StaticValue_IsCorrect()
        {
            Debug.Log("[BaseIntTests] Running Zero_StaticValue_IsCorrect");
            Assert.AreEqual(0, BaseInt.Zero.Base);
            Assert.AreEqual(0, BaseInt.Zero.Incr);
            Assert.AreEqual(0, BaseInt.Zero.Total);
        }
    }

    [TestFixture]
    public class BaseFloatTests
    {
        [Test]
        public void Constructor_SetsBaseAndIncr()
        {
            Debug.Log("[BaseFloatTests] Running Constructor_SetsBaseAndIncr");
            var baseFloat = new BaseFloat(10.5f, 2.3f);
            Debug.Log($"[BaseFloatTests] Created BaseFloat: Base={baseFloat.Base}, Incr={baseFloat.Incr}, Total={baseFloat.Total}");
            Assert.AreEqual(10.5f, baseFloat.Base, 0.001f);
            Assert.AreEqual(2.3f, baseFloat.Incr, 0.001f);
            Assert.AreEqual(12.8f, baseFloat.Total, 0.001f);
        }

        [Test]
        public void Constructor_WithFloorAndCeiling_SetsAllValues()
        {
            Debug.Log("[BaseFloatTests] Running Constructor_WithFloorAndCeiling_SetsAllValues");
            var baseFloat = new BaseFloat(10.5f, 2.3f, 0f, 20f);
            Debug.Log($"[BaseFloatTests] Created BaseFloat: Base={baseFloat.Base}, Incr={baseFloat.Incr}, Floor={baseFloat.Floor}, Ceiling={baseFloat.Ceiling}, Total={baseFloat.Total}");
            Assert.AreEqual(10.5f, baseFloat.Base, 0.001f);
            Assert.AreEqual(2.3f, baseFloat.Incr, 0.001f);
            Assert.AreEqual(0f, baseFloat.Floor, 0.001f);
            Assert.AreEqual(20f, baseFloat.Ceiling, 0.001f);
            Assert.AreEqual(12.8f, baseFloat.Total, 0.001f);
        }

        [Test]
        public void Total_RespectsFloorConstraint()
        {
            Debug.Log("[BaseFloatTests] Running Total_RespectsFloorConstraint");
            var baseFloat = new BaseFloat(-10f, -5f, 0f, 100f);
            Debug.Log($"[BaseFloatTests] Created BaseFloat: Base={baseFloat.Base}, Incr={baseFloat.Incr}, Floor={baseFloat.Floor}, Ceiling={baseFloat.Ceiling}, Total={baseFloat.Total}");
            Assert.AreEqual(0f, baseFloat.Total, 0.001f); // Should be clamped to floor
        }

        [Test]
        public void Total_RespectsCeilingConstraint()
        {
            Debug.Log("[BaseFloatTests] Running Total_RespectsCeilingConstraint");
            var baseFloat = new BaseFloat(50f, 60f, 0f, 100f);
            Debug.Log($"[BaseFloatTests] Created BaseFloat: Base={baseFloat.Base}, Incr={baseFloat.Incr}, Floor={baseFloat.Floor}, Ceiling={baseFloat.Ceiling}, Total={baseFloat.Total}");
            Assert.AreEqual(100f, baseFloat.Total, 0.001f); // Should be clamped to ceiling
        }

        [Test]
        public void AddBonus_AddsToTotalAndReturnsClamped()
        {
            Debug.Log("[BaseFloatTests] Running AddBonus_AddsToTotalAndReturnsClamped");
            var baseFloat = new BaseFloat(10f, 0f, 0f, 20f);
            Debug.Log($"[BaseFloatTests] Before AddBonus: Base={baseFloat.Base}, Incr={baseFloat.Incr}, Total={baseFloat.Total}");
            var result = baseFloat.AddBonus("test", 5f);
            Debug.Log($"[BaseFloatTests] After AddBonus: Base={baseFloat.Base}, Incr={baseFloat.Incr}, Total={baseFloat.Total}, Result={result}");
            Assert.AreEqual(5f, baseFloat.Incr, 0.001f);
            Assert.AreEqual(15f, result, 0.001f);
            Assert.AreEqual(15f, baseFloat.Total, 0.001f);
        }

        [Test]
        public void AddBonus_ExceedsCeiling_ClampedInTotal()
        {
            Debug.Log("[BaseFloatTests] Running AddBonus_ExceedsCeiling_ClampedInTotal");
            var baseFloat = new BaseFloat(10f, 0f, 0f, 20f);
            Debug.Log($"[BaseFloatTests] Before AddBonus: Base={baseFloat.Base}, Incr={baseFloat.Incr}, Total={baseFloat.Total}");
            baseFloat.AddBonus("test", 50f); // This would make total 60, but should clamp to 20
            Debug.Log($"[BaseFloatTests] After AddBonus: Base={baseFloat.Base}, Incr={baseFloat.Incr}, Total={baseFloat.Total}");
            Assert.AreEqual(50f, baseFloat.Incr); // Incr should store the actual bonus
            Assert.AreEqual(20f, baseFloat.Total, 0.001f); // Total should be clamped
        }

        [Test]
        public void RemoveBonus_RemovesFromTotal()
        {
            Debug.Log("[BaseFloatTests] Running RemoveBonus_RemovesFromTotal");
            var baseFloat = new BaseFloat(10f);
            baseFloat.AddBonus("test1", 5f);
            baseFloat.AddBonus("test2", 3f);
            Debug.Log($"[BaseFloatTests] Before RemoveBonus: Base={baseFloat.Base}, Incr={baseFloat.Incr}, Total={baseFloat.Total}");
            var result = baseFloat.RemoveBonus("test1");
            Debug.Log($"[BaseFloatTests] After RemoveBonus: Base={baseFloat.Base}, Incr={baseFloat.Incr}, Total={baseFloat.Total}, Result={result}");
            Assert.AreEqual(3f, baseFloat.Incr, 0.001f);
            Assert.AreEqual(13f, result, 0.001f);
            Assert.AreEqual(13f, baseFloat.Total, 0.001f);
        }

        [Test]
        public void RemoveBonus_NonExistentKey_DoesNothing()
        {
            Debug.Log("[BaseFloatTests] Running RemoveBonus_NonExistentKey_DoesNothing");
            var baseFloat = new BaseFloat(10f);
            baseFloat.AddBonus("test", 5f);
            Debug.Log($"[BaseFloatTests] Before RemoveBonus: Base={baseFloat.Base}, Incr={baseFloat.Incr}, Total={baseFloat.Total}");
            var result = baseFloat.RemoveBonus("nonexistent");
            Debug.Log($"[BaseFloatTests] After RemoveBonus: Base={baseFloat.Base}, Incr={baseFloat.Incr}, Total={baseFloat.Total}, Result={result}");
            Assert.AreEqual(5f, baseFloat.Incr, 0.001f);
            Assert.AreEqual(15f, result, 0.001f);
        }

        [Test]
        public void HasBonus_ExistingKey_ReturnsTrue()
        {
            Debug.Log("[BaseFloatTests] Running HasBonus_ExistingKey_ReturnsTrue");
            var baseFloat = new BaseFloat(10f);
            baseFloat.AddBonus("test", 5f);
            Debug.Log($"[BaseFloatTests] HasBonus 'test': {baseFloat.HasBonus("test")}, HasBonus 'nonexistent': {baseFloat.HasBonus("nonexistent")}");
            Assert.IsTrue(baseFloat.HasBonus("test"));
            Assert.IsFalse(baseFloat.HasBonus("nonexistent"));
        }

        [Test]
        public void GetBonusKeys_ReturnsAllKeys()
        {
            Debug.Log("[BaseFloatTests] Running GetBonusKeys_ReturnsAllKeys");
            var baseFloat = new BaseFloat(10f);
            baseFloat.AddBonus("key1", 5f);
            baseFloat.AddBonus("key2", 3f);
            var keys = baseFloat.GetBonusKeys();
            Debug.Log($"[BaseFloatTests] Bonus keys: {string.Join(",", keys)}");
            Assert.AreEqual(2, keys.Length);
            Assert.Contains("key1", keys);
            Assert.Contains("key2", keys);
        }

        [Test]
        public void ClearBonuses_RemovesAllBonuses()
        {
            Debug.Log("[BaseFloatTests] Running ClearBonuses_RemovesAllBonuses");
            var baseFloat = new BaseFloat(10f);
            baseFloat.AddBonus("key1", 5f);
            baseFloat.AddBonus("key2", 3f);
            Debug.Log($"[BaseFloatTests] Before ClearBonuses: Base={baseFloat.Base}, Incr={baseFloat.Incr}, Total={baseFloat.Total}, BonusKeys={string.Join(",", baseFloat.GetBonusKeys())}");
            var result = baseFloat.ClearBonuses();
            Debug.Log($"[BaseFloatTests] After ClearBonuses: Base={baseFloat.Base}, Incr={baseFloat.Incr}, Total={baseFloat.Total}, Result={result}, BonusKeys={string.Join(",", baseFloat.GetBonusKeys())}");
            Assert.AreEqual(0f, baseFloat.Incr, 0.001f);
            Assert.AreEqual(10f, result, 0.001f);
            Assert.AreEqual(0, baseFloat.GetBonusKeys().Length);
        }

        [Test]
        public void BonusIndexer_GetAndSet()
        {
            Debug.Log("[BaseFloatTests] Running BonusIndexer_GetAndSet");
            var baseFloat = new BaseFloat(10f);
            baseFloat["test"] = 5f;
            Debug.Log($"[BaseFloatTests] BonusIndexer 'test': {baseFloat["test"]}, 'nonexistent': {baseFloat["nonexistent"]}");
            Assert.AreEqual(5f, baseFloat["test"], 0.001f);
            Assert.AreEqual(0f, baseFloat["nonexistent"], 0.001f);
        }

        [Test]
        public void Amplify_SetsIncrBasedOnBasePercentage()
        {
            Debug.Log("[BaseFloatTests] Running Amplify_SetsIncrBasedOnBasePercentage");
            var baseFloat = new BaseFloat(100f);
            Debug.Log($"[BaseFloatTests] Before Amplify: Base={baseFloat.Base}, Incr={baseFloat.Incr}, Total={baseFloat.Total}");
            var result = baseFloat.Amplify(25f); // 25% of 100 = 25
            Debug.Log($"[BaseFloatTests] After Amplify: Base={baseFloat.Base}, Incr={baseFloat.Incr}, Total={baseFloat.Total}, Result={result}");
            Assert.AreEqual(25f, baseFloat.Incr, 0.001f);
            Assert.AreEqual(125f, result, 0.001f);
        }

        [Test]
        public void Increase_AddsToIncr()
        {
            Debug.Log("[BaseFloatTests] Running Increase_AddsToIncr");
            var baseFloat = new BaseFloat(10f, 5f);
            Debug.Log($"[BaseFloatTests] Before Increase: Base={baseFloat.Base}, Incr={baseFloat.Incr}, Total={baseFloat.Total}");
            var result = baseFloat.Increase(3f);
            Debug.Log($"[BaseFloatTests] After Increase: Base={baseFloat.Base}, Incr={baseFloat.Incr}, Total={baseFloat.Total}, Result={result}");
            Assert.AreEqual(8f, baseFloat.Incr, 0.001f);
            Assert.AreEqual(18f, result, 0.001f);
        }

        [Test]
        public void SetFloor_UpdatesFloorAndReturnsTotal()
        {
            Debug.Log("[BaseFloatTests] Running SetFloor_UpdatesFloorAndReturnsTotal");
            var baseFloat = new BaseFloat(5f, 0f);
            Debug.Log($"[BaseFloatTests] Before SetFloor: Floor={baseFloat.Floor}, Total={baseFloat.Total}");
            var result = baseFloat.SetFloor(10f);
            Debug.Log($"[BaseFloatTests] After SetFloor: Floor={baseFloat.Floor}, Total={baseFloat.Total}, Result={result}");
            Assert.AreEqual(10f, baseFloat.Floor, 0.001f);
            Assert.AreEqual(10f, result, 0.001f); // Should be clamped to new floor
        }

        [Test]
        public void SetCeiling_UpdatesCeilingAndReturnsTotal()
        {
            Debug.Log("[BaseFloatTests] Running SetCeiling_UpdatesCeilingAndReturnsTotal");
            var baseFloat = new BaseFloat(20f, 0f);
            Debug.Log($"[BaseFloatTests] Before SetCeiling: Ceiling={baseFloat.Ceiling}, Total={baseFloat.Total}");
            var result = baseFloat.SetCeiling(15f);
            Debug.Log($"[BaseFloatTests] After SetCeiling: Ceiling={baseFloat.Ceiling}, Total={baseFloat.Total}, Result={result}");
            Assert.AreEqual(15f, baseFloat.Ceiling, 0.001f);
            Assert.AreEqual(15f, result, 0.001f); // Should be clamped to new ceiling
        }

        [Test]
        public void SetRange_UpdatesBothFloorAndCeiling()
        {
            Debug.Log("[BaseFloatTests] Running SetRange_UpdatesBothFloorAndCeiling");
            var baseFloat = new BaseFloat(50f, 0f);
            Debug.Log($"[BaseFloatTests] Before SetRange: Floor={baseFloat.Floor}, Ceiling={baseFloat.Ceiling}, Total={baseFloat.Total}");
            var result = baseFloat.SetRange(10f, 40f);
            Debug.Log($"[BaseFloatTests] After SetRange: Floor={baseFloat.Floor}, Ceiling={baseFloat.Ceiling}, Total={baseFloat.Total}, Result={result}");
            Assert.AreEqual(10f, baseFloat.Floor, 0.001f);
            Assert.AreEqual(40f, baseFloat.Ceiling, 0.001f);
            Assert.AreEqual(40f, result, 0.001f); // Should be clamped to new ceiling
        }

        [Test]
        public void OperatorPlus_BaseFloatAndBaseFloat()
        {
            Debug.Log("[BaseFloatTests] Running OperatorPlus_BaseFloatAndBaseFloat");
            var left = new BaseFloat(10f, 5f);
            var right = new BaseFloat(3f, 2f);
            Debug.Log($"[BaseFloatTests] Left: Base={left.Base}, Incr={left.Incr}, Right: Base={right.Base}, Incr={right.Incr}");
            var result = left + right;
            Debug.Log($"[BaseFloatTests] Result: Base={result.Base}, Incr={result.Incr}");
            Assert.AreEqual(13f, result.Base, 0.001f);
            Assert.AreEqual(7f, result.Incr, 0.001f);
        }

        [Test]
        public void OperatorMinus_BaseFloatAndBaseFloat()
        {
            Debug.Log("[BaseFloatTests] Running OperatorMinus_BaseFloatAndBaseFloat");
            var left = new BaseFloat(10f, 5f);
            var right = new BaseFloat(3f, 2f);
            Debug.Log($"[BaseFloatTests] Left: Base={left.Base}, Incr={left.Incr}, Right: Base={right.Base}, Incr={right.Incr}");
            var result = left - right;
            Debug.Log($"[BaseFloatTests] Result: Base={result.Base}, Incr={result.Incr}");
            Assert.AreEqual(7f, result.Base, 0.001f);
            Assert.AreEqual(3f, result.Incr, 0.001f);
        }

        [Test]
        public void OperatorPlus_BaseFloatAndFloat()
        {
            Debug.Log("[BaseFloatTests] Running OperatorPlus_BaseFloatAndFloat");
            var baseFloat = new BaseFloat(10f, 5f);
            Debug.Log($"[BaseFloatTests] Before OperatorPlus: Base={baseFloat.Base}, Incr={baseFloat.Incr}");
            var result = baseFloat + 3f;
            Debug.Log($"[BaseFloatTests] After OperatorPlus: Base={result.Base}, Incr={result.Incr}");
            Assert.AreEqual(13f, result.Base, 0.001f);
            Assert.AreEqual(5f, result.Incr, 0.001f);
        }

        [Test]
        public void OperatorMinus_BaseFloatAndFloat()
        {
            Debug.Log("[BaseFloatTests] Running OperatorMinus_BaseFloatAndFloat");
            var baseFloat = new BaseFloat(10f, 5f);
            Debug.Log($"[BaseFloatTests] Before OperatorMinus: Base={baseFloat.Base}, Incr={baseFloat.Incr}");
            var result = baseFloat - 3f;
            Debug.Log($"[BaseFloatTests] After OperatorMinus: Base={result.Base}, Incr={result.Incr}");
            Assert.AreEqual(7f, result.Base, 0.001f);
            Assert.AreEqual(5f, result.Incr, 0.001f);
        }

        [Test]
        public void Zero_StaticValue_IsCorrect()
        {
            Debug.Log("[BaseFloatTests] Running Zero_StaticValue_IsCorrect");
            Assert.AreEqual(0f, BaseFloat.Zero.Base, 0.001f);
            Assert.AreEqual(0f, BaseFloat.Zero.Incr, 0.001f);
            Assert.AreEqual(0f, BaseFloat.Zero.Total, 0.001f);
        }
    }

    [TestFixture]
    public class Warcraft3AttributeTests
    {
        private Warcraft3Attribute attribute;

        [SetUp]
        public void SetUp()
        {
            attribute = new Warcraft3Attribute();
        }

        [Test]
        public void Constructor_SetsDefaultValues()
        {
            Assert.AreEqual(10f, attribute.Strength, 0.001f);
            Assert.AreEqual(10f, attribute.Agility, 0.001f);
            Assert.AreEqual(10f, attribute.Intelligence, 0.001f);
            Assert.AreEqual(1, attribute.Level);
        }

        [Test]
        public void Constructor_WithCustomValues()
        {
            var customAttribute = new Warcraft3Attribute(15f, 20f, 25f);
            
            Assert.AreEqual(15f, customAttribute.Strength, 0.001f);
            Assert.AreEqual(20f, customAttribute.Agility, 0.001f);
            Assert.AreEqual(25f, customAttribute.Intelligence, 0.001f);
        }

        [Test]
        public void AttackDamage_IncludesStrengthBonus()
        {
            // Base attack damage is 0, strength is 10, so total should be 10
            Assert.AreEqual(10f, attribute.AttackDamage, 0.001f);
            
            // Add 5 strength
            attribute.GetBaseFloat(AttributeName.STRENGTH).AddBonus("test", 5f);
            Assert.AreEqual(15f, attribute.AttackDamage, 0.001f);
        }

        [Test]
        public void Armor_IncludesAgilityBonus()
        {
            // Base armor is 0, agility is 10, so armor should be 10/7 ≈ 1.43
            Assert.AreEqual(10f / 7f, attribute.Armor, 0.001f);
            
            // Add 7 agility (should add 1 armor)
            attribute.GetBaseFloat(AttributeName.AGILITY).AddBonus("test", 7f);
            Assert.AreEqual(17f / 7f, attribute.Armor, 0.001f);
        }

        [Test]
        public void MaxHp_IncludesStrengthBonus()
        {
            // Base HP is 100, strength is 10, so total should be 100 + (10 * 19) = 290
            Assert.AreEqual(290f, attribute.GetMaxHp(), 0.001f);
            
            // Add 5 strength (should add 95 HP)
            attribute.GetBaseFloat(AttributeName.STRENGTH).AddBonus("test", 5f);
            Assert.AreEqual(385f, attribute.GetMaxHp(), 0.001f);
        }

        [Test]
        public void MaxMp_IncludesIntelligenceBonus()
        {
            // Base MP is 100, intelligence is 10, so total should be 100 + (10 * 13) = 230
            Assert.AreEqual(230f, attribute.GetMaxMp(), 0.001f);
            
            // Add 5 intelligence (should add 65 MP)
            attribute.GetBaseFloat(AttributeName.INTELLIGENT).AddBonus("test", 5f);
            Assert.AreEqual(295f, attribute.GetMaxMp(), 0.001f);
        }

        [Test]
        public void HpRegen_IncludesStrengthBonus()
        {
            // Base regen is 0.25, strength is 10, so total should be 0.25 + (10 * 0.03) = 0.55
            Assert.AreEqual(0.55f, attribute.HpRegen, 0.001f);
        }

        [Test]
        public void MpRegen_IncludesIntelligenceBonus()
        {
            // Base regen is 0.01, intelligence is 10, so total should be 0.01 + (10 * 0.04) = 0.41
            Assert.AreEqual(0.41f, attribute.MpRegen, 0.001f);
        }

        [Test]
        public void AttackSpeed_IncludesAgilityBonus()
        {
            // Base attack speed is 1.0, agility is 10, so total should be 1.0 + (10 * 0.02) = 1.2
            Assert.AreEqual(1.2f, attribute.AttackSpeed, 0.001f);
        }

        [Test]
        public void GetFloat_ReturnsCorrectValues()
        {
            Assert.AreEqual(10f, attribute.GetFloat(AttributeName.STRENGTH), 0.001f);
            Assert.AreEqual(10f, attribute.GetFloat(AttributeName.AGILITY), 0.001f);
            Assert.AreEqual(10f, attribute.GetFloat(AttributeName.INTELLIGENT), 0.001f);
            Assert.AreEqual(300f, attribute.GetFloat(AttributeName.MOVEMENT_SPEED), 0.001f);
        }

        [Test]
        public void SetFloat_UpdatesValues()
        {
            attribute.SetFloat(AttributeName.STRENGTH, 20f);
            Assert.AreEqual(20f, attribute.GetFloat(AttributeName.STRENGTH), 0.001f);
        }

        [Test]
        public void IncreaseFloat_AddsToExisting()
        {
            attribute.IncreaseFloat(AttributeName.STRENGTH, 5f);
            Assert.AreEqual(15f, attribute.GetFloat(AttributeName.STRENGTH), 0.001f);
        }

        [Test]
        public void IndexerProperty_GetAndSet()
        {
            Assert.AreEqual(10f, attribute[AttributeName.STRENGTH], 0.001f);
            
            attribute[AttributeName.STRENGTH] = 25f;
            Assert.AreEqual(25f, attribute[AttributeName.STRENGTH], 0.001f);
        }

        [Test]
        public void GetBaseFloat_ReturnsBaseFloatInstance()
        {
            var strengthBase = attribute.GetBaseFloat(AttributeName.STRENGTH);
            Assert.IsNotNull(strengthBase);
            Assert.AreEqual(10f, strengthBase.Total, 0.001f);
        }

        [Test]
        public void AddBaseFloat_CreatesNewAttributeIfNotExists()
        {
            var result = attribute.AddBaseFloat("CustomFloat", 42f);
            Assert.IsNotNull(result);
            Assert.AreEqual(42f, attribute.GetFloat("CustomFloat"), 0.001f);
        }

        [Test]
        public void SetBaseFloat_UpdatesOrCreatesAttribute()
        {
            var result = attribute.SetBaseFloat("NewFloat", 15f);
            Assert.IsNotNull(result);
            Assert.AreEqual(15f, attribute.GetFloat("NewFloat"), 0.001f);
        }
    }
}