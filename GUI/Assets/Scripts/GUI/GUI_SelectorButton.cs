using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI_SelectorButton : MonoBehaviour
{
	[SerializeField]
	private GUI_TabNavigation.EType _type = default;
	[SerializeField]
	private Button _mainButton = default;
	[SerializeField]
	private Button _leftButton = default;
	[SerializeField]
	private Button _rightButton = default;
	[SerializeField]
	private GameObject _selectorPanel = default;

	[SerializeField]
	private GUI_TabNavigation _leftPanelNavigation = default;
	[SerializeField]
	private GUI_TabNavigation _rightPanelNavigation = default;

	private void Awake()
	{
		ActivateSelector(false);
		_mainButton.onClick.AddListener(MainButtonPressedListener);
		_leftButton.onClick.AddListener(LeftButtonPressedListener);
		_rightButton.onClick.AddListener(RightButtonPressedListener);
	}

	private void RightButtonPressedListener()
	{
		_rightPanelNavigation.CreateObjFromType(_type);
		ActivateSelector(false);
	}

	private void LeftButtonPressedListener()
	{
		_leftPanelNavigation.CreateObjFromType(_type);
		ActivateSelector(false);
	}

	private void MainButtonPressedListener()
	{
		ActivateSelector(true);
	}

	private void ActivateSelector(bool active)
	{
		if (active && !_selectorPanel.activeInHierarchy)
		{
			_selectorPanel.gameObject.SetActive(true);
		}
		else
		{
			_selectorPanel.gameObject.SetActive(false);
		}
	}
}
