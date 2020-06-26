using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PredicateButton : MonoBehaviour
{
    [SerializeField]
    private Button _targetButton = default;
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
        if (_targetButton)
        {
            _targetButton.onClick.AddListener(ButtonClicked);
        }
    }

    private void ButtonClicked()
    {
        _instance?.AddPredicate(_predicate);
    }

    private void SelectionUnclickedListener(SelectionManager.EventArgs arg0)
    {
        _instance = null;
    }

    private void SelectionClickedListener(SelectionManager.EventArgs arg0)
    {
        var element = arg0.CurrentSelectedElement.GetRootObj().GetComponent<IPredicate>();
        if (element != null)
        {
            _instance = element;
        }
    }

}

