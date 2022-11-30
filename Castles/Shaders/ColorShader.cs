using Castles.Interfaces;
using Castles.Providers;
using System;
using System.IO;
using Veldrid;
using Veldrid.SPIRV;

namespace Castles.Shaders
{
    public class ColorShader : Shader
    {
        public DeviceBuffer ProjectionBuffer { get; private set; }
        public DeviceBuffer ViewBuffer { get; private set; }
        public DeviceBuffer WorldBuffer { get; private set; }
        public CommandList CommandList { get; private set; }
        public Pipeline Pipeline { get; private set; }
        public ResourceSet ProjectionViewSet { get; private set; }
        public ResourceSet WorldTextureSet { get; private set; }

        public ColorShader(IApplicationWindow window,
            GameResourcesProvider gameResourcesProvider) : base(window)
        {
        }

        protected override void CreateResources(ResourceFactory factory)
        {
            ProjectionBuffer = factory.CreateBuffer(new BufferDescription(64, BufferUsage.UniformBuffer));
            ViewBuffer = factory.CreateBuffer(new BufferDescription(64, BufferUsage.UniformBuffer));
            WorldBuffer = factory.CreateBuffer(new BufferDescription(64, BufferUsage.UniformBuffer));

            var shadersPath = Path.Combine(Environment.CurrentDirectory, @"Content/shader");
            var vertexShaderBytes = File.ReadAllBytes(Path.Combine(shadersPath, "WorldColor.vert.spv"));
            var fragmentShaderBytes = File.ReadAllBytes(Path.Combine(shadersPath, "WorldColor.frag.spv"));

            ShaderSetDescription shaderSet = new ShaderSetDescription(
                new[]
                {
                    new VertexLayoutDescription(
                        new VertexElementDescription("Position", VertexElementSemantic.TextureCoordinate, VertexElementFormat.Float2),
                        new VertexElementDescription("Color", VertexElementSemantic.TextureCoordinate, VertexElementFormat.Float4))
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
                    new ResourceLayoutElementDescription("WorldBuffer", ResourceKind.UniformBuffer, ShaderStages.Vertex)));

            Pipeline = factory.CreateGraphicsPipeline(new GraphicsPipelineDescription(
                BlendStateDescription.SingleAlphaBlend,
                DepthStencilStateDescription.DepthOnlyLessEqual,
                RasterizerStateDescription.Default,
                PrimitiveTopology.LineList,
                shaderSet,
                new[] { projViewLayout, worldTextureLayout },
                MainSwapchain.Framebuffer.OutputDescription));

            ProjectionViewSet = factory.CreateResourceSet(new ResourceSetDescription(
                projViewLayout,
                ProjectionBuffer,
                ViewBuffer));

            WorldTextureSet = factory.CreateResourceSet(new ResourceSetDescription(
                worldTextureLayout,
                WorldBuffer));

            CommandList = factory.CreateCommandList();
        }
    }
}
