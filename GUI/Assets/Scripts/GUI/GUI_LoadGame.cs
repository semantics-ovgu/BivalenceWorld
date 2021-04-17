using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Assets.Scripts.Boards;
using Assets.Scripts.GUI.World;
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
    private Board _boardPrefab = default;

    [SerializeField]
    private GUI_Factory_PanelNavigation _panelNavigationFactory = default;
    private PanelNavigation _panelNavigation = null;
    [SerializeField]
    private GUI_Factory_Panel _boardPanel = default;
    [SerializeField]
    private GUI_Factory_TabsButtonPanel _buttonPanelFactory = default;
    private TabsButtonPanel _buttonPanel = null;

    private string _lastChoosenDirectory = "";

    private void Start()
    {
        _panelNavigation = _panelNavigationFactory.Create();
        _buttonPanel = _buttonPanelFactory.Create();
    }

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

            List<string> txt = new List<string>();
            for (var i = 0; i < deserializedObj.Count; i++)
            {
                if (i < deserializedObj.Count)
                {
                    txt.Add(deserializedObj[i]);
                }
            }

            var instance = manager.NavigationText.CreateTextInstance(name, txt);
            instance.SetSavePath(path);
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
            _panelNavigation.AddAndShowPanel(_boardPanel.Create());
            _buttonPanel.GetActiveButton().SetName(Path.GetFileNameWithoutExtension(path));
            var board = GameManager.Instance.GetCurrentBoard();

            foreach (var item in worldObjs)
            {
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

            board.SetSavePath(path);
        }
    }
}
