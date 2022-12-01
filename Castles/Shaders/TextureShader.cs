using Castles.Interfaces;
using Castles.Providers;
using System;
using System.IO;
using Veldrid;
using Veldrid.SPIRV;

namespace Castles.Shaders
{
    public class TextureShader : Shader
    {
        private GameResourcesProvider gameResourcesProvider;
        private GraphicsDeviceProvider graphicsDeviceProvider;
        public DeviceBuffer ProjectionBuffer { get; private set; }
        public DeviceBuffer ViewBuffer { get; private set; }
        public DeviceBuffer WorldBuffer { get; private set; }
        public CommandList CommandList { get; private set; }
        public Pipeline Pipeline { get; private set; }
        public ResourceSet ProjectionViewSet { get; private set; }
        public ResourceSet WorldTextureSet { get; private set; }
        public TextureView SurfaceTextureView { get; private set; }

        public TextureShader(IApplicationWindow window,
            GraphicsDeviceProvider graphicsDeviceProvider,
            GameResourcesProvider gameResourcesProvider) : base(window, graphicsDeviceProvider)
        {
            this.graphicsDeviceProvider = graphicsDeviceProvider;
            this.gameResourcesProvider = gameResourcesProvider;

            var factory = graphicsDeviceProvider.ResourceFactory;

            ProjectionBuffer = factory.CreateBuffer(new BufferDescription(64, BufferUsage.UniformBuffer));
            ViewBuffer = factory.CreateBuffer(new BufferDescription(64, BufferUsage.UniformBuffer));
            WorldBuffer = factory.CreateBuffer(new BufferDescription(64, BufferUsage.UniformBuffer));

            SurfaceTextureView = gameResourcesProvider.TextureView;

            var shadersPath = Path.Combine(Environment.CurrentDirectory, @"Content/shader");
            var vertexShaderBytes = File.ReadAllBytes(Path.Combine(shadersPath, "WorldTexture.vert.spv"));
            var fragmentShaderBytes = File.ReadAllBytes(Path.Combine(shadersPath, "WorldTexture.frag.spv"));

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

            Pipeline = factory.CreateGraphicsPipeline(new GraphicsPipelineDescription(
                BlendStateDescription.SingleOverrideBlend,
                DepthStencilStateDescription.DepthOnlyLessEqual,
                RasterizerStateDescription.Default,
                PrimitiveTopology.TriangleList,
                shaderSet,
                new[] { projViewLayout, worldTextureLayout },
                MainSwapchain.Framebuffer.OutputDescription));

            ProjectionViewSet = factory.CreateResourceSet(new ResourceSetDescription(
                projViewLayout,
                ProjectionBuffer,
                ViewBuffer));

            WorldTextureSet = factory.CreateResourceSet(new ResourceSetDescription(
                worldTextureLayout,
                WorldBuffer,
                SurfaceTextureView,
                GraphicsDevice.Aniso4xSampler));

            CommandList = factory.CreateCommandList();
        }
    }
}
