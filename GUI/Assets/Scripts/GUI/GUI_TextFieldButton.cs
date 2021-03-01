using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI_TextFieldButton : GUI_TabButton
{
	private List<string> _texts = new List<string>();
	[SerializeField]
	private Color _hoverColor = default;

	public void SetTexts(List<string> txt)
	{
		_texts = txt;
	}

	public List<string> GetText()
	{
		return _texts;
	}

	public void UnHover()
	{
		GetComponent<Image>().color = new Color(1, 1, 1, 1);

	}

	public void Hover()
	{
		GetComponent<Image>().color = _hoverColor;
	}
}
