namespace CloudinaryDotNet
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Class that represents an Eager transformation.
    /// </summary>
    public class EagerTransformation : Transformation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EagerTransformation"/> class.
        /// Creates eager transformation object chained with other transformations.
        /// </summary>
        /// <param name="transforms">A list of transformations to chain with.</param>
        public EagerTransformation(params Transformation[] transforms)
            : base(transforms.ToList())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EagerTransformation"/> class.
        /// Creates eager transformation object chained with other transformations.
        /// </summary>
        /// <param name="transforms">A list of transformations to chain with.</param>
        public EagerTransformation(List<Transformation> transforms)
            : base(transforms)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EagerTransformation"/> class.
        /// Creates an empty eager transformation object.
        /// </summary>
        public EagerTransformation()
            : base()
        {
        }

        /// <summary>
        /// Gets or sets a file format for the transformation.
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// Set file format for the transformation.
        /// </summary>
        /// <param name="format">The file format to set.</param>
        /// <returns>The transformation with file format defined.</returns>
        public EagerTransformation SetFormat(string format)
        {
            Format = format;
            return this;
        }

        /// <summary>
        /// Get this transformation represented as string.
        /// </summary>
        /// <returns>The transformation represented as string.</returns>
        public override string Generate()
        {
            string s = base.Generate();

            if (!string.IsNullOrEmpty(Format))
            {
                s += "/" + Format;
            }

            return s;
        }
    }
}