using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using SFB;
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

    private string _lastChoosenDirectory = "";


    protected override void ButtonClickedListener()
    {
        var extensions = new ExtensionFilter[]
        {
                new ExtensionFilter("World/Sentence", GUI_SaveCurrentGame.WORLD, GUI_SaveCurrentGame.SENTENCES),
                new ExtensionFilter("World", GUI_SaveCurrentGame.WORLD),
                new ExtensionFilter("Sentence", GUI_SaveCurrentGame.SENTENCES)
        };

        var pathSelection = StandaloneFileBrowser.OpenFilePanel("Load World/Sentence", GetDirectory(), extensions, false);

        if (pathSelection != null && pathSelection.Length > 0)
        {
            var filePath = pathSelection[0];
            var extension = Path.GetExtension(filePath).Trim('.');
            _lastChoosenDirectory = Path.GetDirectoryName(filePath);

            switch (extension)
            {
                case GUI_SaveCurrentGame.SENTENCES:
                    LoadSentences(filePath);
                    break;
                case GUI_SaveCurrentGame.WORLD:
                    LoadWorldObj(filePath);
                    break;
            }
        }
    }

    private bool ExistsPath(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private string Load(string path)
    {
        if (File.Exists(path))
        {
            return File.ReadAllText(path);
        }

        return "";

    }

    private string GetDirectory()
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

    private void LoadSentences(string path)
    {
        var jsonStringBack = Load(path);
        if (jsonStringBack == "")
        {
	        return;
        }

        var deserializedObj = JsonConvert.DeserializeObject<List<string>>(jsonStringBack);
        var rawName = Path.GetFileName(path);
        var name = rawName.Split('.')[0];

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
                for (var i = 0; i < deserializedObj.Count; i++)
                {
	                if (i < deserializedObj.Count)
	                {
		                txt.Add(deserializedObj[i]);
	                }
                }

                manager.NavigationText.CreateTextInstance(name, txt);
            }
            else
            {
                List<string> txt = new List<string>();
                for (var i = 0; i < deserializedObj.Count; i++)
                {
	                if (i < deserializedObj.Count)
                    {
	                    txt.Add(deserializedObj[i]);
                    }
                }
                manager.NavigationText.OverwriteCurrentText(name, txt);
            }
        }
    }

    private void CleanUpBoard(Board board)
    {
        board.DestroyMap();
        board.CreateMap();
    }

    private void LoadWorldObj(string path)
    {
        var jsonStringBack = Load(path);
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
