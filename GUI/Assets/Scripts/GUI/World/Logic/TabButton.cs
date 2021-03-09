using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.GUI.World
{
    public interface TabButton
    {
        void AddOnButtonDeleteClickedEventListener(UnityAction<TabButtonContainer> callback);
        void AddOnButtonSelectEventListener(UnityAction<TabButtonContainer> callback);

        void SetRoot(RectTransform root);

        void Destroy();
    }

    public struct TabButtonContainer
    {
        public Panel Panel { get; set; }
        public TabButton Button { get; set; }
    }
}
