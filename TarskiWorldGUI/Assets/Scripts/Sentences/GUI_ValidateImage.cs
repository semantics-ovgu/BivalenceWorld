using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Validator.World;

public class GUI_ValidateImage : MonoBehaviour
{
    [SerializeField]
    private Image _targetImage = default;

    [SerializeField]
    private Color _wrongColor = default;

    [SerializeField]
    private Color _rightColor = default;
    [SerializeField]
    private Color _wrongParseColor = default;



    public void SetColor(EValidationResult isCorrect)
    {
        ActivateImage(true);
        if (isCorrect == EValidationResult.False)
        {
            SetColorToImage(_wrongColor);
        }
        else if (isCorrect == EValidationResult.True)
        {
            SetColorToImage(_rightColor);
        }
    }

    public void ActivateImage(bool isActiv)
    {
        _targetImage.gameObject.SetActive(isActiv);
    }

    private void SetColorToImage(Color color)
    {
        _targetImage.color = color;
    }

    internal void ParseError()
    {
        SetColorToImage(_wrongParseColor);
        ActivateImage(true);
    }
}
