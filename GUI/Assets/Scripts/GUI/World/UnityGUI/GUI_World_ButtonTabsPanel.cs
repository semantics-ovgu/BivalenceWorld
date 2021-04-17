using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.GUI.World
{
    public class GUI_World_ButtonTabsPanel : GUI_Factory_TabsButtonPanel, TabsButtonPanel
    {
        [SerializeField]
        private RectTransform _root = default;

        private List<TabButton> _tabButtons = new List<TabButton>();
        private TabButton _activeButton = null;

        public void AddTabButton(TabButton tabButton)
        {
            _tabButtons.Add(tabButton);
            tabButton.AddOnButtonSelectEventListener(OnButtonSelectEventListener);

            tabButton.SetRoot(_root);
            _activeButton = tabButton;

            foreach (var button in _tabButtons)
            {
                if (button != tabButton)
                {
                    button.Unselect();
                }
            }
        }

        private void OnButtonSelectEventListener(TabButtonContainer arg0)
        {
            foreach (var button in _tabButtons)
            {
                if (button != arg0.Button)
                {
                    button.Unselect();
                }
            }

            _activeButton = arg0.Button;
        }

        public void RemoveTabButton(TabButton tabButton)
        {
            _tabButtons.Remove(tabButton);
            if (_activeButton == tabButton)
            {
                _activeButton = _tabButtons.LastOrDefault();

                if (_activeButton != null)
                {
                    _activeButton.Select();
                }
            }
            tabButton.Destroy();
        }

        public void RemoveTabButtonFromPanel(Panel panel)
        {
            for (var i = 0; i < _tabButtons.Count; i++)
            {
                var tabButton = _tabButtons[i];
                if (tabButton.GetPanel() == panel)
                {
                    RemoveTabButton(tabButton);
                    break;
                }
            }
        }

        public TabButton GetActiveButton()
        {
            return _activeButton;
        }

        public override TabsButtonPanel Create()
        {
            return this;
        }

        public void Update()
        {
            foreach (var button in _tabButtons)
            {
                button.UpdateName();
            }
        }
    }
}
