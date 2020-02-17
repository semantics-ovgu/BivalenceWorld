using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectable_ABhvr : MonoBehaviour
{
    [SerializeField]
    private SelectableObject _selectableObj = default;

    private void Awake()
    {
        _selectableObj.HoverEndEvent.AddEventListener(EndHoveredListener);
        _selectableObj.HoverStartEvent.AddEventListener(StartHoveredListener);
        _selectableObj.SelectedEndEvent.AddEventListener(EndSelectedListener);
        _selectableObj.SelectedStartEvent.AddEventListener(StartSelectedListener);
        OnAwake();
    }

    protected virtual void OnAwake() { }
    protected virtual void StartHoveredListener(SelectableObject.SelectableArgs arg0) {}

    protected virtual void EndHoveredListener(SelectableObject.SelectableArgs arg0) {}

    protected virtual void StartSelectedListener(SelectableObject.SelectableArgs arg0) { }

    protected virtual void EndSelectedListener(SelectableObject.SelectableArgs arg0) { }

}
