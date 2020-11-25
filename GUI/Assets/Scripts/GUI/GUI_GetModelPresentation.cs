using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI_GetModelPresentation : APage
{
	[SerializeField]
	private TMPro.TextMeshProUGUI _text = default;

	public void SetText(string txt)
	{

		_text.SetText(txt);
	}

	private void Awake()
	{
		GameManager.Instance.SetModelPresentation(this);
	}

	private void OnDestroy()
	{
		GameManager.Instance.RemoveModelPresentation(this);
	}
}
