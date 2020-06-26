using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI_ValidateButton : MonoBehaviour, IDebug
{
	[SerializeField]
	private Button _targetButton = default;

	private Validation _validation = new Validation();

	private void Start()
	{
		_targetButton.onClick.AddListener(ButtonClickedListener);
		GameManager.Instance.AddObjToDebugList(this);
	}

	public void DebugModeChanged(bool isDebug)
	{
		_validation.DebugModeChanged(isDebug);
	}

	public int GetDebugID()
	{
		return 3;
	}

	private void ButtonClickedListener()
	{
		_validation.StartCalculator();
	}

	public void ValidateSentences(List<GUI_TextInputElement> textputElement, List<string> sentences)
	{
		_validation.StartCalculator(textputElement, sentences);
	}
}