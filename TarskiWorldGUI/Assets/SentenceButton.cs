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
        GameManager.Instance.GetTextInputField().CurrentTextInputElement?.AddText(GetDisplayString());
        SpaceAfterText();
    }

    protected override string GetDisplayString()
    {
        return _sentence;
    }
}
