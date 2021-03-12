using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour, IDebug
{
    private ISelectable _targetSelectable = default;
    private ISelectable _clickedElemente = default;

    public ISelectable TargetHoveredElement => _targetSelectable;
    private bool _isDebugModeActive = false;

    [SerializeField]
    private LayerMask _layerMask = default;

    public GenericEvent<EventArgs> SelectionClickedEvent = new GenericEvent<EventArgs>();
    public GenericEvent<EventArgs> SelectionUnclickedEvent = new GenericEvent<EventArgs>();

    private void Start()
    {
        GameManager.Instance?.AddObjToDebugList(this);
    }

    private void Update()
    {
        CheckInput();
        CalculateRayCast();
    }

    private void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && _targetSelectable != null)
        {
            TryDeselectLastClickedObj();

            if (_clickedElemente != null && _clickedElemente == _targetSelectable)
            {
                return;
            }

            _targetSelectable.Selectable();
            _clickedElemente = _targetSelectable;
            SelectionClickedEvent.InvokeEvent(new EventArgs(_clickedElemente));
        }
    }

    internal void SelectObj()
    {
        TryDeselectLastClickedObj();
        _targetSelectable.Selectable();
        _clickedElemente = _targetSelectable;
        SelectionClickedEvent.InvokeEvent(new EventArgs(_clickedElemente));
    }

    private void TryDeselectLastClickedObj()
    {
        if (_clickedElemente != null)// && _clickedElemente != _targetSelectable)
        {
            SelectionUnclickedEvent.InvokeEvent(new EventArgs(_clickedElemente));
            _clickedElemente.Deselectable();
            _clickedElemente = null;
        }
    }

    public void ResetSelection()
    {
        TryDeselectLastClickedObj();
    }

    private void CalculateRayCast()
    {
        RaycastHit[] elements = Physics.RaycastAll(GameManager.Instance.GetScreenToRay(), 150f, _layerMask.value);
        if (elements != null && elements.Length > 0)
        {
            Transform selection = elements[0].transform;
            CheckRayCastTarget(selection);

        }
        else
        {
            ResetTmpInstance();
        }
    }

    private void CheckRayCastTarget(Transform selection)
    {
        var targetInterface = selection.GetComponent<ISelectable>();
        if (targetInterface != null)
        {
            RayCastHitWithObj(targetInterface);
        }
        else
        {
            Debug.LogWarning("Interact Obj with Tag but can not find a interface called " + typeof(ISelectable).ToString());
        }
    }
    private void RayCastHitWithObj(ISelectable targetInterface)
    {
        if (_targetSelectable == null)
        {
            SaveAsNewInstance(targetInterface);
        }
        else if (targetInterface != _targetSelectable)
        {
            if (_targetSelectable != null)
            {
                _targetSelectable.EndHover();
            }

            SaveAsNewInstance(targetInterface);
        }
        else
        {
            SetDebugConsole("Same Obj");
        }
    }

    private void SaveAsNewInstance(ISelectable targetInterface)
    {
        _targetSelectable = targetInterface;
        targetInterface.StartHover();
        SetDebugConsole(targetInterface.GetDebugInformation());
    }

    public void SelectSelectable(ISelectable selectable)
    {
        _targetSelectable = selectable;
        SelectObj();
    }

    private void ResetTmpInstance()
    {
        if (_targetSelectable != null)
        {
            _targetSelectable.EndHover();
            _targetSelectable = null;
        }
    }

    private void SetDebugConsole(string txt)
    {
        if (_isDebugModeActive)
        {
            Debug.Log(txt);
        }
    }

    public int GetDebugID()
    {
        return 2;
    }

    public void DebugModeChanged(bool isDebug)
    {
        _isDebugModeActive = isDebug;
    }

    public struct EventArgs
    {
        public ISelectable CurrentSelectedElement;

        public EventArgs(ISelectable currentSelectedElement)
        {
            CurrentSelectedElement = currentSelectedElement;
        }
    }
}


