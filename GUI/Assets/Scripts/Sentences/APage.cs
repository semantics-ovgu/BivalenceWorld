using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class APage: MonoBehaviour
{
	[SerializeField]
	protected GUI_TabNavigation.EType _type = default;

	public GUI_TabNavigation.EType GetType() => _type;

	public virtual void ActivatePanel()
	{
		this.gameObject.SetActive(true);
	}

	public virtual void DeactivatePanel()
	{
		this.gameObject.SetActive(false);
	}

}
