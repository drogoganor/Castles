using Castles.Enums;
using Castles.Interfaces;
using Castles.Providers;
using System;
using System.Diagnostics;
using Veldrid;
using Veldrid.Sdl2;
using Veldrid.Utilities;

namespace Castles.SampleBase
{
    public class VeldridStartupWindow : IApplicationWindow
    {
        private readonly Sdl2Window window;
        private readonly GraphicsDevice graphicsDevice;
        private DisposeCollectorResourceFactory factory;
        private bool windowResized = true;

        public event Action<float> Rendering;
        public event Action PostRender;
        public event Action<GraphicsDevice, ResourceFactory, Swapchain> GraphicsDeviceCreated;
        public event Action GraphicsDeviceDestroyed;
        public event Action Resized;
        public event Action<KeyEvent> KeyPressed;

        public uint Width => (uint)window.Width;
        public uint Height => (uint)window.Height;

        public PlatformType PlatformType => PlatformType.Desktop;

        public VeldridStartupWindow(
            Sdl2WindowProvider sdl2WindowProvider,
            GraphicsDeviceProvider graphicsDeviceProvider)
        {
            window = sdl2WindowProvider.Window;
            window.Resized += () =>
            {
                windowResized = true;
            };

            window.KeyDown += OnKeyDown;

            graphicsDevice = graphicsDeviceProvider.GraphicsDevice;
        }

        public void Run()
        {
            factory = new DisposeCollectorResourceFactory(graphicsDevice.ResourceFactory);

            GraphicsDeviceCreated?.Invoke(graphicsDevice, factory, graphicsDevice.MainSwapchain);

            var sw = Stopwatch.StartNew();
            var previousElapsed = sw.Elapsed.TotalSeconds;

            while (window.Exists)
            {
                double newElapsed = sw.Elapsed.TotalSeconds;
                float deltaSeconds = (float)(newElapsed - previousElapsed);

                var inputSnapshot = window.PumpEvents();
                InputTracker.UpdateFrameInput(inputSnapshot);

                if (window.Exists)
                {
                    previousElapsed = newElapsed;
                    if (windowResized)
                    {
                        windowResized = false;
                        graphicsDevice.ResizeMainWindow((uint)window.Width, (uint)window.Height);
                        Resized?.Invoke();
                    }

                    Rendering?.Invoke(deltaSeconds);
                    PostRender?.Invoke();
                }
            }

            graphicsDevice.WaitForIdle();
            factory.DisposeCollector.DisposeAll();
            graphicsDevice.Dispose();
            GraphicsDeviceDestroyed?.Invoke();
        }

        protected void OnKeyDown(KeyEvent keyEvent)
        {
            KeyPressed?.Invoke(keyEvent);
        }

        public void Close()
        {
            window.Close();
        }
    }
}
