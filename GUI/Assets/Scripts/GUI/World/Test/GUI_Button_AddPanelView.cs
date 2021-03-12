using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GUI.World.Debug
{
    public class GUI_Button_AddPanelView : MonoBehaviour
    {
        [SerializeField]
        private GUI_World_PanelNavigation _panelNavigation = default;
        [SerializeField]
        private GUI_World_PanelView _viewPanel = default;
        [SerializeField]
        private Button _button = default;

        private void Awake()
        {
            _button.onClick.AddListener(ButtonOnClickListener);
        }

        private void ButtonOnClickListener()
        {
            _panelNavigation.AddAndShowPanel(Instantiate(_viewPanel));
        }
    }
}
