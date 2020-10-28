using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.IO;
using TMPro;
using Validator;

public class GUI_SaveCurrentGame : GUI_Button
{
	public const string SENTENCES = "Sen";
	public const string WORLD = "Wld";
	public const string FOLDER = "Saves";


	[SerializeField]
	private TMP_InputField _inputField = default;

	[SerializeField]
	private Toggle _world = default;

	[SerializeField]
	private Toggle _sentences = default;

	protected override void ButtonClickedListener()
	{
		if (_sentences.isOn)
			SaveSentences();
		if (_world.isOn)
			SaveWorldObjs();
	}

	private void SaveWorldObjs()
	{
		var board = GameManager.Instance.GetCurrentBoard();

		List<Field> obj = board.GetFieldElements();
		List<WorldObject> worldObjs = new List<WorldObject>();
		foreach (Field item in obj)
		{
			if (item.HasPredicateInstance())
			{
				List<Predicate> predicates = item.GetPredicatesList();
				var constant = item.GetConstantsList();
				List<string> worldPredicates = new List<string>();
				foreach (var pred in predicates)
				{
					worldPredicates.Add(pred.PredicateIdentifier);
				}
				List<object> coord = new List<object>();
				coord.Add(item.GetX());
				coord.Add(item.GetZ());
				worldObjs.Add(new WorldObject(constant, worldPredicates, coord));
			}
		}

		var jsonString = JsonConvert.SerializeObject(worldObjs);
		SaveData(jsonString, WORLD);

	}

	private void SaveSentences()
	{
		var manager = GameManager.Instance;
		if (manager == null)
		{
			return;
		}
		List<GUI_TextInputElement> list = manager.GetTextInputField().GetGuiTextElementsWithText();
		var resultSentences = new List<string>();
		foreach (GUI_TextInputElement item in list)
		{
			resultSentences.Add(item.GetInputText());
		}

		var jsonString = JsonConvert.SerializeObject(resultSentences);
		SaveData(jsonString, SENTENCES);
	}

	private void SaveData(string data, string endString)
	{
		Debug.Log(data);
		var path = _inputField.text;
		File.WriteAllText(Application.dataPath + "/"+ FOLDER + "/" + path + ".json" + endString, data);
		var jsonStringBack = File.ReadAllText(Application.dataPath + "/"+ FOLDER + "/" + path + ".json" + endString);
		Debug.Log(jsonStringBack);
	}
}

