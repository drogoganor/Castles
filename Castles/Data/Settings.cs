using Castles.Data.Json;
using System.Numerics;
using System.Text.Json.Serialization;

namespace Castles.Data
{
    public class Settings
    {
        [JsonConverter(typeof(JsonConverterVector2))]
        public Vector2 ScreenResolution { get; set; }
    }
}
