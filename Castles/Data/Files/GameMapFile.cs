using System;
using System.Numerics;
using System.Text.Json.Serialization;
using Castles.Data.Json;

namespace Castles.Data.Files
{
    public class GameMapFile
    {
        public string Name { get; set; } = string.Empty;

        [JsonConverter(typeof(JsonConverterVector2))]
        public Vector2 Size { get; set; }
        public MapTile[] Tiles { get; set; } = Array.Empty<MapTile>();
    }
}
