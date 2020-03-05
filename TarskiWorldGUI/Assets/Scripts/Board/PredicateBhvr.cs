using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredicateBhvr : MonoBehaviour
{
    [SerializeField]
    private float _scaleFactor = 2.0f;

    private PredicateObj _targetObj = default;

    public void Create(PredicateObj obj)
    {
        _targetObj = obj;
        ChangeScaleFactor(_scaleFactor);
    }

    public void Undo()
    {
        _targetObj.GetVisual().transform.localScale = Vector3.one * _targetObj.GetDefaultSize();
    }

    private void ChangeScaleFactor(float value)
    {
        _targetObj.GetVisual().transform.localScale = Vector3.one * _targetObj.GetDefaultSize() * value;
    }
}
