using System;
using System.IO;

namespace Castles.Data
{
    public class FileSystem : IFileSystem
    {
        private readonly string applicationDirectory;

        private readonly string contentDirectory;
        public string ContentDirectory => contentDirectory;

        private readonly string mapDirectory;
        public string MapDirectory => mapDirectory;

        private readonly string textureDirectory;
        public string TextureDirectory => textureDirectory;

        private readonly string shaderDirectory;
        public string ShaderDirectory => shaderDirectory;

        public FileSystem()
        {
            applicationDirectory = Path.Combine(AppContext.BaseDirectory, "Content");
            contentDirectory = applicationDirectory;
            mapDirectory = Path.Combine(ContentDirectory, "maps");
            textureDirectory = Path.Combine(ContentDirectory, "textures");
            shaderDirectory = Path.Combine(ContentDirectory, "shaders");
        }
    }
}
