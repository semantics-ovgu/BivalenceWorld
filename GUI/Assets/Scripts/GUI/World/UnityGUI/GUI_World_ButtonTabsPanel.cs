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

        public void AddTabButton(TabButton tabButton)
        {
            _tabButtons.Add(tabButton);
            tabButton.AddOnButtonSelectEventListener(OnButtonSelectEventListener);

            tabButton.SetRoot(_root);

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
        }

        public void RemoveTabButton(TabButton tabButton)
        {
            _tabButtons.Remove(tabButton);
            tabButton.Destroy();
        }

        public override TabsButtonPanel Create()
        {
            return this;
        }
    }
}
