using System;

namespace CloudinaryDotNet.Core
{
    /// <summary>
    /// Stores a set of four integers that represent the location and size of a rectangle.
    /// </summary>
    public struct Rectangle
    {
        /// <summary>
        /// Instantiates the <see cref="Rectangle"/> object with the specified location and size.
        /// </summary>
        /// <param name="x">the x-coordinate of the upper-left corner of this <see cref="Rectangle"/> structure</param>
        /// <param name="y">the y-coordinate of the upper-left corner of this <see cref="Rectangle"/> structure</param>
        /// <param name="width">the width of this <see cref="Rectangle"/> structure</param>
        /// <param name="height">the height of this <see cref="Rectangle"/> structure</param>
        public Rectangle(int x, int y, int width, int height) : this()
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Gets or sets the height of this <see cref="Rectangle"/> structure.
        /// </summary>
        public int Height { get; set; }
        /// <summary>
        /// Gets or sets the width of this <see cref="Rectangle"/> structure.
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// Gets or sets the x-coordinate of the upper-left corner of this <see cref="Rectangle"/> structure.
        /// </summary>
        public int X { get; set; }
        /// <summary>
        /// Gets or sets the y-coordinate of the upper-left corner of this <see cref="Rectangle"/> structure.
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Converts the attributes of this <see cref="Rectangle"/> to a human-readable string.
        /// </summary>
        /// <returns>
        /// A string that contains the position, width, and height of this <see cref="Rectangle"/> structure.
        /// For example, {{X=20, Y=20, Width=100, Height=50}}.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{{X={0}, Y={1}, Width={2}, Height={3}}}", X, Y, Width, Height);
        }
    }
}
