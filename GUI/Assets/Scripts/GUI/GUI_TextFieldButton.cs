using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI_TextFieldButton : MonoBehaviour
{
	private List<string> _texts = new List<string>();
	[SerializeField]
	private Button _button = default;

	public void SetTexts(List<string> txt)
	{
		_texts = txt;
	}

	public Button GetButton()
	{
		return _button;
	}

	public List<string> GetText()
	{
		return _texts;
	}
}
