using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUI_Button_RotateBoard : GUI_Button
{
    [SerializeField]
    private bool _rotateClockwise = true;

    protected override void ButtonClickedListener()
    {
        base.ButtonClickedListener();

        if (GameManager.Instance.GetCurrentBoard() != null)
        {
            GameManager.Instance.GetCurrentBoard().RotateMap(_rotateClockwise);
        }
    }
}
