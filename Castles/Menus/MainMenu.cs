using ImGuiNET;
using System.Numerics;
using System;
using Castles.Interfaces;
using Castles.Providers;
using Castles.Enums;
using Veldrid;

namespace Castles.UI
{
    /// <summary>
    /// TODO: Create separate "MainMenuScreen" to handle Show/Hide functions, incorporating MainMenu.
    /// MainMenuScreen should also call GraphicsDevice.SwapBuffers(MainSwapchain); GraphicsDevice.WaitForIdle();
    /// </summary>
    public class MainMenu : Menu
    {
        private readonly IApplicationWindow window;
        private readonly ImGuiProvider imGuiProvider;

        public event Action OnNewGame;
        public event Action OnExitGame;

        public MainMenu(
            IApplicationWindow window,
            ImGuiProvider imGuiProvider,
            GraphicsDeviceProvider graphicsDeviceProvider)
            : base(window, imGuiProvider, graphicsDeviceProvider)
        {
            this.imGuiProvider = imGuiProvider;
            this.window = window;
        }

        private void HandleNewGame()
        {
            OnNewGame?.Invoke();
        }

        private void HandleExitGame()
        {
            OnExitGame?.Invoke();
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
                HorizontallyCenteredText("Castles", menuSize.X);

                ImGui.SetCursorPosX(menuPadding / 2f);
                if (ImGui.Button("New Game", buttonSize))
                {
                    HandleNewGame();
                }

                ImGui.SetCursorPosX(menuPadding / 2f);
                if (ImGui.Button("Quit", buttonSize))
                {
                    HandleExitGame();
                }
            }

            base.Draw(deltaSeconds);
        }
    }
}
