using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ATextPanel: MonoBehaviour
{
	[SerializeField]
	protected GUI_Navigation_Text.EType _type = default;

	public GUI_Navigation_Text.EType GetType() => _type;

	public virtual void ActivatePanel()
	{
		this.gameObject.SetActive(true);
	}

	public virtual void DeactivatePanel()
	{
		this.gameObject.SetActive(false);
	}

}
