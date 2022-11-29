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
            ModManifestProvider modManifestProvider,
            IApplicationWindow window) : base(modManifestProvider, window)
        {
        }

        private void EndGame()
        {
            Hide();
            OnEndGame?.Invoke();
        }

        private void ReturnToGame()
        {
            Hide();
            OnReturnToGame?.Invoke();
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
            ImGui.PushFont(Fonts[FontSize.Large].Value);

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
                    ReturnToGame();
                }

                ImGui.SetCursorPosX(menuPadding / 2f);
                if (ImGui.Button("End Game", buttonSize))
                {
                    EndGame();
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
