using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentenceButton : ASentenceButton
{
    [SerializeField]
    private string _sentence = "";


    protected override void ButtonClickedListener()
    {
        SpaceBeforeText();

        var inputTextField = GameManager.Instance.GetTextInputField().CurrentTextInputElement?.InputField;

        if (inputTextField != null)
        {
            GameManager.Instance.GetTextInputField().CurrentTextInputElement?.AddText(GetInsertString());

            var maxPosition = inputTextField.text.Length;
            inputTextField.caretPosition = maxPosition - (_sentence.Length - ParantheseOpenCharPosition()) + 1;
        }

        SpaceAfterText();
    }

    private int ParantheseOpenCharPosition()
    {
        var chars = _sentence.ToCharArray();
        for (int i = 0; i < chars.Length; i++)
        {
            if (chars[i] == '(')
            {
                return i;
            }
        }

        return _sentence.Length;
    }

    private string GetInsertString()
    {
        return _sentence;
    }

    protected override string GetDisplayString()
    {
        return _sentence.Split('(')[0];
    }
}
