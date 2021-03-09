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

        public void AddTabButton(TabButton tabButton)
        {
            tabButton.SetRoot(_root);
        }

        public void RemoveTabButton(TabButton tabButton)
        {
            tabButton.Destroy();
        }

        public override TabsButtonPanel Create()
        {
            return this;
        }
    }
}
