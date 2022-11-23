using Castles.Data;
using Castles.Interfaces;
using Castles.Providers;
using Castles.Render;
using Castles.SampleBase;
using Castles.UI;
using System;
using Veldrid;

namespace Castles
{
    /// <summary>
    /// TODO: Refactor input handling
    /// </summary>
    public class GameClient : IGameClient
    {
        public Action OnEndGame;

        private readonly ModManifestProvider modManifestProvider;
        private readonly GameResourcesProvider gameResourcesProvider;

        private readonly Scene scene;

        private readonly MainMenu mainMenu;
        private readonly InGameMenu inGameMenu;
        private readonly IApplicationWindow window;

        public GameClient(
            IApplicationWindow window,
            ModManifestProvider modManifestProvider,
            GameResourcesProvider gameResourcesProvider,
            MainMenu mainMenu,
            Scene scene)
        {
            this.gameResourcesProvider = gameResourcesProvider;
            this.modManifestProvider = modManifestProvider;
            this.window = window;
            this.scene = scene;

            this.mainMenu = mainMenu;

            mainMenu.OnNewGame += HandleNewGame;
            OnEndGame += HandleEndGame;

            inGameMenu = new InGameMenu(modManifestProvider, window);
            inGameMenu.OnReturnToGame += InGameMenu_OnReturnToGame;
            inGameMenu.OnEndGame += InGameMenu_OnEndGame;

            window.KeyPressed += Window_KeyPressed;
        }

        private void Window_KeyPressed(KeyEvent obj)
        {
            // TODO: Should check if we're in the game
            // Amongst other things...
            if (obj.Key == Key.Escape)
            {
                ShowInGameMenu();
            }
        }

        public void Run()
        {
            mainMenu.Show();
            window.Run();

            inGameMenu.OnReturnToGame -= InGameMenu_OnReturnToGame;
            inGameMenu.OnEndGame -= InGameMenu_OnEndGame;
        }

        private void HandleEndGame()
        {
            scene.Hide();
            mainMenu.Show();
        }

        private void HandleNewGame()
        {
            mainMenu.Hide();
            scene.Show();
        }

        private void InGameMenu_OnEndGame()
        {
            scene.Hide();
            OnEndGame?.Invoke();
        }

        private void ShowInGameMenu()
        {
            scene.Hide();
            inGameMenu.Show();
        }

        private void InGameMenu_OnReturnToGame()
        {
            scene.Show();
        }
    }
}
