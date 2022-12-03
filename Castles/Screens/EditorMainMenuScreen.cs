using System;
using Castles.Interfaces;
using Castles.Providers;
using Castles.Render;
using Castles.UI;
using Veldrid;

namespace Castles.Screens
{
    public class EditorMainMenuScreen : GameScreen
    {
        public event Action OnNewMap;
        public event Action OnEditMap;
        public event Action OnExitEditor;

        private readonly EditorMainMenu editorMainMenu;
        private readonly IApplicationWindow window;

        public EditorMainMenuScreen(
            IApplicationWindow window,
            GraphicsDeviceProvider graphicsDeviceProvider,
            EditorMainMenu editorMainMenu)
            : base(window, graphicsDeviceProvider)
        {
            this.window = window;
            this.editorMainMenu = editorMainMenu;

            editorMainMenu.OnNewMap += HandleNewMap;
            editorMainMenu.OnExitEditor += HandleExitEditor;
        }

        private void HandleNewMap()
        {
            Hide();
            OnNewMap?.Invoke();
        }

        private void HandleExitEditor()
        {
            Hide();
            OnExitEditor?.Invoke();
            window.Close();
        }

        protected override void Draw(float deltaSeconds)
        {
            editorMainMenu.Draw(deltaSeconds);

            GraphicsDevice.SwapBuffers(MainSwapchain);
            GraphicsDevice.WaitForIdle();
        }
    }
}
