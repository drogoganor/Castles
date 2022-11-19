using Castles.Data.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Castles.Data
{
    public class ModManifestFile
    {
        public string Name { get; set; }
        public int TileSize { get; set; }
        public string[] Campaigns { get; set; }
        public TileTextureFile[] Textures { get; set; }
        public ModMenuFonts Fonts { get; set; }
    }

    public class ModMenuFonts
    {
        public string FontName { get; set; }
        public int FontSize { get; set; }
    }
}
