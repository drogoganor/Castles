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
            ColorA = color.X;
            ColorR = color.Y;
            ColorG = color.Z;
            ColorB = color.W;
        }
    }
}
