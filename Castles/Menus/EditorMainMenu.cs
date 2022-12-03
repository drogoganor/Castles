using ImGuiNET;
using System.Numerics;
using System;
using Castles.Interfaces;
using Castles.Providers;
using Castles.Enums;

namespace Castles.UI
{
    public class EditorMainMenu : Menu
    {
        public event Action OnNewMap;
        public event Action OnEditMap;
        public event Action OnExitEditor;

        public EditorMainMenu(
            IApplicationWindow window,
            ImGuiProvider imGuiProvider,
            GraphicsDeviceProvider graphicsDeviceProvider)
            : base(window, imGuiProvider, graphicsDeviceProvider)
        {
        }

        private void HandleNewMap()
        {
            OnNewMap?.Invoke();
        }

        private void HandleEditMap()
        {
            OnEditMap?.Invoke();
        }

        private void HandleExitEditor()
        {
            OnExitEditor?.Invoke();
        }

        public override void Draw(float deltaSeconds)
        {
            UpdateInput(deltaSeconds);

            var windowSize = new Vector2(window.Width, window.Height);
            var menuSize = new Vector2(400, 600);
            var menuPadding = 40f;
            var buttonSize = new Vector2(menuSize.X - menuPadding, 32);
            ImGui.SetNextWindowSize(menuSize);

            var menuPos = (windowSize - menuSize) / 2;
            ImGui.SetNextWindowPos(menuPos);
            ImGui.PushFont(imGuiProvider.Fonts[FontSize.Large].Value);

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
                    HandleExitEditor();
                }
            }

            base.Draw(deltaSeconds);
        }
    }
}
