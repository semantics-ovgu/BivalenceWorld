using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI_TrashButton : GUI_Button
{

    protected override void ButtonClickedListener()
    {
        GameManager.Instance.GetTextInputField().CurrentTextInputElement?.RemoveText();
    }
}
