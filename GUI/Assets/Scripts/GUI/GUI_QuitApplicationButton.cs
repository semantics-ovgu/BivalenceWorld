using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class GUI_QuitApplicationButton : GUI_Button
{
    private Stack<Board> _unsavedBoards = new Stack<Board>();
    private Board _currentBoardToSave = null;

    protected override void ButtonClickedListener()
    {
        SetUnsavedWorlds();
        CreateMessageBoxes();
    }

    private void CreateMessageBoxes()
    {
        if (_unsavedBoards.Any())
        {
            _currentBoardToSave = _unsavedBoards.Pop();
            var messageBox = GameManager.Instance.CreateMessageBox();
            messageBox.Init($"Do you want to save the changes you made in \"{_currentBoardToSave.GetName()}\"?");
            messageBox.OnDontSaveButtonClickedEvent.AddListener(() => CreateMessageBoxes());
            messageBox.OnSaveButtonClickedEvent.AddListener(() => SaveBoard());
            messageBox.OnCancelButtonClickedEvent.AddListener(() => AbortQuitProcess());
        }
        else
        {
            Application.Quit();
        }
    }

    private void AbortQuitProcess()
    {
        _unsavedBoards.Clear();
        _currentBoardToSave = null;
    }

    private void SaveBoard()
    {
        GameManager.Instance.SaveCurrentWorld(_currentBoardToSave);
        CreateMessageBoxes();
    }

    private void SetUnsavedWorlds()
    {
        var boards = GameManager.Instance.BoardHandler.GetAllBoards();
        _unsavedBoards.Clear();

        foreach (var board in boards)
        {
            if (board.HasUnsavedChanges)
            {
                _unsavedBoards.Push(board);
            }
        }
    }

    private void SaveSentence()
    {

    }
}
