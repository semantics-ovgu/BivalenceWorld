using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI_Navigation_Text : MonoBehaviour
{
	[SerializeField]
	private List<Element> _panels = default;


	private void Awake()
	{
		if (_panels.Count > 0)
		{
		
			for (var i = 0; i < _panels.Count; i++)
			{
				var element = _panels[i];
				int id = i;
				element.Button.onClick.AddListener(() => ButtonClickedListener(id));
			}

			ActivatePanel(_panels[0].Panel);
		}
	}

	private void ButtonClickedListener(int id)
	{
		if(_panels.Count > id)
		{
			ActivatePanel(_panels[id].Panel);
		}
	}

	private void ActivatePanel(GameObject panel)
	{
		DeactivateAllPanels();
		panel.SetActive(true);
	}

	private void DeactivateAllPanels()
	{
		foreach (var element in _panels)
		{
			element.Panel.SetActive(false);
		}
	}

	[System.Serializable]
	public class Element
	{
		public Button Button = default;
		public GameObject Panel = default;
	}
}
