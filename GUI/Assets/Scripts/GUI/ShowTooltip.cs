using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShowTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField]
	private LocalizedString _tooltipText = default;
	[SerializeField]
	private float _timer = 1.0f;

	private float _internTimer = default;
	private bool _isHovered = false;

	private void Update()
	{
		if (_isHovered)
		{
			_internTimer += Time.deltaTime;

			if (_internTimer >= _timer)
			{
				TooltipScreenSpaceUI.ShowTooltip_Static(_tooltipText.GetLocalizedString(this));
				_isHovered = false;
			}
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{ 
		_isHovered = true;
	}

	public void OnPointerExit(PointerEventData eventData)
	{ 
		_isHovered = false;
		_internTimer = 0f;
		TooltipScreenSpaceUI.HideTooltip_Static();
	}
}
