using Castles.Data;
using Castles.Interfaces;
using Castles.Providers;
using Castles.Shaders;
using System.Collections.Generic;
using System.Numerics;
using Veldrid;

namespace Castles.Render
{
    public class DebugGrid : GameScreen
    {
        private readonly ModManifestProvider modManifestProvider;
        private readonly ColorShader colorShader;
        private readonly Camera2D camera;
        private readonly VertexPositionColor2D[] vertices;
        private DeviceBuffer vertexBuffer;

        public DebugGrid(IApplicationWindow window,
            ModManifestProvider modManifestProvider,
            ColorShader colorShader,
            Camera2D camera) : base(window)
        {
            this.modManifestProvider = modManifestProvider;
            this.colorShader = colorShader;
            this.camera = camera;
            vertices = GetVertexArray();
        }

        public VertexPositionColor2D[] GetVertexArray()
        {
            var tileSize = modManifestProvider.ModManifestFile.TileSize;
            var vertexList = new List<VertexPositionColor2D>();

            var sizeX = 20;
            var sizeY = 20;
            var gridColor = new Vector4(0.5f, 0f, 1f, 0f);

            for (var x = 0; x <= sizeX; x++)
            {
                vertexList.Add(new VertexPositionColor2D(new Vector2(x * tileSize, -1), gridColor));
                vertexList.Add(new VertexPositionColor2D(new Vector2(x * tileSize, sizeY * tileSize), gridColor));
            }

            for (var y = 0; y <= sizeY; y++)
            {
                vertexList.Add(new VertexPositionColor2D(new Vector2(0, y * tileSize), gridColor));
                vertexList.Add(new VertexPositionColor2D(new Vector2(sizeY * tileSize, y * tileSize), gridColor));
            }

            return vertexList.ToArray();
        }

        protected override void CreateResources(ResourceFactory factory)
        {
            if (vertices != null)
            {
                vertexBuffer = factory.CreateBuffer(new BufferDescription((uint)(VertexPositionColor2D.SizeInBytes * vertices.Length), BufferUsage.VertexBuffer));
                GraphicsDevice.UpdateBuffer(vertexBuffer, 0, vertices);
            }
        }

        protected override void Draw(float deltaSeconds)
        {
            var cl = colorShader.CommandList;
            
            //camera.Update(deltaSeconds);

            if (vertices == null)
            {
                return;
            }

            cl.Begin();

            //var projection = Matrix4x4.CreateOrthographic(
            //    Window.Width,
            //    Window.Height,
            //    0.5f,
            //    1000f);

            var projection = Matrix4x4.CreateOrthographicOffCenter(
                0,
                Window.Width,
                Window.Height,
                0,
                1f,
                100f);

            cl.UpdateBuffer(colorShader.ProjectionBuffer, 0, projection);

            var direction = new Vector3(0, 0, -1f);

            var view = Matrix4x4.CreateLookAt(camera.Position, camera.Position + direction, Vector3.UnitY);

            cl.UpdateBuffer(colorShader.ViewBuffer, 0, view);

            cl.UpdateBuffer(colorShader.WorldBuffer, 0, Matrix4x4.CreateTranslation(new Vector3(1, 1, -2)));

            cl.SetFramebuffer(MainSwapchain.Framebuffer);
            cl.ClearDepthStencil(1f);
            cl.SetPipeline(colorShader.Pipeline);
            cl.SetVertexBuffer(0, vertexBuffer);
            cl.SetGraphicsResourceSet(0, colorShader.ProjectionViewSet);
            cl.SetGraphicsResourceSet(1, colorShader.WorldTextureSet);
            cl.Draw((uint)vertices.Length);

            cl.End();
            GraphicsDevice.SubmitCommands(cl);
        }

        protected override void HandleWindowResize()
        {
        }
    }
}
