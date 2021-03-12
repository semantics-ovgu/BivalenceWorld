using System.Collections.Generic;
using Assets.Scripts.Boards;
using Assets.Scripts.GUI.World;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : ASingleton<GameManager>
{
    [SerializeField]
    private FloatVar _debugFloatVar = default;

    private BoardHandler _boardHandler = null;

    [SerializeField]
    private BoardHandlerFactory _boardFactory = default;

    [SerializeField]
    private Camera _textureCamera = default;
    [SerializeField]
    private Camera _mainCamera = default;
    [SerializeField]
    private SelectionManager _selectionManager = default;
    [SerializeField]
    private GUI_Game _guiGame = default;
    public GUI_Game GUIGame => _guiGame;

    [SerializeField]
    private GUI_Factory_TabButton _buttonPrefab = default;
    public GUI_Factory_TabButton ButtonPrefabFactory => _buttonPrefab;

    [SerializeField]
    private RectTransform _scaleRectTransform = default;
    [SerializeField]
    private RectTransform _gameRectTransform = default;
    [SerializeField]
    private CanvasScaler _canvasScaler = default;

    [SerializeField]
    private GUI_TextInputField _textInputField = default;
    [SerializeField]
    private List<GUI_GetModelPresentation> _modelPResentation = default;
    public List<GUI_GetModelPresentation> GetModelPresentation() => _modelPResentation;
    public void SetModelPresentation(GUI_GetModelPresentation guiGetModelPresentation)
    {

        _modelPResentation.Add(guiGetModelPresentation);
        CreateNewInstanceFromModelPresentationEvent.InvokeEvent(guiGetModelPresentation);
    }

    public void RemoveModelPresentation(GUI_GetModelPresentation instance)
    {
        if (_modelPResentation.Contains(instance))
        {
            _modelPResentation.Remove(instance);
        }
    }

    public GenericEvent<GUI_GetModelPresentation> CreateNewInstanceFromModelPresentationEvent = new GenericEvent<GUI_GetModelPresentation>();
    public GenericEvent<string> ConstantChangedEvent = new GenericEvent<string>();

    private List<IDebug> _debugList = new List<IDebug>();
    public FloatVar DebugFloatVar => _debugFloatVar;

    [SerializeField]
    private CameraRotation _cameraManager = default;
    public CameraRotation GetCameraManager() => _cameraManager;

    [SerializeField]
    private Validation _validation = new Validation();

    public Validation GetValidation() => _validation;

    [SerializeField]
    private GUI_Navigation_Text _navigationText = default;
    public GUI_Navigation_Text NavigationText => _navigationText;

    private void Start()
    {
        _boardHandler = _boardFactory.CreateBoardHandler();
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
        return _boardHandler.GetCurrentBoard();
    }

    public Camera GetMainCamera()
    {
        return _cameraManager.GetCurrentCamera();
    }

    public Camera GetTextureCamera()
    {
        return _cameraManager.GetCurrentCamera();
    }

    public Ray GetScreenToRay()
    {
        float xAspect = 1 / _scaleRectTransform.localScale.x;
        float yAspect = _canvasScaler.referenceResolution.y / _mainCamera.pixelHeight;
        var mousePos = new Vector3(Input.mousePosition.x * xAspect, Input.mousePosition.y * yAspect, Input.mousePosition.z);
        return _textureCamera.ScreenPointToRay(mousePos);
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

    public Board RegisterBoard(Board prefabBoard)
    {
        return _boardHandler.CreateNewBoard(prefabBoard);
    }

    public void SetCurrentActiveBoard(Board board)
    {
        _boardHandler.SetCurrentBoard(board);
    }

    public void RemoveBoard(Board board)
    {
        _boardHandler.RemoveBoard(board);
    }

    public void SetGame(GUI_Game game)
    {
        _guiGame = game;
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
