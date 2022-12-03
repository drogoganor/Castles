using ImGuiNET;
using Castles.SampleBase;
using Veldrid;
using Castles.Interfaces;
using Castles.Providers;

namespace Castles.UI
{
    /// <summary>
    /// TODO: Handle device destroy
    /// </summary>
    public abstract class Menu : IRenderable
    {
        protected readonly IApplicationWindow window;
        protected readonly ImGuiProvider imGuiProvider;
        private readonly ImGuiRenderer imGuiRenderer;
        private readonly CommandList commandList;
        private readonly GraphicsDevice graphicsDevice;

        private InputSnapshot inputSnapshot;

        public Menu(
            IApplicationWindow window,
            ImGuiProvider imGuiProvider,
            GraphicsDeviceProvider graphicsDeviceProvider)
        {
            this.window = window;
            this.imGuiProvider = imGuiProvider;
            imGuiRenderer = imGuiProvider.ImGuiRenderer;
            commandList = imGuiProvider.CommandList;
            graphicsDevice = graphicsDeviceProvider.GraphicsDevice;

            window.Resized += HandleWindowResize;
        }

        //protected override void OnDeviceDestroyed()
        //{
        //    base.OnDeviceDestroyed();

        //    imGuiRenderer = null;
        //    commandList = null;
        //    Fonts.Clear();
        //}

        protected void UpdateInput(float deltaSeconds)
        {
            inputSnapshot = InputTracker.FrameSnapshot;
            imGuiRenderer.Update(1f / 60f, inputSnapshot);
        }

        public virtual void Draw(float deltaSeconds)
        {
            commandList.Begin();
            commandList.SetFramebuffer(graphicsDevice.MainSwapchain.Framebuffer);
            commandList.ClearColorTarget(0, RgbaFloat.Black);
            imGuiRenderer.Render(graphicsDevice, commandList);
            commandList.End();
            graphicsDevice.SubmitCommands(commandList);
        }

        protected virtual void HandleWindowResize()
        {
            imGuiRenderer.WindowResized((int)window.Width, (int)window.Height);
        }

        protected static void HorizontallyCenteredText(string text, float width)
        {
            var textWidth = ImGui.CalcTextSize(text).X;

            ImGui.SetCursorPosX((width - textWidth) * 0.5f);
            ImGui.Text(text);
        }
    }
}
