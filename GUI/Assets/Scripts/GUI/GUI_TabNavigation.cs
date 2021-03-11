using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GUI_TabNavigation : MonoBehaviour
{
    [SerializeField]
    protected GUI_TabButton _buttonPrefab = default;
    [SerializeField]
    protected Transform _buttonAnchor = default;
    [SerializeField]
    protected List<Element> _pagePrefab = default;
    [SerializeField]
    protected Transform _pageAnchor = default;

    protected Pair _currentPageInstance = default;
    protected List<Pair> _textPanelsInstances = new List<Pair>();

    private void Awake()
    {
        OnAwake();
    }

    protected virtual void OnAwake()
    {
        _currentPageInstance = new Pair();
    }

    protected void ActivatePanel(GUI_TabNavigation.EType type)
    {
        Pair targetPanel = _textPanelsInstances.Find(x => x.Page.GetType() == type);
        if (targetPanel == null)
        {
            var prefab = _pagePrefab.Find(x => x.Type == type);
            var buttonInstance = Instantiate(_buttonPrefab, _buttonAnchor);
            var instance = Instantiate(prefab.Obj, _pageAnchor);

            if (buttonInstance is GUI_TabButtonCloseButton tabButtonCloseButton)
            {
                tabButtonCloseButton.DestroyObjectEvent.AddEventListener(DestroyButtonEventListener);
            }

            SetButtonListener(buttonInstance, type);
            var pair = new Pair()
            {
                Button = buttonInstance,
                Page = instance
            };
            _textPanelsInstances.Add(pair);
            ActivatePanel(pair.Page);
        }
        else
        {
            ActivatePanel(targetPanel.Page);
        }
    }

    private void DestroyButtonEventListener(GUI_TabButtonCloseButton arg0)
    {
        Pair obj = _textPanelsInstances.Find(x => x.Button == arg0);
        if (obj != null)
        {
            _textPanelsInstances.Remove(obj);
            Destroy(obj.Button.gameObject);
            Destroy(obj.Page.gameObject);
        }
    }

    private void SetButtonListener(GUI_TabButton buttonInstance, EType type)
    {
        buttonInstance.SetButtonName(type.ToString());
        if (type == EType.Game)
        {
            buttonInstance.GetButton().onClick.AddListener(CreateGame);
        }
        else if (type == EType.PL1Structure)
        {
            buttonInstance.GetButton().onClick.AddListener(CreateModelRepresentation);
        }
    }

    public void CreateObjFromType(GUI_TabNavigation.EType type)
    {
        if (type == EType.Game)
        {
            CreateGame();
        }
        else if (type == EType.PL1Structure)
        {
            CreateModelRepresentation();
        }
    }

    public virtual void CreateGame()
    {
        ActivatePanel(GUI_TabNavigation.EType.Game);
    }

    public virtual void CreateModelRepresentation()
    {
        ActivatePanel(GUI_TabNavigation.EType.PL1Structure);
    }

    private void ActivatePanel(APage panel)
    {
        if (_currentPageInstance.Page != null)
        {
            _currentPageInstance.Page.DeactivatePanel();
        }

        _currentPageInstance.Page = panel;
        _currentPageInstance.Page.ActivatePanel();
        if (panel is GUI_Game guiGame)
        {
            GameManager.Instance.SetGame(guiGame);
        }
    }

    #region Helper

    [System.Serializable]
    public class Pair
    {
        public APage Page = default;
        public GUI_TabButton Button = default;
    }


    [System.Serializable]
    public class Element
    {
        public EType Type = default;
        public APage Obj = default;
    }

    public enum EType
    {
        Sentences,
        PL1Structure,
        Game
    }

    #endregion
}
