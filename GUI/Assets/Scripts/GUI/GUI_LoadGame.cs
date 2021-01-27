using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Validator;
using SmartDLL;

public class GUI_LoadGame : GUI_Button
{
	[SerializeField]
	private TMP_InputField _inputField = default;
	public SmartFileExplorer fileExplorer = new SmartFileExplorer();

	[SerializeField]
	private List<Predicate> _predicates;
	[SerializeField]
	private Toggle _openNewTabToggle;


	protected override void ButtonClickedListener()
	{
		LoadWorldObj();
		LoadSentences();
	}

	private bool ExistsPath(string endString)
	{
		string path =  GUI_SaveCurrentGame.FOLDER + "/" + _inputField.text + ".json" + endString;
		if (File.Exists(path))
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	private string Load(string endString)
	{
		var path = GUI_SaveCurrentGame.FOLDER + "/" + _inputField.text + ".json" + endString;
		if (File.Exists(path))
		{
			return File.ReadAllText(path);
		}

		return "";

	}

	private void LoadSentences()
	{
		if (ExistsPath(GUI_SaveCurrentGame.SENTENCES))
		{
			var jsonStringBack = Load(GUI_SaveCurrentGame.SENTENCES);
			if (jsonStringBack == "")
				return;

			var deserializedObj = JsonConvert.DeserializeObject<List<string>>(jsonStringBack);

			if (deserializedObj != null)
			{
				var manager = GameManager.Instance;
				if (manager == null)
				{
					return;
				}

				if (_openNewTabToggle.isOn)
				{
					List<string> txt = new List<string>();
					List<GUI_TextInputElement> list = manager.GetTextInputField().InputField;
					for (var i = 0; i < list.Count; i++)
					{
						var item = list[i];
						if (i < deserializedObj.Count)
						{
							txt.Add(deserializedObj[i]);
						}
					}
					manager.NavigationText.CreateTextInstance(txt);
				}
				else
				{
					manager.GetTextInputField().CleanAllText();
					List<GUI_TextInputElement> list = manager.GetTextInputField().InputField;
					for (var i = 0; i < list.Count; i++)
					{
						var item = list[i];
						if (i < deserializedObj.Count)
						{
							list[i].AddText(deserializedObj[i]);
						}
					}
				}
			}
		}
	}

	private void CleanUpBoard(Board board)
	{
		board.DestroyMap();
		board.CreateMap();
	}

	private void LoadWorldObj()
	{
		if (ExistsPath(GUI_SaveCurrentGame.WORLD))
		{
			var jsonStringBack = Load(GUI_SaveCurrentGame.WORLD);
			if (jsonStringBack == "")
				return;

			WorldObject[] worldObjs = JsonConvert.DeserializeObject<WorldObject[]>(jsonStringBack);

			if (worldObjs != null && worldObjs.Length > 0)
			{
				var board = GameManager.Instance.GetCurrentBoard();
				CleanUpBoard(board);

				Debug.Log("Anzahl: " + worldObjs.Length);
				foreach (var item in worldObjs)
				{
					Debug.Log(item.Predicates.Count);

					var xRaw = item.Tags[0].ToString();
					int x = int.Parse(xRaw);

					var zRaw = item.Tags[1].ToString();
					int z = int.Parse(zRaw);

					var field = board.GetFieldFromCoord(x, z);
					if (field != null)
					{
						foreach (var predicates in item.Predicates)
						{
							var predicatePrefab = _predicates.Find(pre => (pre.PredicateIdentifier == predicates));

							field.AddPredicate(predicatePrefab);
						}

						foreach (var constant in item.Consts)
						{
							field.GetPredicateInstance().AddConstant(constant);
						}
					}
					else
					{
						Debug.LogWarning("Can not find field with the coord: X: " + xRaw + ", Z: " + zRaw);
					}
				}
			}
		}
	}
}
