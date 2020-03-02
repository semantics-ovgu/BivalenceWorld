using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour, IPredicate, IConstant
{
    // X Y
    // Constants (Unendlich A - Z)
    // Predicate (Tet und BIG -> Liste)

    [SerializeField]
    private Transform _anchor = default;
    [SerializeField]
    private GameObject _debugInformationCanvas = default;
    private GameObject _debugInformationInstance = default;

    [SerializeField]
    private MeshRenderer _meshRenderer = default;

    [SerializeField]
    private List<Predicate> _predicate = new List<Predicate>();
    private PredicateObj _predicateInstance = default;


    private int _x;
    private int _z;

    public void Init(int x, int z)
    {
        _x = x;
        _z = z;
        SpawnTextIntern("X: " + x + "\nZ: " + z);
    }

    public void SetMaterial(Material material)
    {
        _meshRenderer.material = material;
    }

    public bool HasPredicateInstance()
    {
        return _predicateInstance;
    }

    private void SpawnTextIntern(string txt)
    {
        _debugInformationInstance = Instantiate(_debugInformationCanvas, this.transform.position, Quaternion.identity, this.transform);

        _debugInformationInstance.transform.SetParent(this.transform);
        _debugInformationInstance.transform.localRotation = Quaternion.Euler(90, 90, 0);
        _debugInformationInstance.transform.localPosition = new Vector3(0f, 0.15f, 0f);
        _debugInformationInstance.transform.localScale = new Vector3(1,1,1);
        _debugInformationInstance.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = txt;
    }

    public void SetDebugMode(bool value)
    {
        _debugInformationInstance.SetActive(value);
    }

    //Refactor tmp list and this predicates list to obj list
    public List<Predicate> GetPredicatesList()
    {
        if(_predicateInstance != null)
        {
            var tmpPredicateList = new List<Predicate>();
            foreach (var item in _predicate)
            {
                tmpPredicateList.Add(item);
            }

            foreach (var item in _predicateInstance.GetPredicates())
            {
                tmpPredicateList.Add(item);
            }

            return tmpPredicateList;
        }
        else
        {
            Debug.Log("PredicateInstance is null" + this.name);
            return null;
        }

    }

    public void AddPredicate(Predicate predicate)
    {
        var obj = predicate.Prefab.GetComponent<PredicateObj>();
        if (obj != null)
        {
            TryCreatePredicate(predicate);
        }
        else
        {
            var bhvr = predicate.Prefab.GetComponent<PredicateBhvr>();
            if (bhvr != null && _predicateInstance != null)
            {
                _predicateInstance.AddModifier(predicate);
            }
            else
            {
                Debug.LogWarning("Can not find PredicatBhvr or PredicateObj");
            }
        }
    }

    public List<string> GetConstantsList()
    {
        return _predicateInstance?.GetConstant();
    }

    public void AddConstant(string constant)
    {
        if (_predicateInstance != null)
        {
            _predicateInstance.AddConstant(constant);
        }
    }

    public void RemoveConstant(string constant)
    {
        if (_predicateInstance != null)
        {
            _predicateInstance.RemoveConstant(constant);
        }
    }

    private void TryCreatePredicate(Predicate predicate)
    {
        if (!_predicate.Contains(predicate))
        {
            if(_predicate.Count > 0)
            {
                DestroyPredicateObj(_predicate[0]);
            }

            var instance  = Instantiate(predicate.Prefab, _anchor).GetComponent<PredicateObj>();
            if (instance == null)
            {
                Debug.LogWarning("Wrong Mapping here: is no predicateObj");
                return;
            }

            _predicateInstance = instance;
            _predicate.Add(predicate);
        }
        else
        {
            DestroyPredicateObj(predicate);
        }
    }

    private void DestroyPredicateObj(Predicate predicate)
    {
        if (_predicateInstance != null)
        {
            _predicate.Remove(predicate);
            Destroy(_predicateInstance.gameObject);
            _predicateInstance = null;
        }
    }
}



