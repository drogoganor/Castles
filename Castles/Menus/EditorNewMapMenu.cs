using ImGuiNET;
using System.Numerics;
using System;
using Castles.Interfaces;
using Castles.Providers;
using Castles.Data.Files;
using Castles.Enums;

namespace Castles.UI
{
    public class EditorNewMapMenu : Menu
    {
        private const uint MAX_FILENAME = 128;
        private string filename = "test";
        private int sizeX = 128;
        private int sizeY = 128;

        public event Action<GameMapFileHeader> OnCreateNewMap;
        public event Action OnCancelNewMap;

        public EditorNewMapMenu(
            IApplicationWindow window,
            ImGuiProvider imGuiProvider,
            GraphicsDeviceProvider graphicsDeviceProvider)
            : base(window, imGuiProvider, graphicsDeviceProvider)
        {
        }

        private void HandleCreateNewMap()
        {
            OnCreateNewMap?.Invoke(new GameMapFileHeader
            {
                Name = filename,
                Size = new Vector2(sizeX, sizeY),
            });
        }

        private void HandleCancelNewMap()
        {
            OnCancelNewMap?.Invoke();
        }

        public override void Draw(float deltaSeconds)
        {
            UpdateInput(deltaSeconds);

            var windowSize = new Vector2(window.Width, window.Height);
            var menuSize = new Vector2(400, 600);
            var menuPadding = 40f;
            var buttonSize = new Vector2((menuSize.X / 2), 32);
            ImGui.SetNextWindowSize(menuSize);

            var menuPos = (windowSize - menuSize) / 2;
            ImGui.SetNextWindowPos(menuPos);
            ImGui.PushFont(imGuiProvider.Fonts[FontSize.Medium].Value);

            if (ImGui.Begin("Main Menu",
                ImGuiWindowFlags.NoTitleBar |
                ImGuiWindowFlags.NoDecoration |
                ImGuiWindowFlags.NoBackground |
                ImGuiWindowFlags.NoCollapse |
                ImGuiWindowFlags.NoMove |
                ImGuiWindowFlags.NoResize))
            {
                HorizontallyCenteredText("New Map", menuSize.X);

                ImGui.SetCursorPosX(menuPadding / 2f);
                ImGui.InputText("Filename", ref filename, MAX_FILENAME);

                ImGui.SetCursorPosX(menuPadding / 2f);
                ImGui.InputInt2("Size", ref sizeX);

                ImGui.SetCursorPosX(menuPadding / 2f);
                ImGui.BeginGroup();

                if (ImGui.Button("OK", buttonSize))
                {
                    HandleCreateNewMap();
                }

                ImGui.SameLine();

                if (ImGui.Button("Cancel", buttonSize))
                {
                    HandleCancelNewMap();
                }

                ImGui.EndGroup();
            }

            base.Draw(deltaSeconds);
        }
    }
}
