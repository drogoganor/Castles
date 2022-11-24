using ImGuiNET;
using System.Numerics;
using System;
using Castles.Interfaces;
using Castles.Providers;

namespace Castles.UI
{
    public class EditorMainMenu : Menu
    {
        public event Action OnNewMap;
        public event Action OnEditMap;

        public EditorMainMenu(
            ModManifestProvider modManifestProvider,
            IApplicationWindow window) : base(modManifestProvider, window)
        {
        }

        private void HandleNewMap()
        {
            Hide();
            OnNewMap?.Invoke();
        }

        private void HandleEditMap()
        {
            Hide();
            OnEditMap?.Invoke();
        }

        private void ExitEditor()
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
                HorizontallyCenteredText("Castles - Map Editor", menuSize.X);

                ImGui.SetCursorPosX(menuPadding / 2f);
                if (ImGui.Button("New Map", buttonSize))
                {
                    HandleNewMap();
                }

                ImGui.SetCursorPosX(menuPadding / 2f);
                if (ImGui.Button("Edit Map", buttonSize))
                {
                    HandleEditMap();
                }

                ImGui.SetCursorPosX(menuPadding / 2f);
                if (ImGui.Button("Quit", buttonSize))
                {
                    ExitEditor();
                }
            }

            base.Draw(deltaSeconds);
        }
    }
}
