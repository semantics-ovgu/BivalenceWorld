using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
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
        [SerializeField]
        private Color _selectionColor = Color.yellow;
        [SerializeField]
        private TMP_Text _name = default;
        private Color _defaultColor = Color.white;

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

            _button.image.color = _selectionColor;

            callback.Invoke(eventArgs);
        }

        public void AddOnButtonSelectEventListener(UnityAction<TabButtonContainer> callback)
        {
            _button.onClick.AddListener(() => OnButtonClickEventListener(callback));
        }

        public void SetRoot(RectTransform root)
        {
            transform.SetParent(root);
            transform.localScale = Vector3.one;
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }

        public void Unselect()
        {
            _button.image.color = _defaultColor;
        }

        public void Select()
        {
            _button.image.color = _selectionColor;
        }

        public bool IsSelected()
        {
            return _button.image.color == _selectionColor;
        }

        public Panel GetPanel()
        {
            return _panel;
        }

        public void SetName(string label)
        {
            _name.text = label;
        }

        public string GetName()
        {
            return _name.text;
        }

        public void UpdateName()
        {
            _name.text = _panel.DefaultName();
        }
    }
}
