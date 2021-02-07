using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI_TextFieldButton : GUI_TabButton
{
	private List<string> _texts = new List<string>();

	public void SetTexts(List<string> txt)
	{
		_texts = txt;
	}

	public List<string> GetText()
	{
		return _texts;
	}
}
