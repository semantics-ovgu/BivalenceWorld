using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableObject : MonoBehaviour, ISelectable
{

    public GenericEvent<SelectableArgs> HoverStartEvent = new GenericEvent<SelectableArgs>();
    public GenericEvent<SelectableArgs> HoverEndEvent = new GenericEvent<SelectableArgs>();
    public GenericEvent<SelectableArgs> SelectedStartEvent = new GenericEvent<SelectableArgs>();
    public GenericEvent<SelectableArgs> SelectedEndEvent = new GenericEvent<SelectableArgs>();

    [SerializeField]
    private bool _isDebuMode = false;

    public string GetDebugInformation()
    {
        return this.gameObject.name;
    }

    public void StartHover()
    {
        SetDebugText("StartHover");
        HoverStartEvent.InvokeEvent(new SelectableArgs());
    }

    public void EndHover()
    {
        SetDebugText("EndHover");
        HoverEndEvent.InvokeEvent(new SelectableArgs());
    }

    public void Selectable()
    {
        SetDebugText("Selectable");
        SelectedStartEvent.InvokeEvent(new SelectableArgs());
    }

    public void Deselectable()
    {
        SetDebugText("NONSelectable");
        SelectedEndEvent.InvokeEvent(new SelectableArgs());
    }

    private void SetDebugText(string txt)
    {
        if (_isDebuMode)
            Debug.Log(txt);
    }

    public GameObject GetRootObj()
    {
        return this.gameObject;
    }

    public class SelectableArgs : EventArgs
    {

    }
}
