using Castles.Data;
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
    public class Game : SampleApplication
    {
        public Action OnEndGame;

        private ModManifestProvider modManifestProvider;
        private GameResourcesProvider gameResourcesProvider;

        private readonly VertexPositionTexture2D[] vertices;
        private DeviceBuffer _projectionBuffer;
        private DeviceBuffer _viewBuffer;
        private DeviceBuffer _worldBuffer;
        private DeviceBuffer _vertexBuffer;
        private CommandList _cl;
        private Pipeline _pipeline;
        private ResourceSet _projViewSet;
        private ResourceSet _worldTextureSet;
        private TextureView _surfaceTextureView;
        private float _ticks;

        private InGameMenu inGameMenu;
        private GameMapProvider gameMapProvider;
        private Camera2D camera;

        public Game(
            IApplicationWindow window,
            Camera2D camera,
            ModManifestProvider modManifestProvider,
            GameResourcesProvider gameResourcesProvider,
            GameMapProvider gameMapProvider) : base(window, camera)
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
            _projectionBuffer = factory.CreateBuffer(new BufferDescription(64, BufferUsage.UniformBuffer));
            _viewBuffer = factory.CreateBuffer(new BufferDescription(64, BufferUsage.UniformBuffer));
            _worldBuffer = factory.CreateBuffer(new BufferDescription(64, BufferUsage.UniformBuffer));

            if (vertices != null)
            {
                _vertexBuffer = factory.CreateBuffer(new BufferDescription((uint)(VertexPositionTexture2D.SizeInBytes * vertices.Length), BufferUsage.VertexBuffer));
                GraphicsDevice.UpdateBuffer(_vertexBuffer, 0, vertices);
            }

            _surfaceTextureView = gameResourcesProvider.TextureView;

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

            _pipeline = factory.CreateGraphicsPipeline(new GraphicsPipelineDescription(
                BlendStateDescription.SingleOverrideBlend,
                DepthStencilStateDescription.DepthOnlyLessEqual,
                RasterizerStateDescription.Default,
                PrimitiveTopology.TriangleList,
                shaderSet,
                new[] { projViewLayout, worldTextureLayout },
                MainSwapchain.Framebuffer.OutputDescription));

            _projViewSet = factory.CreateResourceSet(new ResourceSetDescription(
                projViewLayout,
                _projectionBuffer,
                _viewBuffer));

            _worldTextureSet = factory.CreateResourceSet(new ResourceSetDescription(
                worldTextureLayout,
                _worldBuffer,
                _surfaceTextureView,
                GraphicsDevice.Aniso4xSampler));

            _cl = factory.CreateCommandList();

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

            // Render
            _ticks += deltaSeconds * 1000f;
            _cl.Begin();

            var projection = Matrix4x4.CreateOrthographic(
                Window.Width,
                Window.Height,
                0.5f,
                1000f);

            _cl.UpdateBuffer(_projectionBuffer, 0, projection);

            var direction = new Vector3(0, 0, 1f);

            var view = Matrix4x4.CreateLookAt(camera.Position, camera.Position + direction, Vector3.UnitY);

            _cl.UpdateBuffer(_viewBuffer, 0, view);
            //_cl.UpdateBuffer(_viewBuffer, 0, Matrix4x4.CreateRotationX(0.785398f) * Matrix4x4.CreateRotationY(0.523599f));

            _cl.UpdateBuffer(_worldBuffer, 0, Matrix4x4.Identity);

            _cl.SetFramebuffer(MainSwapchain.Framebuffer);
            _cl.ClearColorTarget(0, RgbaFloat.Black);
            _cl.ClearDepthStencil(1f);
            _cl.SetPipeline(_pipeline);
            _cl.SetVertexBuffer(0, _vertexBuffer);
            //_cl.SetIndexBuffer(_indexBuffer, IndexFormat.UInt16);
            _cl.SetGraphicsResourceSet(0, _projViewSet);
            _cl.SetGraphicsResourceSet(1, _worldTextureSet);
            _cl.Draw((uint)vertices.Length);
            //_cl.DrawIndexed(36, 1, 0, 0, 0);

            _cl.End();
            GraphicsDevice.SubmitCommands(_cl);
            GraphicsDevice.SwapBuffers(MainSwapchain);
            GraphicsDevice.WaitForIdle();
        }
    }
}
