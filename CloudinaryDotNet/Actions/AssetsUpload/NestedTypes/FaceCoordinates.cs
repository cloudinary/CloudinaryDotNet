namespace CloudinaryDotNet.Actions
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using CloudinaryDotNet.Core;

    /// <summary>
    /// The coordinates of faces contained in an uploaded image.
    /// </summary>
    [Obsolete("One could use List<Rectangle>")]
    public class FaceCoordinates : List<Rectangle>
    {
        /// <summary>
        /// Represents the face coordinates as string "x,y,w,h|x,y,w,h" separated with a pipe (❘).
        /// For example: "10,20,150,130❘213,345,82,61".
        /// </summary>
        /// <returns>The string representation of face coordinates.</returns>
        public override string ToString()
        {
            return string.Join(
                "|",
                this.Select(r =>
                    string.Format(CultureInfo.InvariantCulture, "{0},{1},{2},{3}", r.X, r.Y, r.Width, r.Height)).ToArray());
        }
    }
}