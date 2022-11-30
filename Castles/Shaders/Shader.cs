using Castles.Interfaces;
using Veldrid;

namespace Castles.Shaders
{
    public abstract class Shader
    {
        protected IApplicationWindow Window { get; private set; }
        protected GraphicsDevice GraphicsDevice { get; private set; }
        protected ResourceFactory ResourceFactory { get; private set; }
        protected Swapchain MainSwapchain { get; private set; }

        public Shader(IApplicationWindow window)
        {
            Window = window;
            Window.GraphicsDeviceCreated += OnGraphicsDeviceCreated;
            Window.GraphicsDeviceDestroyed += OnDeviceDestroyed;
        }

        public virtual void OnGraphicsDeviceCreated(GraphicsDevice gd, ResourceFactory factory, Swapchain sc)
        {
            GraphicsDevice = gd;
            ResourceFactory = factory;
            MainSwapchain = sc;
            CreateResources(factory);
        }

        protected virtual void OnDeviceDestroyed()
        {
            Window.GraphicsDeviceCreated -= OnGraphicsDeviceCreated;
            Window.GraphicsDeviceDestroyed -= OnDeviceDestroyed;
            GraphicsDevice = null;
            ResourceFactory = null;
            MainSwapchain = null;
        }

        protected abstract void CreateResources(ResourceFactory factory);
    }
}
