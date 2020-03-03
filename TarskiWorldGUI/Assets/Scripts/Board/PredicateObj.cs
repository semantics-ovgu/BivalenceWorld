using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredicateObj : MonoBehaviour
{
    public float GetDefaultSize() => _defaultSize;
    [SerializeField]
    private float _defaultSize = 4.0f;

    public GameObject GetVisual() => _visual;
    [SerializeField]
    private GameObject _visual = default;

    [SerializeField]
    private GameObject _worldCanvasPrefab = default;
    private GameObject _worldCanvasInstance = default;

    public List<string> GetConstant() => _constant;
    private List<string> _constant = new List<string>();

    public List<Predicate> GetPredicates() => _predicateList;
    private List<Predicate> _predicateList = new List<Predicate>();

    public Predicate GetInitialPredicate() => _initialPredicate;
    private Predicate _initialPredicate = default;

    public void Init(Predicate predicate)
    {
        _initialPredicate = predicate;
    }

    public void AddConstant(string constant)
    {
        if (_constant.Contains(constant))
        {
            _constant.Remove(constant);
            SpawnText();
        }
        else 
        {
            _constant.Add(constant);
            SpawnText();
        }
    }

    public void RemoveConstant(string constant)
    {
        if (_constant.Contains(constant))
        {
            _constant.Remove(constant);
            SpawnText();
        }
        else
        {
            Debug.Log("Something is wrong. Constant has not the key but u try to delete: " + constant);
        }
    }

    public void SpawnText()
    {
        if (_constant.Count == 0 && _worldCanvasInstance != null)
        {
            Destroy(_worldCanvasInstance.gameObject);
        }
        else if (_worldCanvasInstance == null)
        {
            _worldCanvasInstance = Instantiate(_worldCanvasPrefab, this.transform.position, Quaternion.identity, this.transform);
            _worldCanvasInstance.transform.localPosition = new Vector3(0, 7, 0);
            _worldCanvasPrefab.transform.localScale = Vector3.one * 1f;
            _worldCanvasInstance.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = GetConstantString();
        }
        else if (_worldCanvasInstance != null)
        {
            var textElement = _worldCanvasInstance.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            if (textElement != null)
            {
                textElement.text = GetConstantString();
            }
        }
    }

    private string GetConstantString()
    {
        var finalString = "";
        foreach (var item in _constant)
        {
            finalString += item + " ";

        }
        return finalString;
    }

    public void AddModifier(Predicate predicate)
    {
        var bhvr = predicate.Prefab.GetComponent<PredicateBhvr>();
        if (bhvr != null)
        {
            if (!_predicateList.Contains(predicate))
            {
                bhvr.Create(this);
                _predicateList.Add(predicate);
            }
            else
            {
                RemoveModifier(predicate);
            }
        }
        else
        {
            Debug.LogWarning("Wrong Mapping should be PredicateBhvr");
        }
    }

    public void RemoveAllModifier()
    {
        for (int i = _predicateList.Count -1; i >= 0; i--)
        {
            var instance = _predicateList[i];
            instance.Prefab.GetComponent<PredicateBhvr>().Undo();
            _predicateList.Remove(instance);
        }
    }

    public void RemoveModifier(Predicate predicate)
    {
        if (_predicateList.Contains(predicate))
        {
            _predicateList.Remove(predicate);
            predicate.Prefab.GetComponent<PredicateBhvr>().Undo();
        }
    }
}
