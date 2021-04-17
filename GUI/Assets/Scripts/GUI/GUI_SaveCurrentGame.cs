using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using Assets.Scripts.GUI.World;
using SFB;
using Validator;

public class GUI_SaveCurrentGame : GUI_Button
{
    public const string SENTENCES = "sen";
    public const string WORLD = "wld";
    public const string FOLDER = "Saves";

    private string _lastChoosenDirectory = "";

    [SerializeField]
    private bool _saveWorld = false;
    [SerializeField]
    private GUI_Factory_TabsButtonPanel _worldTabsButtons = default;
    [SerializeField]
    private bool _showDialog = false;
    private TabsButtonPanel _buttonPanel = null;

    private void Start()
    {
        _buttonPanel = _worldTabsButtons.Create();
    }

    protected override void ButtonClickedListener()
    {
        if (_saveWorld)
        {
            SaveWorldObjs(null);
        }
        else
        {
            SaveSentences();
        }
    }

    public bool SaveWorldObjs(Board saveBoard)
    {
        var board = saveBoard;
        if (board == null)
        {
            board = GameManager.Instance.GetCurrentBoard();
        }

        if (board != null)
        {
            List<Field> obj = board.GetFieldElements();
            List<WorldObject> worldObjs = new List<WorldObject>();
            var defaultName = board.GetName();

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

                    List<object> coord = new List<object>
                    {
                            item.GetX(),
                            item.GetZ()
                    };
                    worldObjs.Add(new WorldObject(constant, worldPredicates, coord));
                }
            }

            var jsonString = JsonConvert.SerializeObject(worldObjs);
            var path = SaveDataWorld(jsonString, WORLD, board, defaultName);

            if (!string.IsNullOrEmpty(path))
            {
                _buttonPanel.Update();
                return true;
            }
        }

        return false;
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

    private string SaveDataWorld(string fileContent, string world, Board board, string defaultName = "")
    {
        ExtensionFilter[] filter = new ExtensionFilter[]
        {
                new ExtensionFilter("World", world),
        };

        var path = board.SavePath;

        if (string.IsNullOrEmpty(path))
        {
            path = StandaloneFileBrowser.SaveFilePanel("Save World", GetRootDirectory(), defaultName, filter);
        }
        else if (_showDialog)
        {
            var directory = Path.GetDirectoryName(path);
            path = StandaloneFileBrowser.SaveFilePanel("Save World", directory, defaultName, filter);
        }

        if (!string.IsNullOrEmpty(path))
        {
            if (!Path.HasExtension(path) || Path.GetExtension(path) != "." + WORLD)
            {
                path += "." + WORLD;
            }
            File.WriteAllText(path, fileContent);
            _lastChoosenDirectory = Path.GetDirectoryName(path);
            board.SetSavePath(path);
        }

        return path;
    }

    public bool SaveSentences(GUI_TextFieldButton button = null)
    {
        var manager = GameManager.Instance;
        if (manager == null)
        {
            return false;
        }

        var currentButton = button;
        if (currentButton == null)
        {
            currentButton = manager.NavigationText.GetCurrentSelectedButton();
        }

        //List<GUI_TextInputElement> list = manager.GetTextInputField().GetGuiTextElementsWithText();
        var resultSentences = "";
        foreach (var item in currentButton.GetText())
        {
            resultSentences += item;
        }

        Debug.Log(resultSentences);
        //var jsonString = JsonConvert.SerializeObject(currentButton.GetText());
        string correctData = JsonConvert.SerializeObject(currentButton.GetText(), new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        var path = SaveDataSentences(correctData, currentButton, SENTENCES);
        if (!string.IsNullOrEmpty(path))
        {
            var name = Path.GetFileNameWithoutExtension(path);
            currentButton.SetButtonName(name);
        }

        return !string.IsNullOrEmpty(path);
    }

    private string SaveDataSentences(string fileContent, GUI_TextFieldButton currentButton, string sentences)
    {
        ExtensionFilter[] filter = new ExtensionFilter[]
        {
                new ExtensionFilter("Sentence", sentences),
        };

        var path = currentButton.SavePath;

        if (string.IsNullOrEmpty(path))
        {
            path = StandaloneFileBrowser.SaveFilePanel("Save Sentence", GetRootDirectory(), currentButton.GetButtonName(), filter);
        }
        else if (_showDialog)
        {
            var directory = Path.GetDirectoryName(path);
            path = StandaloneFileBrowser.SaveFilePanel("Save Sentence", directory, currentButton.GetButtonName(), filter);
        }

        if (!string.IsNullOrEmpty(path))
        {
            if (!Path.HasExtension(path) || Path.GetExtension(path) != "." + SENTENCES)
            {
                path += "." + SENTENCES;
            }
            File.WriteAllText(path, fileContent);
            _lastChoosenDirectory = Path.GetDirectoryName(path);
            currentButton.SetSavePath(path);
        }

        return path;
    }
}
