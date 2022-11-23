using Castles.AssetPrimitives;
using System;
using System.Collections.Generic;
using System.IO;
using Veldrid;

namespace Castles.Providers
{
    public class GameResourcesProvider
    {
        public Texture Texture { get; private set; }
        public TextureView TextureView { get; private set; }

        private readonly GraphicsDevice graphicsDevice;
        private readonly ResourceFactory resourceFactory;

        public GameResourcesProvider(
            GraphicsDeviceProvider graphicsDeviceProvider,
            ModManifestProvider modManifestProvider,
            IFileSystem fileSystem)
        {
            graphicsDevice = graphicsDeviceProvider.GraphicsDevice;
            resourceFactory = graphicsDeviceProvider.GraphicsDevice.ResourceFactory;

            var imageSharpProcessor = new ImageSharpProcessor();
            var modManifest = modManifestProvider.ModManifestFile;
            var processedTextures = new List<ProcessedTexture>();
            foreach (var texture in modManifest.Textures)
            {
                var textureFilePath = Path.Combine(fileSystem.TextureDirectory, texture.Filename);
                var processedTexture = LoadFileAsset<ProcessedTexture>(textureFilePath, imageSharpProcessor);
                processedTextures.Add(processedTexture);
            }

            Texture = CreateDeviceTextureArray(
                processedTextures.ToArray(),
                graphicsDevice,
                resourceFactory,
                TextureUsage.Sampled,
                processedTextures.Count);

            TextureView = resourceFactory.CreateTextureView(Texture);
        }

        public static T LoadFileAsset<T>(string name, ImageSharpProcessor isp)
        {
            object processedAsset;
            using (var fs = File.OpenRead(name))
            {
                processedAsset = isp.Process(fs, ".png");
            }

            return (T)processedAsset;
        }

        public unsafe Texture CreateDeviceTextureArray(ProcessedTexture[] textures, GraphicsDevice gd, ResourceFactory rf, TextureUsage usage, int arrayLayers)
        {
            if (textures == null || textures.Length == 0)
                throw new ArgumentException("Processed textures array not supplied");

            var first = textures[0];

            Texture texture = rf.CreateTexture(new TextureDescription(
                first.Width, first.Height, first.Depth, first.MipLevels, (uint)arrayLayers, first.Format, usage, first.Type));

            Texture staging = rf.CreateTexture(new TextureDescription(
                first.Width, first.Height, first.Depth, first.MipLevels, (uint)arrayLayers, first.Format, TextureUsage.Staging, first.Type));

            uint layer = 0;
            foreach (var processedTex in textures)
            {
                ulong offset = 0;
                fixed (byte* texDataPtr = &processedTex.TextureData[0])
                {
                    for (uint level = 0; level < processedTex.MipLevels; level++)
                    {
                        uint mipWidth = ProcessedTexture.GetDimension(processedTex.Width, level);
                        uint mipHeight = ProcessedTexture.GetDimension(processedTex.Height, level);
                        uint mipDepth = ProcessedTexture.GetDimension(processedTex.Depth, level);
                        uint subresourceSize = mipWidth * mipHeight * mipDepth * processedTex.GetFormatSize(processedTex.Format);

                        //for (uint layer = 0; layer < processedTex.ArrayLayers; layer++)
                        {
                            gd.UpdateTexture(
                                staging, (IntPtr)(texDataPtr + offset), subresourceSize,
                                0, 0, 0, mipWidth, mipHeight, mipDepth,
                                level, layer);
                            offset += subresourceSize;
                        }
                    }
                }
                layer++;
            }

            CommandList cl = rf.CreateCommandList();
            cl.Begin();
            cl.CopyTexture(staging, texture);
            cl.End();
            gd.SubmitCommands(cl);

            return texture;
        }
    }
}
