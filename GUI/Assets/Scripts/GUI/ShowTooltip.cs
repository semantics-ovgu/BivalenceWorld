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
		if (_isHovered && _tooltipText != null)
		{
			_internTimer += Time.deltaTime;

			if (_internTimer >= _timer)
			{
				TooltipScreenSpaceUI.ShowTooltip_Static(_tooltipText.GetLocalizedString(this));
				_isHovered = false;
			}
		}
	}

	public void SetToolTipTextKey(LocalizedString stringKey)
	{
		_tooltipText = stringKey;
	}

	public void ResetToolTipText()
	{
		_tooltipText = null;
		TooltipScreenSpaceUI.HideTooltip_Static();
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
