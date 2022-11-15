using Castles.Interfaces;
using Veldrid;

namespace Castles.SampleBase
{
    public abstract class BaseGame : IGameScreen
    {
        public IApplicationWindow Window { get; }
        protected GraphicsDevice GraphicsDevice { get; private set; }
        protected ResourceFactory ResourceFactory { get; private set; }
        protected Swapchain MainSwapchain { get; private set; }

        public BaseGame(IApplicationWindow window)
        {
            Window = window;
            Window.Resized += HandleWindowResize;
            Window.GraphicsDeviceCreated += OnGraphicsDeviceCreated;
            Window.GraphicsDeviceDestroyed += OnDeviceDestroyed;
        }

        public void Show()
        {
            Window.Rendering += Draw;
            Window.KeyPressed += OnKeyDown;
        }

        public void Hide()
        {
            Window.Rendering -= Draw;
            Window.KeyPressed -= OnKeyDown;
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
            GraphicsDevice = null;
            ResourceFactory = null;
            MainSwapchain = null;
        }

        protected abstract void CreateResources(ResourceFactory factory);

        protected abstract void Draw(float deltaSeconds);

        protected abstract void HandleWindowResize();

        protected virtual void OnKeyDown(KeyEvent ke) { }
    }
}
