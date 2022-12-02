using System;
using Castles.Interfaces;
using Castles.Providers;
using Castles.Render;
using Castles.UI;
using Veldrid;

namespace Castles.Screens
{
    public class MainMenuScreen : GameScreen
    {
        public event Action OnNewGame;
        public event Action OnExitGame;

        private readonly MainMenu mainMenu;
        private readonly IApplicationWindow window;

        public MainMenuScreen(
            IApplicationWindow window,
            GraphicsDeviceProvider graphicsDeviceProvider,
            MainMenu mainMenu)
            : base(window, graphicsDeviceProvider)
        {
            this.window = window;
            this.mainMenu = mainMenu;

            mainMenu.OnNewGame += HandleNewGame;
            mainMenu.OnExitGame += HandleExitGame;
        }

        private void HandleNewGame()
        {
            Hide();
            OnNewGame?.Invoke();
        }

        private void HandleExitGame()
        {
            Hide();
            OnExitGame?.Invoke();
            window.Close();
        }

        protected override void Draw(float deltaSeconds)
        {
            mainMenu.Draw(deltaSeconds);

            GraphicsDevice.SwapBuffers(MainSwapchain);
            GraphicsDevice.WaitForIdle();
        }
    }
}
