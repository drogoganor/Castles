using ImGuiNET;
using System.Numerics;
using System;
using Castles.Interfaces;
using Castles.Providers;
using Castles.Enums;

namespace Castles.UI
{
    public class InGameMenu : Menu
    {
        public event Action OnEndGame;
        public event Action OnReturnToGame;

        public InGameMenu(
            IApplicationWindow window,
            ImGuiProvider imGuiProvider,
            GraphicsDeviceProvider graphicsDeviceProvider)
            : base(window, imGuiProvider, graphicsDeviceProvider)
        {
        }

        private void HandleEndGame()
        {
            OnEndGame?.Invoke();
        }

        private void HandleReturnToGame()
        {
            OnReturnToGame?.Invoke();
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
                HorizontallyCenteredText("Castles - Ingame Menu", menuSize.X);

                ImGui.SetCursorPosX(menuPadding / 2f);
                if (ImGui.Button("Return to Game", buttonSize))
                {
                    HandleReturnToGame();
                }

                ImGui.SetCursorPosX(menuPadding / 2f);
                if (ImGui.Button("End Game", buttonSize))
                {
                    HandleEndGame();
                }

                ImGui.SetCursorPosX(menuPadding / 2f);
                if (ImGui.Button("Quit", buttonSize))
                {
                    //ExitGame();
                }
            }

            base.Draw(deltaSeconds);
        }
    }
}
