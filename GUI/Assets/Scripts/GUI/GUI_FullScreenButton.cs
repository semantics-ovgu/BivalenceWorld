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

	private void Awake()
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
		Screen.fullScreen = !Screen.fullScreen;
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
