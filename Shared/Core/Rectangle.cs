namespace CloudinaryDotNet.Core
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Stores a set of four integers that represent the location and size of a rectangle.
    /// </summary>
    public struct Rectangle : IEquatable<Rectangle>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Rectangle"/> struct with the specified location and size.
        /// </summary>
        /// <param name="x">the x-coordinate of the upper-left corner of this <see cref="Rectangle"/> structure.</param>
        /// <param name="y">the y-coordinate of the upper-left corner of this <see cref="Rectangle"/> structure.</param>
        /// <param name="width">the width of this <see cref="Rectangle"/> structure.</param>
        /// <param name="height">the height of this <see cref="Rectangle"/> structure.</param>
        public Rectangle(int x, int y, int width, int height)
            : this()
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
        /// Equality operator for two rectangles.
        /// </summary>
        /// <param name="left">First rectangle to compare.</param>
        /// <param name="right">Second rectangle to compare.</param>
        /// <returns>The computed hashcode.</returns>
        public static bool operator ==(Rectangle left, Rectangle right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Inequality operator for two rectangles.
        /// </summary>
        /// <param name="left">First rectangle to compare.</param>
        /// <param name="right">Second rectangle to compare.</param>
        public static bool operator !=(Rectangle left, Rectangle right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Converts the attributes of this <see cref="Rectangle"/> to a human-readable string.
        /// </summary>
        /// <returns>
        /// A string that contains the position, width, and height of this <see cref="Rectangle"/> structure.
        /// For example, {{X=20, Y=20, Width=100, Height=50}}.
        /// </returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{{X={0}, Y={1}, Width={2}, Height={3}}}", X, Y, Width, Height);
        }

        /// <summary>
        /// Check the equality of two rectangles.
        /// </summary>
        /// <param name="other">The rectangle to compare.</param>
        /// <returns>True - if rectangles are equal. Otherwise false.</returns>
        public bool Equals(Rectangle other)
        {
            return Height == other.Height && Width == other.Width && X == other.X && Y == other.Y;
        }

        /// <summary>
        /// Check the equality of two rectangles.
        /// </summary>
        /// <param name="obj">The rectangle to compare.</param>
        /// <returns>True - if rectangles are equal. Otherwise false.</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is Rectangle other && Equals(other);
        }

        /// <summary>
        /// Compute a hashcode for the rectangle.
        /// </summary>
        /// <returns>The computed hashcode.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Height;
                hashCode = (hashCode * 397) ^ Width;
                hashCode = (hashCode * 397) ^ X;
                hashCode = (hashCode * 397) ^ Y;
                return hashCode;
            }
        }
    }
}
