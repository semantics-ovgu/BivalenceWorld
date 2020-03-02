using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        var list = manager.GetTextInputField().GetGuiTextElementsWithText();
        foreach (GUI_TextInputElement item in list)
        {
            resultSentences.Add(item.GetInputText());
            Debug.Log(item.GetInputText());
        }

        var board = manager.GetCurrentBoard();

        var obj = board.GetFieldElements();
        foreach (var item in obj)
        {
            if (item.HasPredicateInstance())
            {
                //ToDo: Create new Datastruct for Validator
            }
        }

        //Conclusion

        foreach (var item in list)
        {
            item.Validate(true);
        }


    }
}
