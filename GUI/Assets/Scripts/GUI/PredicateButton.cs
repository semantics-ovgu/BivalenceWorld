using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PredicateButton : GUI_Button
{
    [SerializeField]
    private Predicate _predicate = default;
    private IPredicate _instance = default;

    private void Start()
    {
        var manager = GameManager.Instance;

        if (manager != null)
        {
            manager.GetSelectionManager().SelectionClickedEvent.AddEventListener(SelectionClickedListener);
            manager.GetSelectionManager().SelectionUnclickedEvent.AddEventListener(SelectionUnclickedListener);
        }
    }

    protected override void ButtonClickedListener()
    {
        _instance?.AddPredicate(_predicate);

        if (_instance != null)
        {
            if (_instance.GetPredicatesList() != null && _instance.GetPredicatesList().Contains(_predicate))
            {
                _activeOverlay.gameObject.SetActive(true);
            }
            else
            {
                _activeOverlay.gameObject.SetActive(false);
            }
        }
    }

    private void SelectionUnclickedListener(SelectionManager.EventArgs arg0)
    {
        _instance = null;

        ValidateOverlay();
    }

    public override void ButtonUnselect()
    {
        base.ButtonUnselect();

        ValidateOverlay();
    }

    private void ValidateOverlay()
    {
        if (_instance != null && _instance.GetPredicatesList() != null && _instance.GetPredicatesList().Contains(_predicate))
        {
            _activeOverlay.gameObject.SetActive(true);
        }
        else
        {
            _activeOverlay.gameObject.SetActive(false);
        }
    }

    private void SelectionClickedListener(SelectionManager.EventArgs arg0)
    {
        if (arg0.CurrentSelectedElement != null)
        {
            var element = arg0.CurrentSelectedElement.GetRootObj().GetComponent<IPredicate>();
            if (element != null)
            {
                _instance = element;
            }
        }
        ValidateOverlay();
    }
}
