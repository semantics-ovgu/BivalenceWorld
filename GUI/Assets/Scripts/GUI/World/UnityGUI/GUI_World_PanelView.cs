using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.GUI.World.UnityGUI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts.GUI.World
{
    public class GUI_World_PanelView : GUI_Factory_Panel, Panel
    {
        [SerializeField]
        private RectTransform _content = default;
        private IPanelName _panelName = null;
        [SerializeField]
        private string _defaultName = "Unknown";

        public void Show()
        {
            _content.gameObject.SetActive(true);
        }

        private void Update()
        {

        }

        public string DefaultName()
        {
            if (_panelName != null)
            {
                return _panelName.GetName();
            }
            else
            {
                return _defaultName;
            }
        }

        public event Action<Panel> PanelDestroyedEvent;

        public void Destroy()
        {
            var destroyRestriction = _content.GetComponentInChildren<IPanelDestroyRestriction>();
            if (destroyRestriction != null)
            {
                destroyRestriction.PanelViewDestroyedEvent -= PanelViewDestroyEventListener;
                destroyRestriction.PanelViewDestroyedEvent += PanelViewDestroyEventListener;
                destroyRestriction.Destroy();
            }
            else
            {
                PanelViewDestroyEventListener();
            }
        }

        private void PanelViewDestroyEventListener()
        {
            PanelDestroyedEvent?.Invoke(this);
            Destroy(gameObject);
        }

        public void Hide()
        {
            _content.gameObject.SetActive(false);
        }

        public void Initialize(RectTransform rect)
        {
            transform.SetParent(rect);
            _content.position = Vector3.zero;
            _content.sizeDelta = Vector2.zero;
            _content.localScale = Vector3.one;

            _panelName = _content.GetComponent<IPanelName>();
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
