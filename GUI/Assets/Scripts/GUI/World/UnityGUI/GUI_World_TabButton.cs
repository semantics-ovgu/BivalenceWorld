using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts.GUI.World
{
    public class GUI_World_TabButton : GUI_Factory_TabButton, TabButton
    {
        [SerializeField]
        private Button _button = default;
        [SerializeField]
        private Button _deleteButton = default;
        private Panel _panel = default;

        public override TabButton Create(Panel panel)
        {
            var resultObject = Instantiate(this);
            resultObject.AddPanel(panel);

            return resultObject;
        }

        private void AddPanel(Panel panel)
        {
            _panel = panel;
        }

        public void AddOnButtonDeleteClickedEventListener(UnityAction<TabButtonContainer> callback)
        {
            _deleteButton.onClick.AddListener(() => OnButtonClickEventListener(callback));
        }

        private void OnButtonClickEventListener(UnityAction<TabButtonContainer> callback)
        {
            TabButtonContainer eventArgs = new TabButtonContainer
            {
                Panel = _panel,
                Button = this
            };

            callback.Invoke(eventArgs);
        }

        public void AddOnButtonSelectEventListener(UnityAction<TabButtonContainer> callback)
        {
            _button.onClick.AddListener(() => OnButtonClickEventListener(callback));
        }

        public void SetRoot(RectTransform root)
        {
            transform.SetParent(root);
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}
