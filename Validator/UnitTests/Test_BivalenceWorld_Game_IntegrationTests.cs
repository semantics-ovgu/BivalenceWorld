using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Validator;
using Validator.Game;
using Validator.World;

namespace UnitTests
{
    [TestClass]
    public class Test_BivalenceWorld_Game_IntegrationTests
    {
        [TestMethod]
        public void Game_Implication_True_GuessFalse()
        {
            BivalenceWorld world = new BivalenceWorld();
            List<string> sentences = new List<string>
            {
                "(Tet(a)∨Cube(a)) → (Small(a) ∧ LeftOf(a,b))",
            };
            List<WorldObject> worldObjects = new List<WorldObject>
            {
                new WorldObject(new List<string> { "a" }, new List<string> {BivalenceWorldDataFields.DODEC, BivalenceWorldDataFields.LARGE }, new List<object> {1, 3 }),
                new WorldObject(new List<string> { "b" }, new List<string> {BivalenceWorldDataFields.CUBE, BivalenceWorldDataFields.LARGE }, new List<object> {3, 3 })
            };
            WorldParameter parameter = new WorldParameter(worldObjects, sentences);
            var result = world.Check(parameter);

            Game game = new Game(sentences[0], world, true);

            var move = game.Play();

            Assert.IsTrue(move is InfoMessage);

            move = game.Play();

            Assert.IsTrue(move is InfoMessage);

            move = game.Play();

            Assert.IsTrue(move is InfoMessage);

            move = game.Play();

            Assert.IsTrue(move is Question);

            var question = move as Question;
            Assert.IsTrue(question.PossibleAnswers.Count == 2);
            question.SetAnswers(question.PossibleAnswers[0]);

            move = game.Play();

            Assert.IsTrue(move is InfoMessage);

            move = game.Play();

            Assert.IsTrue(move is InfoMessage);

            move = game.Play();

            Assert.IsTrue(move is InfoMessage);

            move = game.Play();

            Assert.IsTrue(move is InfoMessage);

            move = game.Play();

            Assert.IsTrue(move is EndMessage);

            var end = move as EndMessage;

            Assert.IsTrue(end.GuessWasRight);
        }
    }
}
