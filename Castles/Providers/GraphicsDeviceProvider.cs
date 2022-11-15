using Veldrid;
using Veldrid.StartupUtilities;

namespace Castles.Providers
{
    public class GraphicsDeviceProvider
    {
        private readonly GraphicsDevice graphicsDevice;

        public GraphicsDevice GraphicsDevice => graphicsDevice;

        public GraphicsDeviceProvider(Sdl2WindowProvider sdl2WindowProvider)
        {
            var options = new GraphicsDeviceOptions(
                debug: false,
                swapchainDepthFormat: PixelFormat.R16_UNorm,
                syncToVerticalBlank: true,
                resourceBindingModel: ResourceBindingModel.Improved,
                preferDepthRangeZeroToOne: true,
                preferStandardClipSpaceYDirection: true);
#if DEBUG
            options.Debug = true;
#endif
            graphicsDevice = VeldridStartup.CreateGraphicsDevice(sdl2WindowProvider.Window, options, GraphicsBackend.Direct3D11);
        }
    }
}
