using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Windows.WebCam;

namespace Assets.Scripts.GUI.World
{
    public interface TabButton
    {
        void AddOnButtonDeleteClickedEventListener(UnityAction<TabButtonContainer> callback);
        void AddOnButtonSelectEventListener(UnityAction<TabButtonContainer> callback);

        void SetRoot(RectTransform root);

        void Destroy();

        void Unselect();

        void Select();

        bool IsSelected();

        Panel GetPanel();

        void SetName(string label);

        void UpdateName();

        string GetName();
    }

    public struct TabButtonContainer
    {
        public Panel Panel { get; set; }
        public TabButton Button { get; set; }
    }
}
