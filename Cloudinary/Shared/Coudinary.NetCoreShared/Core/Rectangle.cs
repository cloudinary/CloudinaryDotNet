using System;

namespace CloudinaryDotNet.Core
{
    public struct Rectangle
    {
        public Rectangle(int x, int y, int width, int height) : this()
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public int Height { get; set; }
        public int Width { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public override string ToString()
        {
            return string.Format("{{X={0}, Y={1}, Width={2}, Height={3}}}", X, Y, Width, Height);
        }
    }
}
