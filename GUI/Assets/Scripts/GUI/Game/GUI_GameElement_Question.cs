using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;
using Validator;
using Validator.Game;

public class GUI_GameElement_Question : GUI_AGameElement
{
    [SerializeField]
    private GameObject _spawnObj = default;

    [SerializeField]
    private Transform _spawnPos = default;


    private Question _question = default;
    public Question Question => _question;
    [SerializeField]
    private Button _button = default;
    [SerializeField]
    private ToggleGroup _group = default;
    private List<Toggle> _toggleObjs = new List<Toggle>();

    private int _choosenIndex = 0;

    public override void Init(AMove move)
    {
        base.Init(move);

        if (move is Question question)
        {
            _button.onClick.AddListener(ContinueButtonClickedListener);
            _question = question;
            foreach (var item in question.PossibleAnswers)
            {
                if (item.SelectionType == Question.Selection.SelectionTypes.Formula)
                {
                    var instance = Instantiate(_spawnObj, _spawnPos);
                    var toggle = instance.GetComponent<Toggle>();
                    if (toggle)
                    {
                        toggle.group = _group;
                        _toggleObjs.Add(toggle);
                    }

                    instance.GetComponentInChildren<Text>().text = item.Message;
                }
            }
        }
        else
        {
            Debug.LogError("Somethings goes wrong. HanHan Logic mistake?");
        }

    }

    public Question.Selection SetWorldObjSelection(WorldObject worldObject)
    {
        var answer = _question.PossibleAnswers.Find(s => s.WorldObject.Equals(worldObject));
        if (answer != null)
        {
            _question.SetAnswers(answer);
            return answer;
        }
        else
        {
            return null;
        }
    }

    public override void EnableBackButton()
    {
        base.EnableBackButton();


        _button.interactable = true;
    }

    public override void ResetStep(AMove move)
    {
        if (move is Question question)
        {
            _question = question;
            question.SetAnswers(question.PossibleAnswers[_choosenIndex]);
        }
    }

    private void ContinueButtonClickedListener()
    {
        if (_toggleObjs.Any())
        {
            for (var i = 0; i < _toggleObjs.Count; i++)
            {
                var item = _toggleObjs[i];
                if (item.isOn)
                {
                    _question.SetAnswers(_question.PossibleAnswers[i]);
                    _choosenIndex = i;
                    break;
                }
            }
        }

        if (_question.Answer != null)
        {
            OnFinishedMoveEvent(null);
            _button.interactable = false;
        }
    }
}
