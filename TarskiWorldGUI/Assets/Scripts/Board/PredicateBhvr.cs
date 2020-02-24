﻿using System.Collections;
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
        ChangeScaleFactor(_targetObj.GetDefaultSize());
    }

    private void ChangeScaleFactor(float value)
    {
        _targetObj.GetVisuel().transform.localScale = Vector3.one * value;
    }
}
