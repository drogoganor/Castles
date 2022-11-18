using ImGuiNET;
using Castles.SampleBase;
using Veldrid;
using Castles.Interfaces;

namespace Castles.UI
{
    public abstract class Menu : GameScreen
    {
        private ImGuiRenderer imGuiRenderer;
        private CommandList commandList;
        private bool isShown;
        private InputSnapshot inputSnapshot;

        protected virtual string GetTitle() => GetType().Name;
        protected ImFontPtr? font;

        public Menu(IApplicationWindow window) : base(window)
        {
        }

        protected override void CreateResources(ResourceFactory factory)
        {
            commandList = ResourceFactory.CreateCommandList();
            imGuiRenderer = new ImGuiRenderer(
                GraphicsDevice,
                GraphicsDevice.MainSwapchain.Framebuffer.OutputDescription,
                (int)Window.Width,
                (int)Window.Height);

            font = ImGui.GetIO().Fonts.AddFontFromFileTTF(@"C:\Windows\Fonts\ARIAL.TTF", 26);
            imGuiRenderer.RecreateFontDeviceTexture();
        }

        //public void OnGraphicsDeviceCreated(GraphicsDevice gd, ResourceFactory factory, Swapchain sc)
        //{
        //    GraphicsDevice = gd;
        //    ResourceFactory = factory;
        //    MainSwapchain = sc;

        //    commandList = gd.ResourceFactory.CreateCommandList();
        //    imGuiRenderer = new ImGuiRenderer(
        //        gd,
        //        gd.MainSwapchain.Framebuffer.OutputDescription,
        //        (int)Window.Width,
        //        (int)Window.Height);

        //    font = ImGui.GetIO().Fonts.AddFontFromFileTTF(@"C:\Windows\Fonts\ARIAL.TTF", 26);
        //    imGuiRenderer.RecreateFontDeviceTexture();
        //}

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
            font = null;
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
                commandList.ClearColorTarget(0, new RgbaFloat(0, 0, 0.2f, 1f));
                imGuiRenderer.Render(GraphicsDevice, commandList);
                commandList.End();
                GraphicsDevice.SubmitCommands(commandList);
                GraphicsDevice.SwapBuffers(GraphicsDevice.MainSwapchain);
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
