using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Validator;
using Validator.World;

namespace UnitTests.Test_DiceGame
{
    [TestClass]
    public class Test_DiceGame_Performance
    {
        [TestMethod]
        public void Test_Performance_LongSentence()
        {
            List<WorldObject> worldObjects = new List<WorldObject>
            {
                new WorldObject(new List<string> { "1" }, new List<string> {GameWorldFields.DICEONE , GameWorldFields.SELECTED }, new List<object>()),
                new WorldObject(new List<string> { "2" }, new List<string> {GameWorldFields.DICEONE , GameWorldFields.NOTSELECTED }, new List<object>()),
                new WorldObject(new List<string> { "3" }, new List<string> {GameWorldFields.DICETWO , GameWorldFields.NOTSELECTED }, new List<object>()),
                new WorldObject(new List<string> { "4" }, new List<string> {GameWorldFields.DICETHREE , GameWorldFields.NOTSELECTED }, new List<object>()),
                new WorldObject(new List<string> { "5" }, new List<string> {GameWorldFields.DICEFOUR , GameWorldFields.NOTSELECTED }, new List<object>()),
                new WorldObject(new List<string> { "6" }, new List<string> {GameWorldFields.DICESIX , GameWorldFields.NOTSELECTED }, new List<object>())
            };

            AWorld world = new GameWorld();
            var result = world.Check(new WorldParameter(worldObjects, GameRuleSet.GetRule(GameRuleSet.ERule.SelectionPossible)));

            Assert.IsTrue(result.Result.Value[0].Value == EValidationResult.True);
        }
    }
}
