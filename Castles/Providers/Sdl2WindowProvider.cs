using Veldrid.Sdl2;
using Veldrid.StartupUtilities;

namespace Castles.Providers
{
    public class Sdl2WindowProvider
    {
        private readonly Sdl2Window window;

        public Sdl2Window Window => window;

        public Sdl2WindowProvider(SettingsProvider settingsProvider)
        {
            var settings = settingsProvider.Settings;
            var windowCreateInfo = new WindowCreateInfo
            {
                X = 100,
                Y = 100,
                WindowWidth = (int)settings.ScreenResolution.X,
                WindowHeight = (int)settings.ScreenResolution.Y,
                WindowTitle = "Castles",
            };

            window = VeldridStartup.CreateWindow(ref windowCreateInfo);
        }
    }
}
