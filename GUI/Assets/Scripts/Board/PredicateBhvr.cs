using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredicateBhvr : MonoBehaviour
{
    [SerializeField]
    private float _scaleFactor = 2.0f;

    private PredicateObj _targetObj = default;
    private GameObject _scaleObj = default;

    public void Create(PredicateObj obj, GameObject scaleObj)
    {
        _scaleObj = scaleObj;
        _targetObj = obj;
        ChangeScaleFactor(_scaleFactor);
    }

    public void Undo()
    {
        _scaleObj.transform.localScale = Vector3.one * _targetObj.GetDefaultSize();
    }

    private void ChangeScaleFactor(float value)
    {
        _scaleObj.transform.localScale = Vector3.one * _targetObj.GetDefaultSize() * value;
        _scaleObj.transform.parent.gameObject.transform.position = new Vector3(_scaleObj.gameObject.transform.position.x, 0, _scaleObj.gameObject.transform.position.z);
    }
}
