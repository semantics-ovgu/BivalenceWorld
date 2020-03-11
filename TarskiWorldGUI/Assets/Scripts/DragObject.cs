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

	private void Awake()
	{
		_normalMaterial = _renderer.material;
	}

	void OnMouseDown()
	{

		_renderer.material = _selectedMaterial;

		_mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
		_mOffset = gameObject.transform.position - GetMouseAsWorldPoint();
		_yCord = this.transform.position.y;

		//Debug.Log("Down");
	}

	private Vector3 GetMouseAsWorldPoint()
	{
		// Pixel coordinates of mouse (x,y)
		Vector3 mousePoint = Input.mousePosition;
		// z coordinate of game object on screen
		mousePoint.z = _mZCoord;
		return Camera.main.ScreenToWorldPoint(mousePoint);
	}


	void OnMouseDrag()
	{
		transform.position = new Vector3(GetMouseAsWorldPoint().x + _mOffset.x, _yCord, GetMouseAsWorldPoint().z + _mOffset.z);
		//Debug.Log(GameManager.Instance.GetSelectionManager().TargetHoveredElement);
	}

	private void OnMouseUp()
	{
		_renderer.material = _normalMaterial;
		var selection = GameManager.Instance.GetSelectionManager();
		selection.SelectObj();
	}
}