﻿using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Validator;
using Validator.Game;
using Validator.World;

namespace UnitTests
{
    [TestClass]
    public class Test_BivalenceWorld_Game_ExistQuantum
    {
        [TestMethod]
        public void Game_ExistQuantum_True_GuessTrue_RightSelection()
        {
            BivalenceWorld world = new BivalenceWorld();
            List<string> sentences = new List<string>
            {
                "\u2203 x Cube(x)"
            };
            List<WorldObject> worldObjects = new List<WorldObject>
            {
                new WorldObject(new List<string> { "a" }, new List<string> {BivalenceWorldDataFields.CUBE, BivalenceWorldDataFields.LARGE }, new List<object> {1, 3 }),
                new WorldObject(new List<string> { "c" }, new List<string> {BivalenceWorldDataFields.TET, BivalenceWorldDataFields.LARGE }, new List<object> {3, 3 })
            };
            WorldParameter parameter = new WorldParameter(worldObjects, sentences);
            var result = world.Check(parameter);

            Game game = new Game(sentences[0], world, true);

            var move = game.Play();

            Assert.IsTrue(move is InfoMessage);

            move = game.Play();

            Assert.IsTrue(move is InfoMessage);

            move = game.Play();

            Assert.IsTrue(move is Question);

            var question = move as Question;
            question.SetAnswers(question.PossibleAnswers[0]);

            move = game.Play();

            Assert.IsTrue(move is InfoMessage);

            move = game.Play();

            Assert.IsTrue(move is EndMessage);

            var end = move as EndMessage;

            Assert.IsTrue(end.GuessWasRight);
        }

        [TestMethod]
        public void Game_ExistQuantum_False_GuessTrue()
        {
            BivalenceWorld world = new BivalenceWorld();
            List<string> sentences = new List<string>
            {
                    "\u2203 x Cube(x)"
            };
            List<WorldObject> worldObjects = new List<WorldObject>
            {
                    new WorldObject(new List<string> { "a" }, new List<string> {BivalenceWorldDataFields.TET, BivalenceWorldDataFields.LARGE }, new List<object> {1, 3 }),
                    new WorldObject(new List<string> { "c" }, new List<string> {BivalenceWorldDataFields.TET, BivalenceWorldDataFields.LARGE }, new List<object> {3, 3 })
            };
            WorldParameter parameter = new WorldParameter(worldObjects, sentences);
            var result = world.Check(parameter);

            Game game = new Game(sentences[0], world, true);

            var move = game.Play();

            Assert.IsTrue(move is InfoMessage);

            move = game.Play();

            Assert.IsTrue(move is InfoMessage);

            move = game.Play();

            Assert.IsTrue(move is Question);

            var question = move as Question;
            question.SetAnswers(question.PossibleAnswers[0]);

            move = game.Play();

            Assert.IsTrue(move is InfoMessage);

            move = game.Play();

            Assert.IsTrue(move is EndMessage);

            var end = move as EndMessage;

            Assert.IsFalse(end.GuessWasRight);
        }

        [TestMethod]
        public void Game_ExistQuantum_True_GuessTrue_WrongSelection()
        {
            BivalenceWorld world = new BivalenceWorld();
            List<string> sentences = new List<string>
            {
                    "\u2203 x Cube(x)"
            };
            List<WorldObject> worldObjects = new List<WorldObject>
            {
                    new WorldObject(new List<string> { "a" }, new List<string> {BivalenceWorldDataFields.TET, BivalenceWorldDataFields.LARGE }, new List<object> {1, 3 }),
                    new WorldObject(new List<string> { "c" }, new List<string> {BivalenceWorldDataFields.TET, BivalenceWorldDataFields.LARGE }, new List<object> {3, 3 })
            };
            WorldParameter parameter = new WorldParameter(worldObjects, sentences);
            var result = world.Check(parameter);

            Game game = new Game(sentences[0], world, true);

            var move = game.Play();

            Assert.IsTrue(move is InfoMessage);

            move = game.Play();

            Assert.IsTrue(move is InfoMessage);

            move = game.Play();

            Assert.IsTrue(move is Question);

            var question = move as Question;
            question.SetAnswers(question.PossibleAnswers[1]);

            move = game.Play();

            Assert.IsTrue(move is InfoMessage);

            move = game.Play();

            Assert.IsTrue(move is EndMessage);

            var end = move as EndMessage;

            Assert.IsFalse(end.GuessWasRight);
        }

        [TestMethod]
        public void Game_ExistQuantum_False_GuessFalse()
        {
            BivalenceWorld world = new BivalenceWorld();
            List<string> sentences = new List<string>
            {
                    "\u2203 x Cube(x)"
            };
            List<WorldObject> worldObjects = new List<WorldObject>
            {
                    new WorldObject(new List<string> { "a" }, new List<string> {BivalenceWorldDataFields.TET, BivalenceWorldDataFields.LARGE }, new List<object> {1, 3 }),
                    new WorldObject(new List<string> { "c" }, new List<string> {BivalenceWorldDataFields.TET, BivalenceWorldDataFields.LARGE }, new List<object> {3, 3 })
            };
            WorldParameter parameter = new WorldParameter(worldObjects, sentences);
            var result = world.Check(parameter);

            Game game = new Game(sentences[0], world, false);

            var move = game.Play();

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

        [TestMethod]
        public void Game_ExistQuantum_True_GuessFalse()
        {
            BivalenceWorld world = new BivalenceWorld();
            List<string> sentences = new List<string>
            {
                    "\u2203 x Cube(x)"
            };
            List<WorldObject> worldObjects = new List<WorldObject>
            {
                    new WorldObject(new List<string> { "a" }, new List<string> {BivalenceWorldDataFields.TET, BivalenceWorldDataFields.LARGE }, new List<object> {1, 3 }),
                    new WorldObject(new List<string> { "c" }, new List<string> {BivalenceWorldDataFields.CUBE, BivalenceWorldDataFields.LARGE }, new List<object> {3, 3 })
            };
            WorldParameter parameter = new WorldParameter(worldObjects, sentences);
            var result = world.Check(parameter);

            Game game = new Game(sentences[0], world, false);

            var move = game.Play();

            Assert.IsTrue(move is InfoMessage);

            move = game.Play();

            Assert.IsTrue(move is InfoMessage);

            move = game.Play();

            Assert.IsTrue(move is InfoMessage);

            move = game.Play();

            Assert.IsTrue(move is EndMessage);

            var end = move as EndMessage;

            Assert.IsFalse(end.GuessWasRight);
        }
    }
}
