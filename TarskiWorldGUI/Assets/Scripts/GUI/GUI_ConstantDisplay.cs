using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public class GUI_ConstantDisplay : MonoBehaviour
{
	[SerializeField]
	private TMPro.TextMeshProUGUI _text = default;
	[SerializeField]
	private Image _image = default;

	public void SetText(string txt)
	{
		//ToDo Resize
		_text.text = txt;
		Canvas.ForceUpdateCanvases();
		_image.rectTransform.sizeDelta = new Vector2(_text.rectTransform.sizeDelta.x, _image.rectTransform.sizeDelta.y);
	}
}
