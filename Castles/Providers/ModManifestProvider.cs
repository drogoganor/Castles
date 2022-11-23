using Castles.Data;
using System.IO;
using System.Text.Json;

namespace Castles.Providers
{
    public class ModManifestProvider
    {
        private readonly ModManifestFile modManifestFile;

        public ModManifestFile ModManifestFile => modManifestFile;

        public ModManifestProvider(IFileSystem fileSystem)
        {
            var modFilePath = Path.Combine(fileSystem.ContentDirectory, "mod.json");
            var modManifestJson = File.ReadAllText(modFilePath);
            modManifestFile = JsonSerializer.Deserialize<ModManifestFile>(modManifestJson);
        }
    }
}
