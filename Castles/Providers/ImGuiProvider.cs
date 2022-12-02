using Castles.Enums;
using Castles.Interfaces;
using ImGuiNET;
using System.Collections.Generic;
using Veldrid;

namespace Castles.Providers
{
    public class ImGuiProvider
    {
        private readonly IApplicationWindow window;
        private readonly GraphicsDevice graphicsDevice;
        private readonly ModManifestProvider modManifestProvider;
        private readonly CommandList commandList;
        private readonly ImGuiRenderer imGuiRenderer;
        private readonly Dictionary<FontSize, ImFontPtr?> fonts = new();

        //private readonly DisposeCollectorResourceFactory resourceFactory;

        public Dictionary<FontSize, ImFontPtr?> Fonts => fonts;
        public CommandList CommandList => commandList;
        public ImGuiRenderer ImGuiRenderer => imGuiRenderer;

        public ImGuiProvider(
            IApplicationWindow window,
            GraphicsDeviceProvider graphicsDeviceProvider,
            ModManifestProvider modManifestProvider)
        {
            this.window = window;
            this.modManifestProvider = modManifestProvider;

            graphicsDevice = graphicsDeviceProvider.GraphicsDevice;

            commandList = graphicsDeviceProvider.ResourceFactory.CreateCommandList();
            imGuiRenderer = new ImGuiRenderer(
                graphicsDevice,
                graphicsDevice.MainSwapchain.Framebuffer.OutputDescription,
                (int)window.Width,
                (int)window.Height);

            foreach (var font in modManifestProvider.ModManifestFile.Fonts)
            {
                fonts.Add(font.SizeEnum, ImGui.GetIO().Fonts.AddFontFromFileTTF(@"C:\Windows\Fonts\" + font.FontName, font.FontSize));
            }

            imGuiRenderer.RecreateFontDeviceTexture();
        }
    }
}
