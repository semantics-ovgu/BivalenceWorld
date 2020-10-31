using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;
using Validator.Game;

public class GUI_GameElement_Question : GUI_AGameElement
{
	[SerializeField]
	private GameObject _spawnObj = default;

	[SerializeField]
	private Transform _spawnPos = default;


	private Question _question = default;
	[SerializeField]
	private Button _button = default;
	[SerializeField]
	private ToggleGroup _group = default;
	private List<Toggle> _toggleObjs = new List<Toggle>();

	public override void Init(AMove move)
	{
		base.Init(move);

		if (move is Question question)
		{
			_button.onClick.AddListener(ContinueButtonClickedListener);
			_question = question;
			foreach (var item in question.PossibleAnswers)
			{
				var instance = Instantiate(_spawnObj, _spawnPos);
				var toggle = instance.GetComponent<Toggle>();
				if (toggle)
				{
					toggle.group = _group;
					_toggleObjs.Add(toggle);
				}

				instance.GetComponentInChildren<Text>().text = item.Formula.FormattedFormula;
			}
		}
		else
		{
			Debug.LogError("Somethings goes wrong. HanHan Logic mistake?");
		}

	}

	private void ContinueButtonClickedListener()
	{
		for (var i = 0; i < _toggleObjs.Count; i++)
		{
			var item = _toggleObjs[i];
			if (item.isOn)
			{
				_question.SetAnswers(_question.PossibleAnswers[i]);
			}
		}

		OnFinishedMoveEvent(null);
		_button.interactable = false;
	}

}
