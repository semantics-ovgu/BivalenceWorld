using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Utilities;
using UnityEngine;
using UnityEngine.UI;
using Validator;
using Validator.World;

public class GUI_ValidateImage : MonoBehaviour
{
    [SerializeField]
    private Image _targetImage = default;
    [SerializeField]
    private ValidationIconSettings _iconContainer = default;
    [SerializeField]
    private ShowTooltip _tooltip = default;

    private void Start()
    {
        _tooltip.ResetToolTipText();
    }

    public void SetValidationResult(Result<EValidationResult> validateResult)
    {
        ActivateImage(true);

        var obj = _iconContainer.GetValidationContainer(validateResult.Value);
        if (obj != null)
        {
            SetSpriteToImage(obj.Sprite);
            _tooltip.SetToolTipText(validateResult.ErrorMessage);
        }
        else
        {
            Debug.LogError("Can not find type: " + validateResult.ToString());
        }
    }

    public void ActivateImage(bool isActiv)
    {
        _targetImage.gameObject.SetActive(isActiv);
        if (isActiv)
        {
            _tooltip.ResetToolTipText();
        }
    }

    private void SetSpriteToImage(Sprite sprite)
    {
        _targetImage.sprite = sprite;
    }
}
