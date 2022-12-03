using System;
using Castles.Data.Files;
using Castles.Interfaces;
using Castles.Providers;
using Castles.Render;
using Castles.UI;
using Veldrid;

namespace Castles.Screens
{
    public class InGameScreen : GameScreen
    {
        public event Action OnExit;

        private readonly IApplicationWindow window;
        private readonly Scene scene;

        public InGameScreen(
            IApplicationWindow window,
            GraphicsDeviceProvider graphicsDeviceProvider,
            Scene scene)
            : base(window, graphicsDeviceProvider)
        {
            this.window = window;
            this.scene = scene;
        }

        private void HandleExit()
        {
            Hide();
            OnExit?.Invoke();
        }

        protected override void Draw(float deltaSeconds)
        {
            scene.Draw(deltaSeconds);

            GraphicsDevice.SwapBuffers(MainSwapchain);
            GraphicsDevice.WaitForIdle();
        }
    }
}
