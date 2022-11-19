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
    public class GameClient : GameScreen, IGameClient
    {
        public Action OnEndGame;

        private ModManifestProvider modManifestProvider;
        private GameResourcesProvider gameResourcesProvider;

        private readonly Scene scene;

        private MainMenu mainMenu;
        private InGameMenu inGameMenu;

        public GameClient(
            IApplicationWindow window,
            ModManifestProvider modManifestProvider,
            GameResourcesProvider gameResourcesProvider,
            MainMenu mainMenu,
            Scene scene) : base(window)
        {
            this.gameResourcesProvider = gameResourcesProvider;
            this.modManifestProvider = modManifestProvider;
            this.scene = scene;

            this.mainMenu = mainMenu;

            inGameMenu = new InGameMenu(modManifestProvider, window);
            inGameMenu.OnReturnToGame += InGameMenu_OnReturnToGame;
            inGameMenu.OnEndGame += InGameMenu_OnEndGame;
            this.mainMenu = mainMenu;
        }

        public void Run()
        {
            mainMenu.Show();
            mainMenu.OnNewGame += () =>
            {
                mainMenu.Hide();
                Show();
                scene.Show();
                OnEndGame += () =>
                {
                    Hide();
                    scene.Hide();
                    mainMenu.Show();
                };
            };

            Window.Run();
        }

        private void InGameMenu_OnEndGame()
        {
            Hide();
            scene.Hide();
            OnEndGame?.Invoke();
        }

        private void ShowInGameMenu()
        {
            Hide();
            scene.Hide();
            inGameMenu.Show();
        }

        private void InGameMenu_OnReturnToGame()
        {
            Show();
            scene.Show();
        }

        protected unsafe override void CreateResources(ResourceFactory factory)
        {
        }

        protected override void HandleWindowResize()
        {
        }

        protected override void OnDeviceDestroyed()
        {
            base.OnDeviceDestroyed();

            inGameMenu.OnReturnToGame -= InGameMenu_OnReturnToGame;
            inGameMenu.OnEndGame -= InGameMenu_OnEndGame;
        }

        protected override void Draw(float deltaSeconds)
        {
            // TODO: Place in Update/PreDraw
            if (InputTracker.GetKey(Key.Escape))
            {
                ShowInGameMenu();
            }
        }
    }
}
