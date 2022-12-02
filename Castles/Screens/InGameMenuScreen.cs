using System;
using Castles.Interfaces;
using Castles.Providers;
using Castles.Render;
using Castles.UI;
using Veldrid;

namespace Castles.Screens
{
    public class InGameMenuScreen : GameScreen
    {
        public event Action OnEndGame;
        public event Action OnReturnToGame;

        private readonly InGameMenu inGameMenu;

        public InGameMenuScreen(
            IApplicationWindow window,
            GraphicsDeviceProvider graphicsDeviceProvider,
            InGameMenu inGameMenu)
            : base(window, graphicsDeviceProvider)
        {
            this.inGameMenu = inGameMenu;

            inGameMenu.OnEndGame += HandleEndGame;
            inGameMenu.OnReturnToGame += HandleReturnToGame;
        }

        private void HandleEndGame()
        {
            Hide();
            OnEndGame?.Invoke();
        }

        private void HandleReturnToGame()
        {
            Hide();
            OnReturnToGame?.Invoke();
        }

        protected override void Draw(float deltaSeconds)
        {
            inGameMenu.Draw(deltaSeconds);

            GraphicsDevice.SwapBuffers(MainSwapchain);
            GraphicsDevice.WaitForIdle();
        }
    }
}
