using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Misc/FloatVar")]
public class FloatVar : ScriptableObject
{

    public enum EOperator { Set, Add, Sub, Mult, Div }

    [Header(nameof(FloatVar))]
    [SerializeField]
    private float _currentValue = 0.0f;
    public float CurrentValue => _currentValue;


    public GenericEvent<EventArgs> ValueChangedEvent = new GenericEvent<EventArgs>();

    public void ResetValue()
    {
        ResetValue(0.0f);
    }
    public void ResetValue(float resetValue)
    {
        _currentValue = resetValue;
    }

    public void ForceChangedEvent()
    {
        if (Application.isPlaying)
        {
            SetValueIntern(_currentValue, _currentValue, EOperator.Set, this);
        }
    }

    private void SetValueIntern(float newValue, float operatorValue, EOperator op, object context)
    {
        float oldValue = _currentValue;
        _currentValue = newValue;


        ValueChangedEvent.InvokeEvent(new EventArgs(oldValue, _currentValue, this));
    }

    public void Operate(EOperator op, float value, object context)
    {
        float newValue = _currentValue;

        switch (op)
        {
            case EOperator.Set:
                newValue = value;
                break;

            case EOperator.Add:
                newValue += value;
                break;

            case EOperator.Sub:
                newValue -= value;
                break;

            case EOperator.Mult:
                newValue *= value;
                break;

            case EOperator.Div:
                newValue /= value;
                break;

            default:
                Debug.Assert(false, "Unknown enum: " + op);
                break;
        }

        SetValueIntern(newValue, value, op, context);
    }



    public struct EventArgs
    {
        public float OldValue;
        public float NewValue;
        public FloatVar Float;

        public EventArgs(float oldValue, float value, FloatVar scriptableFloat)
        {
            OldValue = oldValue;
            NewValue = value;
            Float = scriptableFloat;
        }
    }
}
