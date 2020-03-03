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
    [SerializeField]
    private TMPro.TextMeshProUGUI _targetText = default;
    private IConstant _instance = default;
    private IConstant _boardWithCurrentConstant = default;

    private void OnValidate()
    {
        if(_targetText != null)
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
        if (_targetButton)
        {
            _targetButton.onClick.AddListener(ButtonClicked);
        }

    }

    private void ButtonClicked()
    {
        if (_instance != null)
        {
            if(((Field)_instance).HasPredicateInstance())
                TryDelteConstant();

            _instance.AddConstant(_constant);
        }
        else
        {
            Debug.LogWarning("Instance is null");
        }
    }

    private void TryDelteConstant()
    {
        var instance = GameManager.Instance;
        if (instance)
        {
            var fieldobj = instance.GetCurrentBoard().GetBoardWithTargetConstant(_constant);
            if (fieldobj != null)
            {
                fieldobj.RemoveConstant(_constant);
            }
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
