using Castles.Data;
using Castles.Interfaces;
using Castles.Providers;
using Castles.Shaders;
using System.Numerics;
using Veldrid;

namespace Castles.Render
{
    public class Scene : GameScreen
    {
        private TextureShader textureShader;
        private GameMapProvider gameMapProvider;
        private Camera2D camera;
        private readonly VertexPositionTexture2D[] vertices;
        private DeviceBuffer vertexBuffer;

        public Scene(
            IApplicationWindow window,
            Camera2D camera,
            TextureShader textureShader,
            GameMapProvider gameMapProvider) : base(window)
        {
            this.textureShader = textureShader;
            this.camera = camera;
            this.gameMapProvider = gameMapProvider;

            vertices = gameMapProvider.GetVertexArray();
        }

        protected override void CreateResources(ResourceFactory factory)
        {
            if (vertices != null)
            {
                vertexBuffer = factory.CreateBuffer(new BufferDescription((uint)(VertexPositionTexture2D.SizeInBytes * vertices.Length), BufferUsage.VertexBuffer));
                GraphicsDevice.UpdateBuffer(vertexBuffer, 0, vertices);
            }
        }

        protected override void Draw(float deltaSeconds)
        {
            var cl = textureShader.CommandList;
            camera.Update(deltaSeconds);

            if (vertices == null)
            {
                return;
            }

            cl.Begin();

            var projection = Matrix4x4.CreateOrthographic(
                Window.Width,
                Window.Height,
                1f,
                1000f);

            cl.UpdateBuffer(textureShader.ProjectionBuffer, 0, projection);

            var direction = new Vector3(0, 0, 1f);

            var view = Matrix4x4.CreateLookAt(camera.Position, camera.Position + direction, Vector3.UnitY);

            cl.UpdateBuffer(textureShader.ViewBuffer, 0, view);

            cl.UpdateBuffer(textureShader.WorldBuffer, 0, Matrix4x4.Identity);

            cl.SetFramebuffer(MainSwapchain.Framebuffer);
            cl.ClearDepthStencil(1f);
            cl.SetPipeline(textureShader.Pipeline);
            cl.SetVertexBuffer(0, vertexBuffer);
            cl.SetGraphicsResourceSet(0, textureShader.ProjectionViewSet);
            cl.SetGraphicsResourceSet(1, textureShader.WorldTextureSet);
            cl.Draw((uint)vertices.Length);

            cl.End();
            GraphicsDevice.SubmitCommands(cl);
        }

        protected override void HandleWindowResize()
        {
            camera.WindowResized(Window.Width, Window.Height);
        }
    }
}
