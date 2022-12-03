using System;
using Castles.Data.Files;
using Castles.Interfaces;
using Castles.Providers;
using Castles.Render;
using Castles.UI;
using Veldrid;

namespace Castles.Screens
{
    public class EditorNewMapMenuScreen : GameScreen
    {
        public event Action<GameMapFileHeader> OnCreateNewMap;
        public event Action OnCancelNewMap;

        private readonly EditorNewMapMenu editorNewMapMenu;
        private readonly IApplicationWindow window;

        public EditorNewMapMenuScreen(
            IApplicationWindow window,
            GraphicsDeviceProvider graphicsDeviceProvider,
            EditorNewMapMenu editorNewMapMenu)
            : base(window, graphicsDeviceProvider)
        {
            this.window = window;
            this.editorNewMapMenu = editorNewMapMenu;

            editorNewMapMenu.OnCreateNewMap += HandleCreateNewMap;
            editorNewMapMenu.OnCancelNewMap += HandleCancelNewMap;
        }

        private void HandleCreateNewMap(GameMapFileHeader header)
        {
            Hide();
            OnCreateNewMap?.Invoke(header);
        }

        private void HandleCancelNewMap()
        {
            Hide();
            OnCancelNewMap?.Invoke();
        }

        protected override void Draw(float deltaSeconds)
        {
            editorNewMapMenu.Draw(deltaSeconds);

            GraphicsDevice.SwapBuffers(MainSwapchain);
            GraphicsDevice.WaitForIdle();
        }
    }
}
