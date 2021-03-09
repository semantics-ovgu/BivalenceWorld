using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.GUI.World
{
    public class GUI_World_PanelNavigation : MonoBehaviour, PanelNavigation
    {
        private PanelNavigation _panelNavigation = null;
        [SerializeField]
        private RectTransform _contentRect = default;
        [SerializeField]
        private GUI_Factory_TabsButtonPanel _tabsButtonPanelFactory = default;

        private TabsButtonPanel _tabsButtonPanel = null;

        private void Start()
        {
            _tabsButtonPanel = _tabsButtonPanelFactory.Create();

            _panelNavigation = new PanelTabsNavigation(_contentRect, _tabsButtonPanel, GameManager.Instance.ButtonPrefabFactory);
        }

        public void AddAndShowPanel(Panel panel)
        {
            _panelNavigation.AddAndShowPanel(panel);
        }

        public void RemovePanel(Panel panel)
        {
            _panelNavigation.RemovePanel(panel);
        }
    }
}
