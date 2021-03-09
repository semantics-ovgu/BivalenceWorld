using UnityEngine;

namespace Assets.Scripts.GUI.World
{
    public interface Panel
    {
        void Destroy();

        void Hide();

        void Initialize(RectTransform rect);

        bool IsVisible();

        void Show();
    }
}
