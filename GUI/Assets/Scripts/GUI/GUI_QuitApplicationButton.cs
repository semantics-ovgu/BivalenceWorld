using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUI_QuitApplicationButton : GUI_Button
{
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
	}

	protected override void ButtonClickedListener()
	{
		Application.Quit();
	}
}
