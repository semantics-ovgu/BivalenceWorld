﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConstantButton : GUI_Button
{
    [SerializeField]
    private string _constant = "";
    [SerializeField]
    private TMPro.TextMeshProUGUI _targetText = default;
    private IConstant _instance = default;


    private void OnValidate()
    {
        if (_targetText != null)
        {
            _targetText.text = _constant;
        }
    }

    private void Start()
    {
        var manager = GameManager.Instance;

        if (manager != null)
        {
            manager.GetSelectionManager().SelectionClickedEvent.AddEventListener(SelectionClickedListener);
            manager.GetSelectionManager().SelectionUnclickedEvent.AddEventListener(SelectionUnclickedListener);
        }
    }

    public override void ButtonUnselect()
    {
        base.ButtonUnselect();
        ValidateOverlay();
    }

    private void ValidateOverlay()
    {
        var board = GameManager.Instance.GetCurrentBoard();
        if (_instance != null)
        {
            if (board.IsConstantUsed(_constant) && !_instance.GetConstantsList().Contains(_constant))
            {
                _button.interactable = false;
                _activeOverlay.gameObject.SetActive(false);
            }
            else
            {
                _button.interactable = true;
                if (_instance.GetConstantsList() != null && _instance.GetConstantsList().Contains(_constant))
                {
                    _activeOverlay.gameObject.SetActive(true);
                }
                else
                {
                    _activeOverlay.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            _activeOverlay.gameObject.SetActive(false);
        }
    }

    protected override void ButtonClickedListener()
    {
        if (_instance != null)
        {
            IConstant targetInstanceWithConstant = null;
            if (((Field)_instance).HasPredicateInstance())
            {
                targetInstanceWithConstant = TryDeleteConstant();
            }

            if (targetInstanceWithConstant == null)
            {
                _instance.AddConstant(_constant);
            }
            else if (targetInstanceWithConstant != null && targetInstanceWithConstant != _instance)
            {
                _instance.AddConstant(_constant);
            }

            ValidateOverlay();
        }
        else
        {
            Debug.LogWarning("Instance is null");
        }
    }

    private IConstant TryDeleteConstant()
    {
        var instance = GameManager.Instance;
        if (instance)
        {
            IConstant fieldobj = instance.GetCurrentBoard().GetBoardWithTargetConstant(_constant);
            if (fieldobj != null)
            {
                fieldobj.RemoveConstant(_constant);
            }
            return fieldobj;
        }
        return null;
    }

    private void SelectionUnclickedListener(SelectionManager.EventArgs arg0)
    {
        _instance = null;

        ValidateOverlay();
    }

    private void SelectionClickedListener(SelectionManager.EventArgs arg0)
    {
        if (arg0.CurrentSelectedElement != null)
        {
            var element = arg0.CurrentSelectedElement.GetRootObj().GetComponent<IConstant>();
            if (element != null)
            {
                _instance = element;
            }
        }

        ValidateOverlay();
    }
}
