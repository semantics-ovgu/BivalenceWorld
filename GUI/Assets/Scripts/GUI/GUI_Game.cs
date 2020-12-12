using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Validator;
using Validator.Game;

public class GUI_Game : APage
{

    [SerializeField]
    private GUI_AGameElement _informationPrefab = default;

    [SerializeField]
    private GUI_AGameElement _questionPrefab = default;

    [SerializeField]
    private GUI_AGameElement _endPrefab = default;
    [SerializeField]
    private GUI_GameElement_Start _startPrefab = default;


    [SerializeField]
    private Transform _anchor = default;
    private Game _game = default;
    public Game Game => _game;

    List<GameObject> _history = new List<GameObject>();
    private string _currentSentence = "";
    private BivalenceWorld _world = default;

    [SerializeField]
    protected Button _button = default;

    public GUI_AGameElement GetLastGUIGameElement()
    {
        if (_history.Any())
        {
            return _history.Last().GetComponent<GUI_AGameElement>();
        }

        return null;
    }

    public bool IsGameRunning
    {
        get
        {
            if (_history.Any(s => s != null))
            {
                return _history.Last().GetComponent<GUI_GameElement_End>() == null;
            }

            return false;
        }
    }

    private void OnValidate()
    {
        if (_button == null)
            this.gameObject.GetComponent<Button>();
    }

    private void OnDestroy()
    {
        ClearTemporaryConstants();
        GameManager.Instance.GetValidation().SetPresentationLayout(true);
    }

    private void Awake()
    {
        _button.onClick.AddListener(ButtonClickedListener);
    }


    private void ButtonClickedListener()
    {
        _world = new BivalenceWorld();

        var manager = GameManager.Instance;
        if (manager == null)
        {
            return;
        }

        string sentence = "";

        if (manager.GetTextInputField().CurrentTextInputElement != null)
        {
            sentence = manager.GetTextInputField().CurrentTextInputElement.GetInputText();
        }


        //List<string> sentences = Validation.CalculateResultSentences(manager, list);
        List<WorldObject> worldObjects = Validation.CalculateWorldObjects();

        WorldParameter parameter = new WorldParameter(worldObjects, new List<string>{
                sentence
        });

        var result = _world.Check(parameter);

        CleanUpHistory();
        ClearTemporaryConstants();
        var startInstance = Instantiate(_startPrefab, _anchor);
        startInstance.StartButtonClickedEvent.AddEventListener(StartGame);
        _history.Add(startInstance.gameObject);

        if (result.Result.Value[0].IsValid)
        {
            _currentSentence = sentence;
            startInstance.Init(_currentSentence, true);
            _button.gameObject.SetActive(false);
        }
        else
        {
            startInstance.Init(result.Result.Value[0].ErrorMessage, false);
        }
    }

    private void ClearTemporaryConstants()
    {
        foreach (var field in GameManager.Instance.GetCurrentBoard().GetFieldElements())
        {
            field.RemoveTemporaryConstants();
        }
    }


    private void StartGame(bool startValue)
    {
        _game = new Game(_currentSentence, _world, startValue);
        NewMove(null);
    }

    public void NewMove(GUI_AGameElement.EventArgs args)
    {
        AMove move = _game.Play();

        if (move is InfoMessage)
        {
            CreateInstanceFromPrefab(_informationPrefab, move);
        }
        else if (move is Question)
        {
            CreateInstanceFromPrefab(_questionPrefab, move);

        }
        else if (move is EndMessage)
        {
            CreateInstanceFromPrefab(_endPrefab, move);
        }
    }

    private void CleanUpHistory()
    {
        for (int i = _history.Count - 1; i >= 0; i--)
        {
            var element = _history[i];
            _history.RemoveAt(i);
            DestroyImmediate(element);
        }

        _history = new List<GameObject>();
    }

    private void CreateInstanceFromPrefab(GUI_AGameElement prefab, AMove move)
    {
        GUI_AGameElement moveInstance = Instantiate(prefab, _anchor);
        moveInstance.Init(move);
        moveInstance.FinishedMoveEvent.AddEventListener(NewMove);
        _history.Add(moveInstance.gameObject);
    }
}
