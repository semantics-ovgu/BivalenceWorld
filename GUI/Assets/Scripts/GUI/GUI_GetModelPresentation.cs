using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI_GetModelPresentation : ATextPanel
{
	[SerializeField]
	private TMPro.TextMeshProUGUI _text = default;

	public void SetText(string txt)
	{

		_text.SetText(txt);

	}
}
