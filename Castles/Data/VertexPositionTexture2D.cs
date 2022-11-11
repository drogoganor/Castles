using System.Numerics;

namespace Castles.Data
{
    public struct VertexPositionTexture2D
    {
        public const uint SizeInBytes = 20;

        public float PosX;
        public float PosY;

        public float TexU;
        public float TexV;
        public float TexW;

        public VertexPositionTexture2D(Vector2 pos, Vector3 uvw)
        {
            PosX = pos.X;
            PosY = pos.Y;
            TexU = uvw.X;
            TexV = uvw.Y;
            TexW = uvw.Z;
        }
    }
}
