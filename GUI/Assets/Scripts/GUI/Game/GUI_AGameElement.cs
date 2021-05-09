using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Validator.Game;

public abstract class GUI_AGameElement : MonoBehaviour
{
    public GenericEvent<EventArgs> FinishedMoveEvent = new GenericEvent<EventArgs>();
    private AMove _move = default;
    [SerializeField]
    private Button _backButton = default;

    public GenericEvent<GUI_AGameElement> OnBackButtonClickedEvent = new GenericEvent<GUI_AGameElement>();

    protected virtual void OnFinishedMoveEvent(EventArgs args)
    {
        FinishedMoveEvent.InvokeEvent(args);
        GameManager.Instance.GetValidation().SetPresentationLayout();
        _backButton.interactable = false;
    }

    public virtual void EnableBackButton()
    {
        _backButton.interactable = true;
    }

    public virtual void ResetStep(AMove move)
    {
        
    }

    public class EventArgs
    {

    }

    public virtual void Init(AMove move)
    {
        _move = move;
        _backButton.onClick.AddListener(() => OnBackButtonClickedEvent.InvokeEvent(this));
    }
}
