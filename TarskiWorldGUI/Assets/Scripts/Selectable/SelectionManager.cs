using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SelectionManager;

public class SelectionManager : MonoBehaviour, IDebug
{
    [SerializeField]
    private Camera _targetCamera = default;
    private string _selectableTag = "Selectable";
    private ISelectable _targetSelectable = default;
    private ISelectable _clickedElemente = default;
    private bool _isDebugModeActive = false;

    public GenericEvent<EventArgs> SelectionClickedEvent = new GenericEvent<EventArgs>();
    public GenericEvent<EventArgs> SelectionUnclickedEvent = new GenericEvent<EventArgs>();

    private void Awake()
    {
        if(_targetCamera == null)
        {
            _targetCamera = Camera.main;
        }
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

    private void TryDeselectLastClickedObj()
    {
        if (_clickedElemente != null && _clickedElemente != _targetSelectable)
        {
            SelectionUnclickedEvent.InvokeEvent(new EventArgs(_clickedElemente));
            _clickedElemente.Deselectable();
        }
    }

    private void CalculateRayCast()
    {
        var ray = _targetCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Transform selection = hit.transform;
            if (selection.CompareTag(_selectableTag))
            {
                CheckRayCastTarget(selection);
            }
            else
            {
                ResetTmpInstance();
            }
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
            Debug.LogWarning("Interact Obj with Tag " + _selectableTag + ", but can not find a interface called " + typeof(ISelectable).ToString());
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
            Debug.Log(txt);
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


