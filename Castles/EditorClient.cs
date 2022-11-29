using Castles.Data.Files;
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

        private readonly EditorMainMenu mainMenu;
        private readonly EditorNewMapMenu newMapMenu;
        private readonly EditorUI editorUI;
        private readonly IApplicationWindow window;

        public EditorClient(
            IApplicationWindow window,
            ModManifestProvider modManifestProvider,
            GameResourcesProvider gameResourcesProvider,
            EditorMainMenu mainMenu,
            EditorNewMapMenu newMapMenu,
            EditorUI editorUI)
        {
            this.gameResourcesProvider = gameResourcesProvider;
            this.modManifestProvider = modManifestProvider;
            this.window = window;

            this.mainMenu = mainMenu;
            this.newMapMenu = newMapMenu;
            this.editorUI = editorUI;

            mainMenu.OnNewMap += HandleNewMap;
            OnEndGame += HandleEndGame;
            newMapMenu.OnCancelNewMap += NewMapMenu_OnCancelNewMap;
            newMapMenu.OnCreateNewMap += NewMapMenu_OnCreateNewMap;
        }

        private void NewMapMenu_OnCreateNewMap(GameMapFileHeader mapHeader)
        {
            // TODO: Create new map file and save
            // Then create new editor gamestate
            editorUI.Show();
        }

        private void NewMapMenu_OnCancelNewMap()
        {
            mainMenu.Show();
        }

        public void Run()
        {
            mainMenu.Show();
            window.Run();
        }

        private void HandleEndGame()
        {
            mainMenu.Show();
        }

        private void HandleNewMap()
        {
            newMapMenu.Show();
        }
    }
}
