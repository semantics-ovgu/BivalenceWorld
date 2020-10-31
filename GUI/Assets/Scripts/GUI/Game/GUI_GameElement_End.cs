using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Validator.Game;

public class GUI_GameElement_End : GUI_AGameElement
{
	[SerializeField]
	private TextMeshProUGUI _text = default;

	public override void Init(AMove move)
	{
		base.Init(move);
		_text.SetText(move.Message);
	}
}
