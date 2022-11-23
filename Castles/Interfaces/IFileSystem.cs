namespace Castles
{
    public interface IFileSystem
    {
        string ContentDirectory { get; }
        string MapDirectory { get; }
        string TextureDirectory { get; }
        string ShaderDirectory { get; }
    }
}
