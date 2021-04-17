namespace Assets.Scripts.GUI.World
{
    public interface TabsButtonPanel
    {
        void AddTabButton(TabButton tabButton);
        void RemoveTabButton(TabButton tabButton);
        void RemoveTabButtonFromPanel(Panel panel);

        TabButton GetActiveButton();

        void Update();
    }
}
