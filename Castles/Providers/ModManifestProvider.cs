using Castles.Data;
using System;
using System.IO;
using System.Text.Json;

namespace Castles.Providers
{
    public class ModManifestProvider
    {
        private readonly ModManifestFile modManifestFile;

        public ModManifestFile ModManifestFile => modManifestFile;

        public ModManifestProvider()
        {
            var contentDir = Path.Combine(AppContext.BaseDirectory, "Content");
            var modFilePath = Path.Combine(contentDir, "mod.json");
            var modManifestJson = File.ReadAllText(modFilePath);
            modManifestFile = JsonSerializer.Deserialize<ModManifestFile>(modManifestJson);
        }
    }
}
