using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GUI_ScriptableObjToTextMesh : MonoBehaviour
{
	[SerializeField]
	private ScriptableObjString _scriptableObjString = default;

	[SerializeField]
	private TextMeshProUGUI _text = default;

	private void Awake()
	{
		if(_scriptableObjString != null)
		{
			_text.text = _scriptableObjString.Text;
		}
	}
}
