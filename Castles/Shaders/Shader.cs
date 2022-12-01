using Castles.Interfaces;
using Castles.Providers;
using Veldrid;

namespace Castles.Shaders
{
    public abstract class Shader
    {
        protected IApplicationWindow Window { get; private set; }
        protected GraphicsDevice GraphicsDevice { get; private set; }
        protected ResourceFactory ResourceFactory { get; private set; }
        protected Swapchain MainSwapchain { get; private set; }

        public Shader(IApplicationWindow window,
            GraphicsDeviceProvider graphicsDeviceProvider)
        {
            Window = window;

            GraphicsDevice = graphicsDeviceProvider.GraphicsDevice;
            ResourceFactory = graphicsDeviceProvider.ResourceFactory;
            MainSwapchain = graphicsDeviceProvider.GraphicsDevice.MainSwapchain;

            //Window.GraphicsDeviceCreated += OnGraphicsDeviceCreated;
            Window.GraphicsDeviceDestroyed += OnDeviceDestroyed;
        }

        protected virtual void OnDeviceDestroyed()
        {
            Window.GraphicsDeviceDestroyed -= OnDeviceDestroyed;
            GraphicsDevice = null;
            ResourceFactory = null;
            MainSwapchain = null;
        }
    }
}
