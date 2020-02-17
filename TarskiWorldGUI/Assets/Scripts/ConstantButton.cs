using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConstantButton : MonoBehaviour
{
    [SerializeField]
    private Button _targetButton = default;
    [SerializeField]
    private string _constant = "";
    private IConstant _instance = default;

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
        if(_instance != null)
        {
            _instance.AddConstant(_constant);
        }
        else
        {
            Debug.LogWarning("Instance is null");
        }
    }

    private void SelectionUnclickedListener(SelectionManager.EventArgs arg0)
    {
        _instance = null;
    }

    private void SelectionClickedListener(SelectionManager.EventArgs arg0)
    {
        var element = arg0.CurrentSelectedElement.GetRootObj().GetComponent<IConstant>();
        if (element != null)
        {
            _instance = element;
        }
    }
}
