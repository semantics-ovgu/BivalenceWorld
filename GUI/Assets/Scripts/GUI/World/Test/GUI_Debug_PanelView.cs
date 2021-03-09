using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.GUI.World.Debug
{
    public class GUI_Debug_PanelView : MonoBehaviour
    {
        [SerializeField]
        private GUI_World_PanelNavigation _panelNavigation = default;
        [SerializeField]
        private GUI_World_PanelView _view01 = default;
        [SerializeField]
        private GUI_World_PanelView _view02 = default;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                _panelNavigation.AddAndShowPanel(_view01.Create());
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                _panelNavigation.AddAndShowPanel(_view02.Create());
            }
        }
    }
}
