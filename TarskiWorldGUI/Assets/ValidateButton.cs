using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Validator;

public class ValidateButton : MonoBehaviour
{
    [SerializeField]
    private Button _targetButton = default;

    private void Awake()
    {
        _targetButton.onClick.AddListener(ButtonClickedListener);
    }

    private void ButtonClickedListener()
    {
        var manager = GameManager.Instance;
        if (manager == null)
            return;

        var resultSentences = new List<string>();
        List<GUI_TextInputElement> list = manager.GetTextInputField().GetGuiTextElementsWithText();
        foreach (GUI_TextInputElement item in list)
        {
            resultSentences.Add(item.GetInputText());
            Debug.Log(item.GetInputText());
        }

        var board = manager.GetCurrentBoard();

        var obj = board.GetFieldElements();
        List<WorldObject> worldObjs = new List<WorldObject>();
        foreach (Field item in obj)
        {

            if (item.HasPredicateInstance())
            {
                Debug.Log("--- New Element --- ");
                List<Predicate> predicates = item.GetPredicatesList();
                var constant = item.GetConstantsList();
                List<string> worldPredicates = new List<string>();
                foreach (var pred in predicates)
                {
                    Debug.Log(pred.PredicateIdentifier);
                    worldPredicates.Add(pred.PredicateIdentifier);
                }
                foreach (var co in constant)
                {
                    Debug.Log(co);
                }
                List<object> coord = new List<object>();
                coord.Add(item.GetX());
                coord.Add(item.GetZ());
                worldObjs.Add(new WorldObject(constant, worldPredicates, coord));



            }
        }

        TarskiWorld world = new TarskiWorld();

        WorldResult<bool> result = world.Check(new WorldParameter(worldObjs, resultSentences));

        //Conclusion

        for (int i = 0; i < result.Result.Value.Count; i++)
        {
            Result<bool> item = result.Result.Value[i];
            list[i].Validate(item.IsValid);
        }
    }
}
