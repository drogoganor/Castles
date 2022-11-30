using Castles.Data;
using Castles.Data.Files;
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

        public GameMapProvider(
            ModManifestProvider modManifestProvider,
            IFileSystem fileSystem)
        {
            this.modManifestProvider = modManifestProvider;
            var mapJsonFilePath = Path.Combine(fileSystem.MapDirectory, "testmap.json");
            var mapJsonText = File.ReadAllText(mapJsonFilePath);
            gameMap = JsonSerializer.Deserialize<GameMapFile>(mapJsonText);
        }

        public VertexPositionTexture2D[] GetVertexArray()
        {
            var tileSize = modManifestProvider.ModManifestFile.TileSize;
            var vertexList = new List<VertexPositionTexture2D>();
            foreach (var block in gameMap.Tiles)
            {
                var scaledPosition = block.Position * modManifestProvider.ModManifestFile.TileSize;
                vertexList.AddRange(
                    new VertexPositionTexture2D[]
                    {
                        // Back
                        new VertexPositionTexture2D(scaledPosition + new Vector2(tileSize, tileSize), new Vector3(1, 1, block.Texture)),
                        new VertexPositionTexture2D(scaledPosition + new Vector2(0, tileSize), new Vector3(0, 1, block.Texture)),
                        new VertexPositionTexture2D(scaledPosition + new Vector2(0, 0), new Vector3(0, 0, block.Texture)),
                        new VertexPositionTexture2D(scaledPosition + new Vector2(tileSize, tileSize), new Vector3(1, 1, block.Texture)),
                        new VertexPositionTexture2D(scaledPosition + new Vector2(0, 0), new Vector3(0, 0, block.Texture)),
                        new VertexPositionTexture2D(scaledPosition + new Vector2(tileSize, 0), new Vector3(1, 0, block.Texture)),
                    }
                );
            }

            return vertexList.ToArray();
        }
    }
}
