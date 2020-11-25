using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUI_TabButton : MonoBehaviour
{
	[SerializeField]
	private Button _button = default;
	[SerializeField]
	private TextMeshProUGUI _buttonName = default;

	public Button GetButton()
	{
		return _button;
	}

	public void SetButtonName(string name)
	{
		_buttonName.SetText(name);
	}
}
