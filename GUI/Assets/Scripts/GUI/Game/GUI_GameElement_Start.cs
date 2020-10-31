using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUI_GameElement_Start : MonoBehaviour
{
	[SerializeField]
	private Button _trueButton = default;

	[SerializeField]
	private Button _falseButton = default;

	public GenericEvent<bool> StartButtonClickedEvent = new GenericEvent<bool>();

	[SerializeField]
	private TextMeshProUGUI _text = default;

	public void Init(string sentence, bool correctSentences)
	{
		_text.SetText(sentence);

		if (correctSentences)
		{
			_trueButton.onClick.AddListener(() => ButtonClickedListener(true));
			_falseButton.onClick.AddListener(() => ButtonClickedListener(false));
		}
		else
		{
			_falseButton.gameObject.SetActive(false);
			_trueButton.gameObject.SetActive(false);
		}

	}

	private void ButtonClickedListener(bool b)
	{
		StartButtonClickedEvent.InvokeEvent(b);
		_falseButton.interactable = false;
		_trueButton.interactable = false;
	}
}
