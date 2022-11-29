using System;
using Castles.Enums;
using Veldrid;

namespace Castles.Interfaces
{
    public interface IApplicationWindow
    {
        PlatformType PlatformType { get; }

        event Action<float> Rendering;
        event Action PostRender;
        event Action<GraphicsDevice, ResourceFactory, Swapchain> GraphicsDeviceCreated;
        event Action GraphicsDeviceDestroyed;
        event Action Resized;
        event Action<KeyEvent> KeyPressed;

        uint Width { get; }
        uint Height { get; }

        void Run();
        void Close();
    }
}
