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
	private bool readText = false;

	[SerializeField]
	private List<Predicate> _predicates;
	[SerializeField]
	private Toggle _openNewTabToggle;

	void Update()
	{
		if (fileExplorer.resultOK && readText)
		{
			ReadText(fileExplorer.fileName);
			readText = false;
		}

	}

	void ShowExplorer()
	{
		string initialDir = @"C:\";
		bool restoreDir = true;
		string title = "Open a jsonWld or jsonSen File";
		string defExt = "txt";
		string filter = "jsonSen files (*.jsonSen)|*.jsonSen |jsonWld files (*.jsonWld) | *.jsonWld";

		fileExplorer.OpenExplorer(initialDir, restoreDir, title, defExt, filter);
		readText = true;
	}

	void ReadText(string path)
	{
		//File.ReadAllText(path);
		LoadSentences(path);
	}

	protected override void ButtonClickedListener()
	{
		//LoadWorldObj();
		ShowExplorer();
	}

	private bool ExistsPath(string endString)
	{
		string path = Application.dataPath + "/" + GUI_SaveCurrentGame.FOLDER + "/" + _inputField.text + ".json" + endString;
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
		string path = Application.dataPath + "/" + GUI_SaveCurrentGame.FOLDER + "/" + _inputField.text + ".json" + endString;
		return File.ReadAllText(path);
	}

	private void LoadSentences(string loadPath)
	{
		if (File.Exists(loadPath))
		{
			var jsonStringBack = File.ReadAllText(loadPath);
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
					Debug.Log(field);
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
