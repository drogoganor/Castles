using System.Numerics;
using System.Text.Json.Serialization;
using Castles.Data.Json;

namespace Castles.Data
{
    public class MapTile
    {
        [JsonConverter(typeof(JsonConverterVector2))]
        public Vector2 Position { get; set; }
        public int Texture { get; set; }
    }
}
