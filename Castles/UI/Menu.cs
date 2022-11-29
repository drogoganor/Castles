using ImGuiNET;
using Castles.SampleBase;
using Veldrid;
using Castles.Interfaces;
using Castles.Providers;
using Castles.Render;
using System.Collections.Generic;
using Castles.Enums;

namespace Castles.UI
{
    public abstract class Menu : GameScreen
    {
        private readonly ModManifestProvider modManifestProvider;

        private ImGuiRenderer imGuiRenderer;
        private CommandList commandList;
        private bool isShown;
        private InputSnapshot inputSnapshot;

        protected virtual string GetTitle() => GetType().Name;

        protected Dictionary<FontSize, ImFontPtr?> Fonts = new();

        public Menu(
            ModManifestProvider modManifestProvider,
            IApplicationWindow window) : base(window)
        {
            this.modManifestProvider = modManifestProvider;
        }

        protected override void CreateResources(ResourceFactory factory)
        {
            commandList = ResourceFactory.CreateCommandList();
            imGuiRenderer = new ImGuiRenderer(
                GraphicsDevice,
                GraphicsDevice.MainSwapchain.Framebuffer.OutputDescription,
                (int)Window.Width,
                (int)Window.Height);

            foreach (var font in modManifestProvider.ModManifestFile.Fonts)
            {
                Fonts.Add(font.SizeEnum, ImGui.GetIO().Fonts.AddFontFromFileTTF(@"C:\Windows\Fonts\" + font.FontName, font.FontSize));
            }

            imGuiRenderer.RecreateFontDeviceTexture();
        }

        public override void Show()
        {
            isShown = true;
            base.Show();
        }

        public override void Hide()
        {
            isShown = false;
            base.Hide();
        }

        protected override void OnDeviceDestroyed()
        {
            base.OnDeviceDestroyed();

            imGuiRenderer = null;
            commandList = null;
            Fonts.Clear();
        }

        protected void PreDraw(float deltaSeconds)
        {
            if (imGuiRenderer == null) return;

            inputSnapshot = InputTracker.FrameSnapshot;
            imGuiRenderer.Update(1f / 60f, inputSnapshot);
        }

        protected override void Draw(float deltaSeconds)
        {
            if (imGuiRenderer == null) return;
            if (commandList == null) return;
            if (GraphicsDevice == null) return;

            if (isShown)
            {
                commandList.Begin();
                commandList.SetFramebuffer(GraphicsDevice.MainSwapchain.Framebuffer);
                commandList.ClearColorTarget(0, RgbaFloat.Black);
                imGuiRenderer.Render(GraphicsDevice, commandList);
                commandList.End();
                GraphicsDevice.SubmitCommands(commandList);
            }
        }

        protected override void HandleWindowResize()
        {
            if (imGuiRenderer == null) return;

            imGuiRenderer.WindowResized((int)Window.Width, (int)Window.Height);
        }

        protected static void HorizontallyCenteredText(string text, float width)
        {
            var textWidth = ImGui.CalcTextSize(text).X;

            ImGui.SetCursorPosX((width - textWidth) * 0.5f);
            ImGui.Text(text);
        }
    }
}
