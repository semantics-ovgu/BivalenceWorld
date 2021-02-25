using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GUI_FullScreenButton : GUI_Button
{

	[SerializeField]
	private Image _buttonImage = default;
	[SerializeField]
	private Sprite _fullscreenSprite = default;
	[SerializeField]
	private Sprite _windowScreenSprite = default;

	private void Start()
	{
		SetSpriteToImage();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			ButtonClickedListener();
		}
	}

	protected override void ButtonClickedListener()
	{

		if (Screen.fullScreen)
		{
			Screen.fullScreen = false;
			Screen.fullScreenMode = FullScreenMode.Windowed;
		}
		else
		{
			Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, FullScreenMode.Windowed);
			Screen.fullScreen = true;
			Screen.fullScreenMode = FullScreenMode.FullScreenWindow;

		}

		SetSpriteToImage();
	}

	private void SetSpriteToImage()
	{
		if (Screen.fullScreen)
		{
			_buttonImage.sprite = _windowScreenSprite;
			
		}
		else
		{
			_buttonImage.sprite = _fullscreenSprite;
		}
	}

}
