using System;
using Castles.Data.Files;
using Castles.Interfaces;
using Castles.Providers;
using Castles.Render;
using Castles.UI;
using Veldrid;

namespace Castles.Screens
{
    public class EditorScreen : GameScreen
    {
        public event Action OnExit;

        private readonly IApplicationWindow window;
        private readonly EditorUI editorUI;
        private readonly Scene scene;
        private readonly DebugGrid debugGrid;

        public EditorScreen(
            IApplicationWindow window,
            GraphicsDeviceProvider graphicsDeviceProvider,
            Scene scene,
            DebugGrid debugGrid,
            EditorUI editorUI)
            : base(window, graphicsDeviceProvider)
        {
            this.window = window;
            this.debugGrid = debugGrid;
            this.scene = scene;
            this.editorUI = editorUI;

            editorUI.OnExit += HandleExit;

            scene.ClearColor = false;
        }

        private void HandleExit()
        {
            Hide();
            OnExit?.Invoke();
        }

        protected override void Draw(float deltaSeconds)
        {
            editorUI.Draw(deltaSeconds);
            scene.Draw(deltaSeconds);
            debugGrid.Draw(deltaSeconds);

            GraphicsDevice.SwapBuffers(MainSwapchain);
            GraphicsDevice.WaitForIdle();
        }
    }
}
