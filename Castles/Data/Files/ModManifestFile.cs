using Castles.Data.Files;
using Castles.Enums;
using System.Text.Json.Serialization;

namespace Castles.Data
{
    public class ModManifestFile
    {
        public string Name { get; set; }
        public int TileSize { get; set; }
        public string[] Campaigns { get; set; }
        public TileTextureFile[] Textures { get; set; }
        public ModMenuFont[] Fonts { get; set; }
    }

    public class ModMenuFont
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public FontSize SizeEnum { get; set; }
        public string FontName { get; set; }
        public int FontSize { get; set; }
    }
}
