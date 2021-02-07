using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI_TextFieldCloseButton : GUI_TextFieldButton
{
	[SerializeField]
	private Button _closeButton = default;
	public GenericEvent<GUI_TextFieldCloseButton> DestroyObjectEvent = new GenericEvent<GUI_TextFieldCloseButton>();

	public void Awake()
	{
		_closeButton.onClick.AddListener(CloseButtonClickedListener);
	}

	private void CloseButtonClickedListener()
	{
		DestroyObjectEvent.InvokeEvent(this);
	}
}
