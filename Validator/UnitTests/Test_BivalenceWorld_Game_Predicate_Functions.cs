﻿using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Validator;
using Validator.Game;
using Validator.World;

namespace UnitTests
{
    [TestClass]
    public class Test_BivalenceWorld_Game_Predicate_Functions
    {
        [TestMethod]
        public void Game_Predicate_Function_True_GuessTrue()
        {
            BivalenceWorld world = new BivalenceWorld();
            List<string> sentences = new List<string>
            {
                    "Tet(lm(a))",
            };
            List<WorldObject> worldObjects = new List<WorldObject>
            {
                    new WorldObject(new List<string> {"a"}, new List<string> {BivalenceWorldDataFields.TET, BivalenceWorldDataFields.LARGE},
                            new List<object> {3, 3}),
            };
            WorldParameter parameter = new WorldParameter(worldObjects, sentences);
            var result = world.Check(parameter);

            Game game = new Game(sentences[0], world, true);

            var move = game.Play();

            Assert.IsTrue(move is InfoMessage);

            move = game.Play();

            Assert.IsTrue(move is InfoMessage);

            move = game.Play();

            Assert.IsTrue(move is EndMessage);

            var end = move as EndMessage;

            Assert.IsTrue(end.GuessWasRight);
        }

        [TestMethod]
        public void Game_Predicate_Function_False_GuessTrue()
        {
            BivalenceWorld world = new BivalenceWorld();
            List<string> sentences = new List<string>
            {
                    "Tet(lm(a))",
            };
            List<WorldObject> worldObjects = new List<WorldObject>
            {
                    new WorldObject(new List<string> {"a"}, new List<string> {BivalenceWorldDataFields.CUBE, BivalenceWorldDataFields.LARGE},
                            new List<object> {3, 3}),
            };
            WorldParameter parameter = new WorldParameter(worldObjects, sentences);
            var result = world.Check(parameter);

            Game game = new Game(sentences[0], world, true);

            var move = game.Play();

            Assert.IsTrue(move is InfoMessage);

            move = game.Play();

            Assert.IsTrue(move is InfoMessage);

            move = game.Play();

            Assert.IsTrue(move is EndMessage);

            var end = move as EndMessage;

            Assert.IsFalse(end.GuessWasRight);
        }

        [TestMethod]
        public void Game_Predicate_MultipleObjects_Function_True_GuessTrue()
        {
            BivalenceWorld world = new BivalenceWorld();
            List<string> sentences = new List<string>
            {
                    "Tet(lm(a))",
            };
            List<WorldObject> worldObjects = new List<WorldObject>
            {
                    new WorldObject(new List<string> {"a"}, new List<string> {BivalenceWorldDataFields.CUBE, BivalenceWorldDataFields.LARGE},
                            new List<object> {3, 3}),
                    new WorldObject(new List<string> {"b"}, new List<string> {BivalenceWorldDataFields.TET, BivalenceWorldDataFields.LARGE},
                            new List<object> {1, 3}),
            };
            WorldParameter parameter = new WorldParameter(worldObjects, sentences);
            var result = world.Check(parameter);

            Game game = new Game(sentences[0], world, true);

            var move = game.Play();

            Assert.IsTrue(move is InfoMessage);

            move = game.Play();

            Assert.IsTrue(move is InfoMessage);

            move = game.Play();

            Assert.IsTrue(move is EndMessage);

            var end = move as EndMessage;

            Assert.IsTrue(end.GuessWasRight);
        }

        [TestMethod]
        public void Game_Predicate_Parantheses_Function_True_GuessTrue()
        {
            BivalenceWorld world = new BivalenceWorld();
            List<string> sentences = new List<string>
            {
                    "Dodec(bm(lm(a)))",
            };
            List<WorldObject> worldObjects = new List<WorldObject>
            {
                    new WorldObject(new List<string> {"a"}, new List<string> {BivalenceWorldDataFields.CUBE, BivalenceWorldDataFields.LARGE},
                            new List<object> {3, 3}),
                    new WorldObject(new List<string> {"b"}, new List<string> {BivalenceWorldDataFields.TET, BivalenceWorldDataFields.LARGE},
                            new List<object> {1, 3}),
                    new WorldObject(new List<string> {"d"}, new List<string> {BivalenceWorldDataFields.DODEC, BivalenceWorldDataFields.LARGE},
                            new List<object> {1, 5}),
            };
            WorldParameter parameter = new WorldParameter(worldObjects, sentences);
            var result = world.Check(parameter);

            Game game = new Game(sentences[0], world, true);

            var move = game.Play();

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