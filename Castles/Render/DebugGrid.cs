using Castles.Data;
using Castles.Interfaces;
using Castles.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Veldrid;

namespace Castles.Render
{
    public class DebugGrid : GameScreen
    {
        private readonly ModManifestProvider modManifestProvider;

        public DebugGrid(IApplicationWindow window,
            ModManifestProvider modManifestProvider) : base(window)
        {
            this.modManifestProvider = modManifestProvider;
        }

        public VertexPositionTexture2D[] GetVertexArray()
        {
            var tileSize = modManifestProvider.ModManifestFile.TileSize;
            var vertexList = new List<VertexPositionTexture2D>();
            //foreach (var block in gameMap.Tiles)
            //{
            //    var scaledPosition = block.Position * modManifestProvider.ModManifestFile.TileSize;
            //    vertexList.AddRange(
            //        new VertexPositionTexture2D[]
            //        {
            //            // Back
            //            new VertexPositionTexture2D(scaledPosition + new Vector2(tileSize, tileSize), new Vector3(0, 0, block.Texture)),
            //            new VertexPositionTexture2D(scaledPosition + new Vector2(0, tileSize), new Vector3(1, 0, block.Texture)),
            //            new VertexPositionTexture2D(scaledPosition + new Vector2(0, 0), new Vector3(1, 1, block.Texture)),
            //            new VertexPositionTexture2D(scaledPosition + new Vector2(tileSize, tileSize), new Vector3(0, 0, block.Texture)),
            //            new VertexPositionTexture2D(scaledPosition + new Vector2(0, 0), new Vector3(1, 1, block.Texture)),
            //            new VertexPositionTexture2D(scaledPosition + new Vector2(tileSize, 0), new Vector3(0, 1, block.Texture)),
            //        }
            //    );
            //}

            return vertexList.ToArray();
        }

        protected override void CreateResources(ResourceFactory factory)
        {
        }

        protected override void Draw(float deltaSeconds)
        {
        }

        protected override void HandleWindowResize()
        {
        }
    }
}
