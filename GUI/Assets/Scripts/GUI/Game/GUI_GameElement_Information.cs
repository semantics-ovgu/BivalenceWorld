using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Validator.Game;

public class GUI_GameElement_Information : GUI_AGameElement
{
	[SerializeField]
	private TextMeshProUGUI _text = default;
	[SerializeField]
	private Button _button = default;

	public override void Init(AMove move)
	{
		base.Init(move);
		_text.SetText(move.Message);
		_button.onClick.AddListener(ButtonClickedListener);
	}

	private void ButtonClickedListener()
	{
		OnFinishedMoveEvent(null);
		_button.interactable = false;
	}
}
