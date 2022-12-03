using Castles.Data.Files;
using Castles.Interfaces;
using Castles.Providers;
using Castles.Render;
using Castles.Screens;
using Castles.UI;
using System;

namespace Castles
{
    public class EditorClient : IGameClient
    {
        public Action OnEndGame;

        private readonly ModManifestProvider modManifestProvider;
        private readonly GameResourcesProvider gameResourcesProvider;

        private readonly EditorMainMenuScreen editorMainMenuScreen;
        private readonly EditorNewMapMenuScreen newMapMenuScreen;
        private readonly EditorScreen editorScreen;
        private readonly IApplicationWindow window;

        public EditorClient(
            IApplicationWindow window,
            ModManifestProvider modManifestProvider,
            GameResourcesProvider gameResourcesProvider,
            EditorMainMenuScreen editorMainMenuScreen,
            EditorNewMapMenuScreen newMapMenuScreen,
            EditorScreen editorScreen)
        {
            this.gameResourcesProvider = gameResourcesProvider;
            this.modManifestProvider = modManifestProvider;
            this.window = window;

            this.editorMainMenuScreen = editorMainMenuScreen;
            this.newMapMenuScreen = newMapMenuScreen;
            this.editorScreen = editorScreen;

            editorMainMenuScreen.OnNewMap += HandleNewMap;
            OnEndGame += HandleEndGame;
            newMapMenuScreen.OnCancelNewMap += NewMapMenu_OnCancelNewMap;
            newMapMenuScreen.OnCreateNewMap += NewMapMenu_OnCreateNewMap;
        }

        private void NewMapMenu_OnCreateNewMap(GameMapFileHeader mapHeader)
        {
            // TODO: Create new map file and save
            // Then create new editor gamestate
            editorScreen.Show();
        }

        private void NewMapMenu_OnCancelNewMap()
        {
            editorMainMenuScreen.Show();
        }

        public void Run()
        {
            editorMainMenuScreen.Show();
            window.Run();
        }

        private void HandleEndGame()
        {
            editorMainMenuScreen.Show();
        }

        private void HandleNewMap()
        {
            newMapMenuScreen.Show();
        }
    }
}
