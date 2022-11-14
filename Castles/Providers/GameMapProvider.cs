using Castles.Data;
using Castles.Data.Files;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text.Json;

namespace Castles.Providers
{
    public class GameMapProvider
    {
        private readonly GameMapFile gameMap;
        private readonly ModManifestProvider modManifestProvider;

        public GameMapFile GameMap => gameMap;

        public GameMapProvider(ModManifestProvider modManifestProvider)
        {
            this.modManifestProvider = modManifestProvider;
            var contentDir = Path.Combine(AppContext.BaseDirectory, "Content");
            var mapJsonFilePath = Path.Combine(contentDir, "mapdemo2.json");
            var mapJsonText = File.ReadAllText(mapJsonFilePath);
            gameMap = JsonSerializer.Deserialize<GameMapFile>(mapJsonText);
        }

        public VertexPositionTexture2D[] GetVertexArray()
        {
            var tileSize = modManifestProvider.ModManifestFile.TileSize;
            var vertexList = new List<VertexPositionTexture2D>();
            foreach (var block in gameMap.Tiles)
            {
                vertexList.AddRange(
                    new VertexPositionTexture2D[]
                    {
                        // Back
                        new VertexPositionTexture2D(block.Position + new Vector2(tileSize, tileSize), new Vector3(0, 0, block.Texture)),
                        new VertexPositionTexture2D(block.Position + new Vector2(0, tileSize), new Vector3(1, 0, block.Texture)),
                        new VertexPositionTexture2D(block.Position + new Vector2(0, 0), new Vector3(1, 1, block.Texture)),
                        new VertexPositionTexture2D(block.Position + new Vector2(tileSize, tileSize), new Vector3(0, 0, block.Texture)),
                        new VertexPositionTexture2D(block.Position + new Vector2(0, 0), new Vector3(1, 1, block.Texture)),
                        new VertexPositionTexture2D(block.Position + new Vector2(tileSize, 0), new Vector3(0, 1, block.Texture)),
                    }
                );
            }

            return vertexList.ToArray();
        }
    }
}
