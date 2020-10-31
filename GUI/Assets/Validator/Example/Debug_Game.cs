using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Validator;
using Validator.Game;

public class Debug_Game : MonoBehaviour
{
    // Start is called before the first frame update
    public void Start()
    {
        BivalenceWorld world = new BivalenceWorld();
        List<string> sentences = new List<string>
            {
                "Tet(a) ∧ Tet(b) ∧ Tet(c)",
            };
        List<WorldObject> worldObjects = new List<WorldObject>
            {
                new WorldObject(new List<string> { "a" }, new List<string> {"Tet", "Large" }, new List<object> {1, 3 }),
                new WorldObject(new List<string> { "b" }, new List<string> {"Tet", "Large" }, new List<object> {2, 3 }),
                new WorldObject(new List<string> { "c" }, new List<string> {"Tet", "Large"}, new List<object> {3, 3 })
            };
        WorldParameter parameter = new WorldParameter(worldObjects, sentences);
        var result = world.Check(parameter);

        Game game = new Game(sentences[0], world, false);

        var move = game.Play();

        move = game.Play(); //Information prefab

        move = game.Play();

        var question = move as Question; //andere Prefab spawnen bei Question
        question.SetAnswers(question.PossibleAnswers[0]); //anpassen an auswahlten an Prefab 2 - n


        move = game.Play();

        move = game.Play();

        var end = move as EndMessage;
    }
}
