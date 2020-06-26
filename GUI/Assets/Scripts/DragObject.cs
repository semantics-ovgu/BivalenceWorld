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

	private void OnMouseDown()
	{
		_startPos = _rootObj.position;
		_mZCoord = GameManager.Instance.GetCameraManager().GetCurrentCamera().WorldToScreenPoint(gameObject.transform.position).z;
	}

	private Vector3 GetMouseAsWorldPoint()
	{
		Vector3 mousePoint = Input.mousePosition;
		mousePoint.z = _mZCoord;
		return GameManager.Instance.GetCameraManager().GetCurrentCamera().ScreenToWorldPoint(mousePoint);
	}


	private void OnMouseDrag()
	{
		var asd = Physics.RaycastAll(GameManager.Instance.GetCameraManager().GetCurrentCamera().ScreenPointToRay(Input.mousePosition));

		if (asd.Length > 0)
		{
			for (int i = 0; i < asd.Length; i++)
			{
				if (asd[i].collider.gameObject.layer == LayerMask.NameToLayer("Selectable"))
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
		SelectionManager selection = GameManager.Instance.GetSelectionManager();

		ISelectable target = selection.TargetHoveredElement;
		if (target != null)
		{
			var field = target.GetRootObj().GetComponent<Field>();

			if (CheckIfFieldIsEmpty(field))
			{
				if (_predicate.GetField() != field)
				{
					selection.SelectObj();
				}

				_predicate.GetField().ResetPredicate();
				field.AddPredicateObj(_predicate);
			}
			else
			{
				_rootObj.position = _startPos;
			}
		}
		else
		{
			_predicate.CurrentField.DestroyPredicateObj();
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

	private bool CheckIfFieldIsEmpty(Field field)
	{
		if (field != null && field.GetPredicateInstance() == null)
		{
			return true;
		}

		return false;
	}
}
