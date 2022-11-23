using ImGuiNET;
using System.Numerics;
using System;
using Castles.Interfaces;
using Castles.Providers;

namespace Castles.UI
{
    public class EditorNewMapMenu : Menu
    {
        public event Action OnNewMap;

        public EditorNewMapMenu(
            ModManifestProvider modManifestProvider,
            IApplicationWindow window) : base(modManifestProvider, window)
        {
        }

        private void NewMap()
        {
            Hide();
            OnNewMap?.Invoke();
        }

        private void CancelNewMap()
        {
            Hide();
            Window.Close();
        }

        protected override void Draw(float deltaSeconds)
        {
            PreDraw(deltaSeconds);

            var windowSize = new Vector2(Window.Width, Window.Height);
            var menuSize = new Vector2(400, 600);
            var menuPadding = 40f;
            var buttonSize = new Vector2(menuSize.X - menuPadding, 32);
            ImGui.SetNextWindowSize(menuSize);

            var menuPos = (windowSize - menuSize) / 2;
            ImGui.SetNextWindowPos(menuPos);
            ImGui.PushFont(font.Value);

            if (ImGui.Begin("Main Menu",
                ImGuiWindowFlags.NoTitleBar |
                ImGuiWindowFlags.NoDecoration |
                ImGuiWindowFlags.NoBackground |
                ImGuiWindowFlags.NoCollapse |
                ImGuiWindowFlags.NoMove |
                ImGuiWindowFlags.NoResize))
            {
                HorizontallyCenteredText("Castles", menuSize.X);

                ImGui.SetCursorPosX(menuPadding / 2f);
                if (ImGui.Button("New Game", buttonSize))
                {
                    NewMap();
                }

                ImGui.SetCursorPosX(menuPadding / 2f);
                if (ImGui.Button("Quit", buttonSize))
                {
                    CancelNewMap();
                }
            }

            base.Draw(deltaSeconds);
        }
    }
}
