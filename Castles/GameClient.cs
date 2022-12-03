using Castles.Data;
using Castles.Interfaces;
using Castles.Providers;
using Castles.Render;
using Castles.SampleBase;
using Castles.Screens;
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

        private readonly InGameScreen inGameScreen;
        private readonly MainMenuScreen mainMenuScreen;
        private readonly InGameMenuScreen inGameMenuScreen;
        private readonly IApplicationWindow window;

        public GameClient(
            IApplicationWindow window,
            ModManifestProvider modManifestProvider,
            GameResourcesProvider gameResourcesProvider,
            MainMenuScreen mainMenuScreen,
            InGameMenuScreen inGameMenuScreen,
            InGameScreen inGameScreen)
        {
            this.gameResourcesProvider = gameResourcesProvider;
            this.modManifestProvider = modManifestProvider;
            this.window = window;
            this.inGameScreen = inGameScreen;
            this.mainMenuScreen = mainMenuScreen;
            this.inGameMenuScreen = inGameMenuScreen;

            mainMenuScreen.OnNewGame += HandleNewGame;
            OnEndGame += HandleEndGame;

            // inGameMenuScreen = new InGameMenuScreen(modManifestProvider, window);
            inGameMenuScreen.OnReturnToGame += InGameMenu_OnReturnToGame;
            inGameMenuScreen.OnEndGame += InGameMenu_OnEndGame;

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
            mainMenuScreen.Show();
            window.Run();

            inGameMenuScreen.OnReturnToGame -= InGameMenu_OnReturnToGame;
            inGameMenuScreen.OnEndGame -= InGameMenu_OnEndGame;
        }

        private void HandleEndGame()
        {
            inGameScreen.Hide();
            mainMenuScreen.Show();
        }

        private void HandleNewGame()
        {
            mainMenuScreen.Hide();
            inGameScreen.Show();
        }

        private void InGameMenu_OnEndGame()
        {
            inGameScreen.Hide();
            OnEndGame?.Invoke();
        }

        private void ShowInGameMenu()
        {
            inGameScreen.Hide();
            inGameMenuScreen.Show();
        }

        private void InGameMenu_OnReturnToGame()
        {
            inGameScreen.Show();
        }
    }
}
