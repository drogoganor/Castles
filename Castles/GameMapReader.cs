using Castles.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text.Json;
using Veldrid;

namespace Castles
{
    public class GameMapReader
    {
        private GameMap gameMap;

        public GameMapReader(string filename)
        {
            var contentDir = Path.Combine(AppContext.BaseDirectory, "Content");
            var mapJsonFilePath = Path.Combine(contentDir, filename);
            var mapJsonText = File.ReadAllText(mapJsonFilePath);
            gameMap = JsonSerializer.Deserialize<GameMap>(mapJsonText);
        }

        public VertexPositionTexture2D[] GetVertexArray()
        {
            var vertexList = new List<VertexPositionTexture2D>();
            foreach (var block in gameMap.Tiles)
            {
                vertexList.AddRange(
                    new VertexPositionTexture2D[]
                    {
                        // Back
                        new VertexPositionTexture2D(block.Position + new Vector2(32f, +32f), new Vector3(0, 0, block.Texture)),
                        new VertexPositionTexture2D(block.Position + new Vector2(-32f, +32f), new Vector3(1, 0, block.Texture)),
                        new VertexPositionTexture2D(block.Position + new Vector2(-32f, -32f), new Vector3(1, 1, block.Texture)),
                        new VertexPositionTexture2D(block.Position + new Vector2(+32f, +32f), new Vector3(0, 0, block.Texture)),
                        new VertexPositionTexture2D(block.Position + new Vector2(-32f, -32f), new Vector3(1, 1, block.Texture)),
                        new VertexPositionTexture2D(block.Position + new Vector2(+32f, -32f), new Vector3(0, 1, block.Texture)),
                    }
                );
            }

            return vertexList.ToArray();
        }
    }
}
