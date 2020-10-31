using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI_Button : MonoBehaviour
{
	[SerializeField]
	protected Button _button = default;

	private void OnValidate()
	{
		if (_button == null)
			this.gameObject.GetComponent<Button>();
	}

	private void Awake()
	{
		_button.onClick.AddListener(ButtonClickedListener);
	}

	protected virtual void ButtonClickedListener()
	{

	}
}
