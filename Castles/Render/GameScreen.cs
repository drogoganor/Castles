using Castles.Interfaces;
using Castles.Providers;
using Veldrid;

namespace Castles.Render
{
    /// <summary>
    /// This should be split out into:
    /// - IRenderable: Has a Draw() method
    /// - IGameScreen: Inherits IRender, Has Show() and Hide(), only one is rendered at a time.
    /// Reasons: 
    /// 1) Conditionally Showing and Hiding items in response to input will make our components render in a different order.
    /// 2) We need to "batch up" render items and conclude rendering with: GraphicsDevice.SwapBuffers(MainSwapchain); GraphicsDevice.WaitForIdle();
    /// 
    /// This existing Show/Hide event setup seems to be designed for mutually exclusive Screens that never render in concert with one another.
    /// </summary>
    public abstract class GameScreen : IGameScreen
    {
        protected IApplicationWindow Window { get; }
        protected GraphicsDevice GraphicsDevice { get; private set; }
        protected ResourceFactory ResourceFactory { get; private set; }
        protected Swapchain MainSwapchain { get; private set; }

        public GameScreen(IApplicationWindow window,
            GraphicsDeviceProvider graphicsDeviceProvider)
        {
            Window = window;
            GraphicsDevice = graphicsDeviceProvider.GraphicsDevice;
            ResourceFactory= graphicsDeviceProvider.ResourceFactory;
            MainSwapchain = graphicsDeviceProvider.GraphicsDevice.MainSwapchain;

            //Window.GraphicsDeviceCreated += OnGraphicsDeviceCreated;
            //Window.GraphicsDeviceDestroyed += OnDeviceDestroyed;
        }

        public virtual void Show()
        {
            Window.Rendering += Draw;
            Window.KeyPressed += OnKeyDown;
        }

        public virtual void Hide()
        {
            Window.Rendering -= Draw;
            Window.KeyPressed -= OnKeyDown;
        }

        //public virtual void OnGraphicsDeviceCreated(GraphicsDevice gd, ResourceFactory factory, Swapchain sc)
        //{
        //    GraphicsDevice = gd;
        //    ResourceFactory = factory;
        //    MainSwapchain = sc;
        //    CreateResources(factory);
        //}

        //protected virtual void OnDeviceDestroyed()
        //{
        //    Hide();
        //    Window.Resized -= HandleWindowResize;
        //    //Window.GraphicsDeviceCreated -= OnGraphicsDeviceCreated;
        //    Window.GraphicsDeviceDestroyed -= OnDeviceDestroyed;
        //    GraphicsDevice = null;
        //    ResourceFactory = null;
        //    MainSwapchain = null;
        //}

        //protected abstract void CreateResources(ResourceFactory factory);

        protected abstract void Draw(float deltaSeconds);

        //protected virtual void PostDraw()
        //{
        //    GraphicsDevice.SwapBuffers(MainSwapchain);
        //    GraphicsDevice.WaitForIdle();
        //}

        protected virtual void OnKeyDown(KeyEvent ke) { }
    }
}
