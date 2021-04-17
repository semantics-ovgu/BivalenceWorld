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
            AddPanelDestroyEventListener(panel);
        }

        private void AddPanelDestroyEventListener(Panel panel)
        {
            panel.PanelDestroyedEvent += PanelDestroyedEventListener;
        }

        private void PanelDestroyedEventListener(Panel panel)
        {
            _buttonPanel.RemoveTabButtonFromPanel(panel);
            _panels.Remove(panel);

            UpdatePanels();
        }

        public void RemovePanel(Panel panel)
        {
            var currentPanel = _panels.Find(p => p == panel);

            if (currentPanel != null)
            {
                currentPanel.Destroy();
            }
        }

        private void AddPanelButton(Panel panel)
        {
            var button = _buttonFactory.Create(panel);
            button.AddOnButtonSelectEventListener(ButtonSelectEventListener);
            button.AddOnButtonDeleteClickedEventListener(ButtonDeleteEventListener);
            button.SetName(panel.DefaultName());

            _buttonPanel.AddTabButton(button);
        }

        private void ButtonDeleteEventListener(TabButtonContainer arg0)
        {
            RemovePanel(arg0.Panel);

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
                    var lastPanel = _panels.Last();
                    lastPanel.Show();
                    _activePanel = lastPanel;
                }
            }
            else
            {
                _activePanel = null;
            }

            foreach (var panel in _panels)
            {
                _buttonPanel.Update();
            }
        }
    }

    public interface Factory_TabsButton
    {
        TabButton Create(Panel panel);
    }
}
