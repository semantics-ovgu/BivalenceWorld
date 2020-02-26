using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentenceButton : ASentenceButton
{
    [SerializeField]
    private string _sentence = "";

    protected override void ButtonClickedListener()
    {
        GameManager.Instance.GetTextInputField().CurrentTextInputElement?.AddText(GetDisplayString());
    }

    protected override string GetDisplayString()
    {
        return _sentence;
    }
}
