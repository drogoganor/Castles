using Castles.Interfaces;
using Castles.Providers;
using Castles.Render;
using Castles.UI;
using System;

namespace Castles
{
    public class EditorClient : IGameClient
    {
        public Action OnEndGame;

        private readonly ModManifestProvider modManifestProvider;
        private readonly GameResourcesProvider gameResourcesProvider;

        private readonly Scene scene;

        private readonly EditorMainMenu mainMenu;
        private readonly IApplicationWindow window;

        public EditorClient(
            IApplicationWindow window,
            ModManifestProvider modManifestProvider,
            GameResourcesProvider gameResourcesProvider,
            EditorMainMenu mainMenu,
            Scene scene)
        {
            this.gameResourcesProvider = gameResourcesProvider;
            this.modManifestProvider = modManifestProvider;
            this.window = window;
            this.scene = scene;

            this.mainMenu = mainMenu;

            mainMenu.OnNewMap += HandleNewMap;
            OnEndGame += HandleEndGame;
        }

        public void Run()
        {
            mainMenu.Show();
            window.Run();
        }

        private void HandleEndGame()
        {
            scene.Hide();
            mainMenu.Show();
        }

        private void HandleNewMap()
        {
            mainMenu.Hide();
            scene.Show();
        }
    }
}
