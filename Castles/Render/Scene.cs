using Castles.Data;
using Castles.Interfaces;
using Castles.Providers;
using Castles.Shaders;
using System.Numerics;
using Veldrid;

namespace Castles.Render
{
    public class Scene : IRenderable
    {
        private readonly IApplicationWindow window;
        private readonly GraphicsDeviceProvider graphicsDeviceProvider;
        private readonly TextureShader textureShader;
        private readonly GameMapProvider gameMapProvider;
        private readonly Camera2D camera;
        private readonly VertexPositionTexture2D[] vertices;
        private readonly DeviceBuffer vertexBuffer;

        public Scene(
            IApplicationWindow window,
            GraphicsDeviceProvider graphicsDeviceProvider,
            Camera2D camera,
            TextureShader textureShader,
            GameMapProvider gameMapProvider)
        {
            this.window = window;
            this.graphicsDeviceProvider = graphicsDeviceProvider;
            this.textureShader = textureShader;
            this.camera = camera;
            this.gameMapProvider = gameMapProvider;

            vertices = gameMapProvider.GetVertexArray();

            if (vertices != null)
            {
                vertexBuffer = graphicsDeviceProvider.ResourceFactory.CreateBuffer(new BufferDescription((uint)(VertexPositionTexture2D.SizeInBytes * vertices.Length), BufferUsage.VertexBuffer));
                graphicsDeviceProvider.GraphicsDevice.UpdateBuffer(vertexBuffer, 0, vertices);
            }

            window.Resized += HandleWindowResize;
        }

        public void Draw(float deltaSeconds)
        {
            var cl = textureShader.CommandList;
            camera.Update(deltaSeconds);

            if (vertices == null)
            {
                return;
            }

            cl.Begin();

            var projection = Matrix4x4.CreateOrthographicOffCenter(
                0,
                window.Width,
                window.Height,
                0,
                1f,
                100f);

            cl.UpdateBuffer(textureShader.ProjectionBuffer, 0, projection);

            var direction = new Vector3(0, 0, -1f);

            var view = Matrix4x4.CreateLookAt(camera.Position, camera.Position + direction, Vector3.UnitY);

            cl.UpdateBuffer(textureShader.ViewBuffer, 0, view);

            cl.UpdateBuffer(textureShader.WorldBuffer, 0, Matrix4x4.CreateTranslation(new Vector3(0, 0, -2)));

            cl.SetFramebuffer(graphicsDeviceProvider.GraphicsDevice.MainSwapchain.Framebuffer);
            cl.ClearDepthStencil(1f);
            cl.SetPipeline(textureShader.Pipeline);
            cl.SetVertexBuffer(0, vertexBuffer);
            cl.SetGraphicsResourceSet(0, textureShader.ProjectionViewSet);
            cl.SetGraphicsResourceSet(1, textureShader.WorldTextureSet);
            cl.Draw((uint)vertices.Length);

            cl.End();
            graphicsDeviceProvider.GraphicsDevice.SubmitCommands(cl);
        }

        private void HandleWindowResize()
        {
            camera.WindowResized(window.Width, window.Height);
        }
    }
}
