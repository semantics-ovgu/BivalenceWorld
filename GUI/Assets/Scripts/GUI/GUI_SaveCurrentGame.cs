using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.IO;
using SFB;
using TMPro;
using Validator;

public class GUI_SaveCurrentGame : GUI_Button
{
    public const string SENTENCES = "sen";
    public const string WORLD = "wld";
    public const string FOLDER = "Saves";

    private string _lastChoosenDirectory = "";


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
        SaveDataWorld(jsonString, WORLD);
    }

    private string GetRootDirectory()
    {
        if (string.IsNullOrEmpty(_lastChoosenDirectory))
        {
            return Application.dataPath;
        }
        else
        {
            return _lastChoosenDirectory;
        }
    }

    private void SaveDataWorld(string fileContent, string world)
    {
        ExtensionFilter[] filter = new ExtensionFilter[]
        {
                new ExtensionFilter("World", world),
        };

        var path = StandaloneFileBrowser.SaveFilePanel("Save World", GetRootDirectory(), "", filter);
        if (!string.IsNullOrEmpty(path))
        {
            File.WriteAllText(path, fileContent);
            _lastChoosenDirectory = Path.GetDirectoryName(path);
        }
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

        //var jsonString = JsonConvert.SerializeObject(resultSentences);
        string correctData = JsonConvert.SerializeObject(resultSentences, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        SaveDataSentences(correctData, SENTENCES);
    }

    private void SaveDataSentences(string fileContent, string sentences)
    {
        ExtensionFilter[] filter = new ExtensionFilter[]
        {
                new ExtensionFilter("Sentence", sentences),
        };

        var path = StandaloneFileBrowser.SaveFilePanel("Save Sentence", GetRootDirectory(), "", filter);

        if (!string.IsNullOrEmpty(path))
        {
            File.WriteAllText(path, fileContent);
            _lastChoosenDirectory = Path.GetDirectoryName(path);
        }
    }

    private void SaveData(string data, string endString)
    {
        Debug.Log("SaveData right now");
        Debug.Log(data);

        var path = _inputField.text;
        Directory.CreateDirectory(FOLDER);
        var asd = FOLDER + "/" + path + ".json" + endString;
        Debug.Log("path: " + asd);
        // Application.persistentDataPath + "/" +

        //File.WriteAllText(asd, data);

        //Beim laden dann machen
        //if (File.Exists(asd))
        //{
        //	File.WriteAllText(asd, data);
        //	var jsonStringBack = File.ReadAllText(asd);
        //	Debug.Log(jsonStringBack);
        //}
    }
}
