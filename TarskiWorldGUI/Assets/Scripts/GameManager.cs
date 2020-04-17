using System.Collections.Generic;
using UnityEngine;

public class GameManager : ASingleton<GameManager>
{
	[SerializeField]
	private FloatVar _debugFloatVar = default;

	[SerializeField]
	private Camera _mainCamera = default;
	[SerializeField]
	private SelectionManager _selectionManager = default;

	[SerializeField]
	private GUI_TextInputField _textInputField = default;
	[SerializeField]
	private GUI_GetModelPresentation _modelPResentation = default;
	public GUI_GetModelPresentation GetModelPresentation() => _modelPResentation;

	private Board _currentBoard = default;

	private List<IDebug> _debugList = new List<IDebug>();
	public FloatVar DebugFloatVar => _debugFloatVar;

	[SerializeField]
	private CameraRotation _cameraManager = default;
	public CameraRotation GetCameraManager() => _cameraManager;

	[SerializeField]
	private Validation _validation = new Validation();

	public Validation GetValidation() => _validation;





	private void Start()
	{
		CheckDebugList(_debugFloatVar.CurrentValue);
	}

	public void AddObjToDebugList(IDebug component)
	{
		if (!_debugList.Contains(component))
		{
			_debugList.Add(component);
		}
	}

	public Board GetCurrentBoard()
	{
		return _currentBoard;
	}

	public Camera GetMainCamera()
	{
		return _cameraManager.GetCurrentCamera();
	}

	public SelectionManager GetSelectionManager()
	{
		return _selectionManager;
	}

	public GUI_TextInputField GetTextInputField()
	{
		return _textInputField;
	}

	public bool IsDebugMode(int id)
	{
		return id == _debugFloatVar.CurrentValue;
	}

	public void RegisterBoard(Board board)
	{
		_currentBoard = board;
	}


	protected override void SingletonAwake()
	{
		_debugFloatVar.ValueChangedEvent.AddEventListener(DebugModeChangedListener);
		_debugFloatVar.ForceChangedEvent();
	}

	private void CheckDebugList(float newValue)
	{
		foreach (var item in _debugList)
		{
			if (item.GetDebugID() == newValue)
			{
				item.DebugModeChanged(true);
			}
			else
			{
				item.DebugModeChanged(false);
			}
		}
	}

	private void DebugModeChangedListener(FloatVar.EventArgs arg0)
	{
		CheckDebugList(arg0.NewValue);
	}
}
