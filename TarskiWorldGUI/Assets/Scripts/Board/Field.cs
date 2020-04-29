using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour, IPredicate, IConstant
{
    [SerializeField]
    private Transform _anchor = default;
    [SerializeField]
    private GameObject _debugInformationCanvas = default;
    private GameObject _debugInformationInstance = default;

    [SerializeField]
    private MeshRenderer _meshRenderer = default;

    public int GetX() => _x;
    private int _x;

    public int GetZ() => _z;
    private int _z;

    private PredicateObj _predicateInstance = default;
	public PredicateObj GetPredicateInstance() => _predicateInstance;


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
    public void SetDebugMode(bool value)
    {
        _debugInformationInstance.SetActive(value);
    }

    public List<Predicate> GetPredicatesList()
    {
        if(_predicateInstance != null)
        {
            var tmpPredicateList = new List<Predicate>();
            tmpPredicateList.Add(_predicateInstance.GetInitialPredicate());
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

	public void AddPredicateObj(PredicateObj obj)
	{
		if (obj == null)
			Debug.Log("NULL");
		_predicateInstance = obj;
		_predicateInstance.gameObject.transform.position = new Vector3(this.transform.position.x, 0, this.transform.position.z);
		_predicateInstance.SetField(this);
		WorldChanged();
    }

	internal void ResetPredicate()
	{
		_predicateInstance = null;
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
        WorldChanged();
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
            WorldChanged();
        }
    }

    private void WorldChanged()
    {
	    var manager = GameManager.Instance;
	    if (manager != null)
	    {
		    manager.GetValidation().SetPresentationLayout();
            manager.GetTextInputField().ResetValidationOnTexts();
        }
    }

    public void RemoveConstant(string constant)
    {
        if (_predicateInstance != null)
        {
            _predicateInstance.RemoveConstant(constant);
            WorldChanged();
        }
    }

    private void CreatePredicate(Predicate predicate)
    {
        var instance = Instantiate(predicate.Prefab, new Vector3(this.transform.position.x, 0, this.transform.position.z), Quaternion.identity, null).GetComponent<PredicateObj>();
        if (instance == null)
        {
            Debug.LogWarning("Wrong Mapping here: is no predicateObj");
            return;
        }
        _predicateInstance = instance;
        _predicateInstance.Init(predicate);
		_predicateInstance.SetField(this);
    }

    private void TryCreatePredicate(Predicate predicate)
    {
        if (_predicateInstance == null)
        {
            CreatePredicate(predicate);
        }
        else if(_predicateInstance != null && _predicateInstance.GetInitialPredicate() != predicate)
        {
            List<string> constants = _predicateInstance.GetConstant();
            DestroyPredicateObj();
            CreatePredicate(predicate);
            AddConstantToNewPredicate(constants);
        }
        else
        {
            DestroyPredicateObj();
        }
    }

    private void AddConstantToNewPredicate(List<string> constants)
    {
        if (constants != null && constants.Count > 0)
        {
            foreach (var item in constants)
            {
                _predicateInstance.AddConstant(item);
            }
        }
    }

    public void DestroyPredicateObj()
    {
        if (_predicateInstance != null)
        {
            Destroy(_predicateInstance.gameObject);
            _predicateInstance = null;
        }
    }

    private void SpawnTextIntern(string txt)
    {
        _debugInformationInstance = Instantiate(_debugInformationCanvas, this.transform.position, Quaternion.identity, this.transform);

        _debugInformationInstance.transform.SetParent(this.transform);
        _debugInformationInstance.transform.localRotation = Quaternion.Euler(90, 90, 0);
        _debugInformationInstance.transform.localPosition = new Vector3(0f, 0.15f, 0f);
        _debugInformationInstance.transform.localScale = new Vector3(1, 1, 1);
        _debugInformationInstance.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = txt;
    }
}



