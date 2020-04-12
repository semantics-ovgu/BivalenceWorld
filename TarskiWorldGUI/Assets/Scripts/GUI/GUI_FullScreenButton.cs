using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GUI_FullScreenButton : MonoBehaviour
{
	[SerializeField]
	private Button _button = default;

	private void Awake()
	{
		_button.onClick.AddListener(ButtonClickedListener);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
			ButtonClickedListener();
	}

	private void ButtonClickedListener()
	{
			Screen.fullScreen = !Screen.fullScreen;
	}
}
