using Castles.Interfaces;
using Veldrid;

namespace Castles.Render
{
    public abstract class GameScreen : IGameScreen
    {
        protected IApplicationWindow Window { get; }
        protected GraphicsDevice GraphicsDevice { get; private set; }
        protected ResourceFactory ResourceFactory { get; private set; }
        protected Swapchain MainSwapchain { get; private set; }

        public GameScreen(IApplicationWindow window)
        {
            Window = window;
            Window.Resized += HandleWindowResize;
            //Window.GraphicsDeviceCreated += OnGraphicsDeviceCreated;
            Window.GraphicsDeviceDestroyed += OnDeviceDestroyed;
        }

        public virtual void Show()
        {
            Window.Rendering += Draw;
            Window.PostRender += PostDraw;
            Window.KeyPressed += OnKeyDown;
        }

        public virtual void Hide()
        {
            Window.Rendering -= Draw;
            Window.PostRender -= PostDraw;
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
            Hide();
            Window.Resized -= HandleWindowResize;
            //Window.GraphicsDeviceCreated -= OnGraphicsDeviceCreated;
            Window.GraphicsDeviceDestroyed -= OnDeviceDestroyed;
            GraphicsDevice = null;
            ResourceFactory = null;
            MainSwapchain = null;
        }

        protected abstract void CreateResources(ResourceFactory factory);

        protected abstract void Draw(float deltaSeconds);

        protected virtual void PostDraw()
        {
            GraphicsDevice.SwapBuffers(MainSwapchain);
            GraphicsDevice.WaitForIdle();
        }

        protected abstract void HandleWindowResize();

        protected virtual void OnKeyDown(KeyEvent ke) { }
    }
}
