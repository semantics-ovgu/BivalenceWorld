using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.GUI.World
{
    public class PanelTabsNavigation : PanelNavigation
    {
        private Panel _activePanel = null;
        private RectTransform _panelContentRect = null;
        private List<Panel> _panels = new List<Panel>();
        private Factory_TabsButton _buttonFactory = null;
        private TabsButtonPanel _buttonPanel = null;

        public PanelTabsNavigation(RectTransform panelContentRect, TabsButtonPanel buttonPanel, Factory_TabsButton tabsButtonFactory)
        {
            _panelContentRect = panelContentRect;
            _buttonFactory = tabsButtonFactory;
            _buttonPanel = buttonPanel;
        }

        public void AddAndShowPanel(Panel panel)
        {
            panel.Initialize(_panelContentRect);

            _panels.Add(panel);

            AddPanelButton(panel);
            ShowNewPanel(panel);
        }

        public void RemovePanel(Panel panel)
        {
            var currentPanel = _panels.Find(p => p == panel);

            if (currentPanel != null)
            {
                _panels.Remove(currentPanel);

                if (currentPanel.IsVisible())
                {
                    UpdatePanels();
                }

                currentPanel.Destroy();
            }
        }

        private void AddPanelButton(Panel panel)
        {
            var button = _buttonFactory.Create(panel);
            button.AddOnButtonSelectEventListener(ButtonSelectEventListener);
            button.AddOnButtonDeleteClickedEventListener(ButtonDeleteEventListener);

            _buttonPanel.AddTabButton(button);
        }

        private void ButtonDeleteEventListener(TabButtonContainer arg0)
        {
            RemovePanel(arg0.Panel);

            _buttonPanel.RemoveTabButton(arg0.Button);
        }

        private void ButtonSelectEventListener(TabButtonContainer arg0)
        {
            ShowNewPanel(arg0.Panel);
        }

        private void ShowNewPanel(Panel panel)
        {
            if (_activePanel != null)
            {
                _activePanel.Hide();
            }

            _activePanel = panel;
            panel.Show();
        }

        private void UpdatePanels()
        {
            if (_panels.Any())
            {
                bool isOnePanelVisible = false;

                foreach (var guiPanel in _panels)
                {
                    isOnePanelVisible |= guiPanel.IsVisible();
                }

                if (!isOnePanelVisible)
                {
                    _panels[0].Show();
                    _activePanel = _panels[0];
                }
            }
        }
    }

    public interface Factory_TabsButton
    {
        TabButton Create(Panel panel);
    }
}
