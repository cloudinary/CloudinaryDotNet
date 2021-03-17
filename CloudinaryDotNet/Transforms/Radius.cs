namespace CloudinaryDotNet
{
    using System;
    using System.Collections;
    using System.Linq;

    /// <summary>
    /// Round the specified corners of an image by specifying 1-4 pixel values.
    /// </summary>
    public class Radius : Core.ICloneable
    {
        private string m_radius;

        /// <summary>
        /// Initializes a new instance of the <see cref="Radius"/> class.
        /// Defines radius value for corners rounding (in pixels).
        /// Or specify 'max' to make the image a perfect circle or oval (ellipse).
        /// </summary>
        /// <param name="value">Can be string, number, float or collection with 1..4 values.</param>
        /// <exception cref="ArgumentNullException">The value cannot be null.</exception>/>
        public Radius(object value) => SetRadius(Normalize(value));

        /// <summary>
        /// Initializes a new instance of the <see cref="Radius"/> class.
        /// Defines radius value for corners rounding (in pixels).
        /// Or specify 'max' to make the image a perfect circle or oval (ellipse).
        /// </summary>
        /// <param name="value">
        /// Symmetrical. All four corners are rounded equally according to the specified value.
        /// </param>
        /// <exception cref="ArgumentNullException">The value cannot be null.</exception>/>
        public Radius(string value) => SetRadius(value);

        /// <summary>
        /// Initializes a new instance of the <see cref="Radius"/> class.
        /// Defines radius value for corners rounding (in pixels).
        /// </summary>
        /// <param name="value">
        /// Symmetrical. All four corners are rounded equally according to the specified value.
        /// </param>
        /// <exception cref="ArgumentNullException">The value cannot be null.</exception>/>
        public Radius(int value) => SetRadius(value);

        /// <summary>
        /// Initializes a new instance of the <see cref="Radius"/> class.
        /// Defines radius value for corners rounding (in pixels).
        /// </summary>
        /// <param name="value">
        /// Symmetrical. All four corners are rounded equally according to the specified value.
        /// </param>
        /// <exception cref="ArgumentNullException">The value cannot be null.</exception>/>
        public Radius(float value) => SetRadius(value);

        /// <summary>
        /// Initializes a new instance of the <see cref="Radius"/> class.
        /// Defines radius value for corners rounding (in pixels).
        /// </summary>
        /// <param name="topLeftAndBottomRight">Top-left and bottom-right corners.</param>
        /// <param name="topRightAndBottomLeft">Top-right and bottom-left corners.</param>
        /// <exception cref="ArgumentNullException">Both arguments cannot be null.</exception>/>
        public Radius(object topLeftAndBottomRight, object topRightAndBottomLeft)
        {
            if (topLeftAndBottomRight == null)
            {
                throw new ArgumentNullException(nameof(topLeftAndBottomRight));
            }

            if (topRightAndBottomLeft == null)
            {
                throw new ArgumentNullException(nameof(topRightAndBottomLeft));
            }

            m_radius = $"{topLeftAndBottomRight}:{topRightAndBottomLeft}";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Radius"/> class.
        /// Defines radius value for corners rounding (in pixels).
        /// </summary>
        /// <param name="topLeft">Top-left corner.</param>
        /// <param name="topRightAndBottomLeft">Top-right and bottom-left corners.</param>
        /// <param name="bottomRight">Bottom-right corner.</param>
        /// <exception cref="ArgumentNullException">None of the arguments can be null.</exception>/>
        public Radius(object topLeft, object topRightAndBottomLeft, object bottomRight)
        {
            if (topLeft == null)
            {
                throw new ArgumentNullException(nameof(topLeft));
            }

            if (topRightAndBottomLeft == null)
            {
                throw new ArgumentNullException(nameof(topRightAndBottomLeft));
            }

            if (bottomRight == null)
            {
                throw new ArgumentNullException(nameof(bottomRight));
            }

            m_radius = $"{topLeft}:{topRightAndBottomLeft}:{bottomRight}";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Radius"/> class.
        /// Defines radius value for corners rounding (in pixels).
        /// The rounding for each corner is specified separately, in clockwise order, starting with the top-left.
        /// </summary>
        /// <param name="topLeft">Top-left corner.</param>
        /// <param name="topRight">Top-right corner.</param>
        /// <param name="bottomRight">Bottom-right corner.</param>
        /// <param name="bottomLeft">Bottom-left corner.</param>
        /// <exception cref="ArgumentNullException">None of the arguments can be null.</exception>/>
        public Radius(object topLeft, object topRight, object bottomRight, object bottomLeft)
        {
            if (topLeft == null)
            {
                throw new ArgumentNullException(nameof(topLeft));
            }

            if (topRight == null)
            {
                throw new ArgumentNullException(nameof(topRight));
            }

            if (bottomRight == null)
            {
                throw new ArgumentNullException(nameof(bottomRight));
            }

            if (bottomLeft == null)
            {
                throw new ArgumentNullException(nameof(bottomLeft));
            }

            m_radius = $"{topLeft}:{topRight}:{bottomRight}:{bottomLeft}";
        }

        /// <summary>
        /// Creates a new Radius object that is a copy of the current instance.
        /// </summary>
        /// <returns>A new Radius object that is a copy of this instance.</returns>
        public Radius Clone() => (Radius)MemberwiseClone();

        /// <summary>
        /// Creates a new object that is a deep copy of the current instance.
        /// </summary>
        /// <returns> A new object that is a deep copy of this instance.</returns>
        object Core.ICloneable.Clone() => Clone();

        /// <inheritdoc />
        public override string ToString()
        {
            return m_radius;
        }

        /// <summary>
        /// Parse provided radius value and make it normalized.
        /// </summary>
        /// <param name="value">Can be string, number, float or collection with 1..4 values.</param>
        /// <exception cref="ArgumentException">The value cannot be null.</exception>/>
        private static string Normalize(object value)
        {
            if (value is ICollection radiusCollection)
            {
                if (radiusCollection.Count == 0 || radiusCollection.Count > 4)
                {
                    throw new ArgumentException("Radius array should contain between 1 and 4 values");
                }

                var strings = from object item in radiusCollection select item.ToString();
                return string.Join(":", strings);
            }

            return value.ToString();
        }

        private void SetRadius(object value)
        {
            m_radius = value != null
                ? value.ToString()
                : throw new ArgumentNullException(nameof(value));
        }
    }
}
