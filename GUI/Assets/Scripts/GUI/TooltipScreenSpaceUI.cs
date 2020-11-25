using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TooltipScreenSpaceUI : MonoBehaviour
{
	public static TooltipScreenSpaceUI Instance { get; private set; }


	[SerializeField]
	private RectTransform _backgroundRectTransform = default;
	[SerializeField]
	private TextMeshProUGUI _textMeshPro = default;
	[SerializeField]
	private RectTransform _rectTransform = default;
	[SerializeField]
	private RectTransform _canvasRectTransform = default;

	private void Awake()
	{
		Instance = this;
		HideTooltip();
	}
	private void SetText(string tooltipText)
	{
		_textMeshPro.SetText(tooltipText);
		_textMeshPro.ForceMeshUpdate();

		Vector2 paddingSize = new Vector2(8,8);
		Vector2 textSize = _textMeshPro.GetRenderedValues(false);
		_backgroundRectTransform.sizeDelta = textSize + paddingSize;
	}

	private void Update()
	{
		Vector2 anchoredPositon = Input.mousePosition / _canvasRectTransform.localScale.x;

		if (anchoredPositon.x + _backgroundRectTransform.rect.width > _canvasRectTransform.rect.width)
		{
			anchoredPositon.x = _canvasRectTransform.rect.width - _backgroundRectTransform.rect.width;
		}
		if (anchoredPositon.y + _backgroundRectTransform.rect.height > _canvasRectTransform.rect.height)
		{
			anchoredPositon.y = _canvasRectTransform.rect.height - _backgroundRectTransform.rect.height;
		}

		_rectTransform.anchoredPosition = anchoredPositon;
	}

	private void ShowTooltip(string tooltipText)
	{
		gameObject.SetActive(true);
		SetText(tooltipText);
	}

	private void HideTooltip()
	{
		gameObject.SetActive(false);
	}

	public static void ShowTooltip_Static(string tooltipText)
	{
		Instance.ShowTooltip(tooltipText);
	}

	public static void HideTooltip_Static()
	{
		Instance.HideTooltip();
	}

}
