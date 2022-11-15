using Castles.Data;
using Castles.Interfaces;
using Castles.Providers;
using Castles.SampleBase;
using Castles.UI;
using System;
using System.IO;
using System.Numerics;
using Veldrid;
using Veldrid.SPIRV;

namespace Castles
{
    public class Game : BaseGame
    {
        public Action OnEndGame;

        private ModManifestProvider modManifestProvider;
        private GameResourcesProvider gameResourcesProvider;

        private readonly VertexPositionTexture2D[] vertices;
        private DeviceBuffer projectionBuffer;
        private DeviceBuffer viewBuffer;
        private DeviceBuffer worldBuffer;
        private DeviceBuffer vertexBuffer;
        private CommandList commandList;
        private Pipeline pipeline;
        private ResourceSet projectionViewSet;
        private ResourceSet worldTextureSet;
        private TextureView surfaceTextureView;

        private InGameMenu inGameMenu;
        private GameMapProvider gameMapProvider;
        private Camera2D camera;

        public Game(
            IApplicationWindow window,
            Camera2D camera,
            ModManifestProvider modManifestProvider,
            GameResourcesProvider gameResourcesProvider,
            GameMapProvider gameMapProvider) : base(window)
        {
            this.camera = camera;
            this.gameResourcesProvider = gameResourcesProvider;
            this.modManifestProvider = modManifestProvider;

            vertices = gameMapProvider.GetVertexArray();

            inGameMenu = new InGameMenu(window);
            inGameMenu.OnReturnToGame += InGameMenu_OnReturnToGame;
            inGameMenu.OnEndGame += InGameMenu_OnEndGame;
        }

        private void InGameMenu_OnEndGame()
        {
            Hide();
            OnEndGame?.Invoke();
        }

        private void ShowInGameMenu()
        {
            Hide();
            inGameMenu.Show();
        }

        private void InGameMenu_OnReturnToGame()
        {
            Show();
        }

        protected unsafe override void CreateResources(ResourceFactory factory)
        {
            projectionBuffer = factory.CreateBuffer(new BufferDescription(64, BufferUsage.UniformBuffer));
            viewBuffer = factory.CreateBuffer(new BufferDescription(64, BufferUsage.UniformBuffer));
            worldBuffer = factory.CreateBuffer(new BufferDescription(64, BufferUsage.UniformBuffer));

            if (vertices != null)
            {
                vertexBuffer = factory.CreateBuffer(new BufferDescription((uint)(VertexPositionTexture2D.SizeInBytes * vertices.Length), BufferUsage.VertexBuffer));
                GraphicsDevice.UpdateBuffer(vertexBuffer, 0, vertices);
            }

            surfaceTextureView = gameResourcesProvider.TextureView;

            var shadersPath = Path.Combine(Environment.CurrentDirectory, @"Content/shader");
            var vertexShaderBytes = File.ReadAllBytes(Path.Combine(shadersPath, "World.vert.spv"));
            var fragmentShaderBytes = File.ReadAllBytes(Path.Combine(shadersPath, "World.frag.spv"));

            ShaderSetDescription shaderSet = new ShaderSetDescription(
                new[]
                {
                    new VertexLayoutDescription(
                        new VertexElementDescription("Position", VertexElementSemantic.TextureCoordinate, VertexElementFormat.Float2),
                        new VertexElementDescription("TexCoords", VertexElementSemantic.TextureCoordinate, VertexElementFormat.Float3))
                },
                factory.CreateFromSpirv(
                    new ShaderDescription(ShaderStages.Vertex, vertexShaderBytes, "main"),
                    new ShaderDescription(ShaderStages.Fragment, fragmentShaderBytes, "main")));

            ResourceLayout projViewLayout = factory.CreateResourceLayout(
                new ResourceLayoutDescription(
                    new ResourceLayoutElementDescription("ProjectionBuffer", ResourceKind.UniformBuffer, ShaderStages.Vertex),
                    new ResourceLayoutElementDescription("ViewBuffer", ResourceKind.UniformBuffer, ShaderStages.Vertex)));

            ResourceLayout worldTextureLayout = factory.CreateResourceLayout(
                new ResourceLayoutDescription(
                    new ResourceLayoutElementDescription("WorldBuffer", ResourceKind.UniformBuffer, ShaderStages.Vertex),
                    new ResourceLayoutElementDescription("SurfaceTexture", ResourceKind.TextureReadOnly, ShaderStages.Fragment),
                    new ResourceLayoutElementDescription("SurfaceSampler", ResourceKind.Sampler, ShaderStages.Fragment)));

            pipeline = factory.CreateGraphicsPipeline(new GraphicsPipelineDescription(
                BlendStateDescription.SingleOverrideBlend,
                DepthStencilStateDescription.DepthOnlyLessEqual,
                RasterizerStateDescription.Default,
                PrimitiveTopology.TriangleList,
                shaderSet,
                new[] { projViewLayout, worldTextureLayout },
                MainSwapchain.Framebuffer.OutputDescription));

            projectionViewSet = factory.CreateResourceSet(new ResourceSetDescription(
                projViewLayout,
                projectionBuffer,
                viewBuffer));

            worldTextureSet = factory.CreateResourceSet(new ResourceSetDescription(
                worldTextureLayout,
                worldBuffer,
                surfaceTextureView,
                GraphicsDevice.Aniso4xSampler));

            commandList = factory.CreateCommandList();
        }

        protected override void HandleWindowResize()
        {
            camera.WindowResized(Window.Width, Window.Height);
        }

        protected override void OnDeviceDestroyed()
        {
            base.OnDeviceDestroyed();

            inGameMenu.OnReturnToGame -= InGameMenu_OnReturnToGame;
            inGameMenu.OnEndGame -= InGameMenu_OnEndGame;
        }

        protected override void Draw(float deltaSeconds)
        {
            // TODO: Place in Update/PreDraw
            if (InputTracker.GetKey(Key.Escape))
            {
                ShowInGameMenu();
            }

            camera.Update(deltaSeconds);

            if (vertices == null)
            {
                return;
            }

            commandList.Begin();

            var projection = Matrix4x4.CreateOrthographic(
                Window.Width,
                Window.Height,
                0.5f,
                1000f);

            commandList.UpdateBuffer(projectionBuffer, 0, projection);

            var direction = new Vector3(0, 0, 1f);

            var view = Matrix4x4.CreateLookAt(camera.Position, camera.Position + direction, Vector3.UnitY);

            commandList.UpdateBuffer(viewBuffer, 0, view);

            commandList.UpdateBuffer(worldBuffer, 0, Matrix4x4.Identity);

            commandList.SetFramebuffer(MainSwapchain.Framebuffer);
            commandList.ClearColorTarget(0, RgbaFloat.Black);
            commandList.ClearDepthStencil(1f);
            commandList.SetPipeline(pipeline);
            commandList.SetVertexBuffer(0, vertexBuffer);
            commandList.SetGraphicsResourceSet(0, projectionViewSet);
            commandList.SetGraphicsResourceSet(1, worldTextureSet);
            commandList.Draw((uint)vertices.Length);

            commandList.End();
            GraphicsDevice.SubmitCommands(commandList);
            GraphicsDevice.SwapBuffers(MainSwapchain);
            GraphicsDevice.WaitForIdle();
        }
    }
}
