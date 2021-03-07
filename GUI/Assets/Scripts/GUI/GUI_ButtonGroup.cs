using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GUI
{
    public class GUI_ButtonGroup : MonoBehaviour
    {
        [SerializeField]
        private List<GUI_Button> _buttons = default;

        private void Awake()
        {
            foreach (var guiButton in _buttons)
            {
                guiButton.OnButtonSelectEvent.AddEventListener(ButtonClick);
            }
        }

        private void ButtonClick(GUI_Button buttonFire)
        {
            foreach (var button in _buttons)
            {
                if (button != buttonFire)
                {
                    button.ButtonUnselect();
                }
            }
        }
    }
}
