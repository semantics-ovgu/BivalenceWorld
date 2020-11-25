using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI_TabButtonCloseButton : GUI_TabButton
{
	[SerializeField]
	private Button _closeButton = default;
	public GenericEvent<GUI_TabButtonCloseButton> DestroyObjectEvent = new GenericEvent<GUI_TabButtonCloseButton>();

	public void Awake()
	{
		_closeButton.onClick.AddListener(CloseButtonClickedListener);
	}

	private void CloseButtonClickedListener()
	{
		DestroyObjectEvent.InvokeEvent(this);
	}
}
