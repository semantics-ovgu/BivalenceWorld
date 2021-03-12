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
    public class GUI_World_PanelView : GUI_Factory_Panel, Panel
    {
        [SerializeField]
        private RectTransform _content = default;

        public void Show()
        {
            _content.gameObject.SetActive(true);
        }

        public void Hide()
        {
            _content.gameObject.SetActive(false);
        }

        public void Initialize(RectTransform rect)
        {
            transform.SetParent(rect);
            _content.position = rect.position;
            _content.sizeDelta = rect.sizeDelta;
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }

        public bool IsVisible()
        {
            return _content.gameObject.activeSelf;
        }

        public override Panel Create()
        {
            return Instantiate(this);
        }
    }
}
