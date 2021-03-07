using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GUI_Button : MonoBehaviour
{
    [SerializeField]
    protected Button _button = default;
    [SerializeField]
    protected Image _activeOverlay = default;

    public GenericEvent<GUI_Button> OnButtonSelectEvent = new GenericEvent<GUI_Button>();

    private void OnValidate()
    {
        if (_button == null)
            this.gameObject.GetComponent<Button>();
    }

    private void Awake()
    {
        _button.onClick.AddListener(ButtonClickedListener);
        _button.onClick.AddListener(() => OnButtonSelectEvent.InvokeEvent(this));
    }

    protected virtual void ButtonClickedListener()
    {

    }

    public virtual void ButtonUnselect()
    {

    }
}
