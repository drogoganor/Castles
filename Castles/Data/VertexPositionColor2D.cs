using System.Numerics;

namespace Castles.Data
{
    public struct VertexPositionColor2D
    {
        public const uint SizeInBytes = 24;

        public float PosX;
        public float PosY;

        public float ColorA;
        public float ColorR;
        public float ColorG;
        public float ColorB;

        public VertexPositionColor2D(Vector2 pos, Vector4 color)
        {
            PosX = pos.X;
            PosY = pos.Y;
            ColorR = color.X;
            ColorG = color.Y;
            ColorB = color.Z;
            ColorA = color.W;
        }
    }
}
