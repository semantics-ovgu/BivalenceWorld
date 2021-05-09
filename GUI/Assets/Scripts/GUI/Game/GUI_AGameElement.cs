using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Validator.Game;

public class GUI_AGameElement : MonoBehaviour
{
    public GenericEvent<EventArgs> FinishedMoveEvent = new GenericEvent<EventArgs>();
    private AMove _move = default;
    [SerializeField]
    private Button _backButton = default;

    protected virtual void OnFinishedMoveEvent(EventArgs args)
    {
        FinishedMoveEvent.InvokeEvent(args);
        GameManager.Instance.GetValidation().SetPresentationLayout();
    }

    public class EventArgs
    {

    }

    public virtual void Init(AMove move)
    {
        _move = move;
    }
}
