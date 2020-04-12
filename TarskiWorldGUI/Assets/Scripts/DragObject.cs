using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragObject : MonoBehaviour
{
	private Vector3 _mOffset;
	private float _mZCoord;
	private float _yCord = 0.0f;
	[SerializeField]
	private MeshRenderer _renderer = default;
	[SerializeField]
	private Material _selectedMaterial = default;
	private Material _normalMaterial = default;
	[SerializeField]
	private PredicateObj _predicate = default;
	private Vector3 _startPos = default;
	[SerializeField]
	private Transform _rootObj = default;

	private void Start()
	{
		_normalMaterial = _renderer.material;

	}

	void OnMouseDown()
	{
		_startPos = _rootObj.position;
		_renderer.material = _selectedMaterial;

		_mZCoord = GameManager.Instance.GetCameraManager().GetCurrentCamera().WorldToScreenPoint(gameObject.transform.position).z;

		//Debug.Log("Down");
	}

	private Vector3 GetMouseAsWorldPoint()
	{
		// Pixel coordinates of mouse (x,y)
		Vector3 mousePoint = Input.mousePosition;
		// z coordinate of game object on screen
		mousePoint.z = _mZCoord;
		return GameManager.Instance.GetCameraManager().GetCurrentCamera().ScreenToWorldPoint(mousePoint);
	}


	void OnMouseDrag()
	{
		//transform.position = new Vector3(GetMouseAsWorldPoint().x + _mOffset.x, _yCord, GetMouseAsWorldPoint().z + _mOffset.z);
	
		var asd = Physics.RaycastAll(GameManager.Instance.GetCameraManager().GetCurrentCamera().ScreenPointToRay(Input.mousePosition));

		if (asd.Length > 0)
		{
			for (int i = 0; i < asd.Length; i++)
			{
				if(asd[i].collider.gameObject.layer == LayerMask.NameToLayer("Selectable"))
				{
					var pos = asd[i].point;
					//Anhand der größe offseten
					_rootObj.position = new Vector3(pos.x, pos.y, pos.z);

				}
			}
		}
	}

	private void OnMouseUp()
	{
		_renderer.material = _normalMaterial;
		SelectionManager selection = GameManager.Instance.GetSelectionManager();

		ISelectable target = selection.TargetHoveredElement;
		//check if is null them remove to old field and select this
		if(target != null)
		{
			var field = target.GetRootObj().GetComponent<Field>();

			if (CheckIfFieldIsEmpty(field))
			{
				if (_predicate.GetField() != field)
					selection.SelectObj();

				_predicate.GetField().ResetPredicate();
				field.AddPredicateObj(_predicate);
			}
			else
			{
				//field.DestroyPredicateObj();
				_rootObj.position = _startPos;
				//this.transform.position = Vector3.zero;
			}
		}
		else
		{
			_predicate.CurrentField.DestroyPredicateObj();
			//_rootObj.position = _startPos;
			//this.transform.position = Vector3.zero;
		}
	}

	private bool CheckIfFieldIsEmpty(Field field)
	{
		if (field != null && field.GetPredicateInstance() == null)
			return true;
		else
			return false;
	}
}