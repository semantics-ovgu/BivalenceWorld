using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    // X Y
    // Konstanten (Unendlich A - Z)
    // Predicate (Tet und BIG -> Liste)

    [SerializeField]
    private Transform _anchor = default;

    [SerializeField]
    private GameObject _obj = default;

    [SerializeField]
    private GameObject _worldCanvasPrefab = default;

    private List<GameObject> _constant = new List<GameObject>();
    private List<GameObject> _predicate = new List<GameObject>();
    
    public void SpawnDefaultElement()
    {
        var instance = Instantiate(_obj, _anchor);
        _predicate.Add(instance);
    }

    public void DeleteObjs()
    {
        for (int i = _constant.Count -1 ; i >= 0; i--)
        {
            DestroyImmediate(_constant[i]);
        }
    }

    public void SpawnText(string constant)
    {
        if(_predicate.Count > 0)
        {
            GameObject obj = _predicate[0];
           
            GameObject canvas = Instantiate(_worldCanvasPrefab, obj.transform.position, Quaternion.identity, obj.transform);
            canvas.transform.localPosition = new Vector3(0, 1, 0);
            canvas.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = constant;
        }
    }
}
