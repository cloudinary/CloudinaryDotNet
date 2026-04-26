namespace CloudinaryDotNet
{
    /// <summary>
    /// Wrapper that lets API methods accept either a raw transformation string
    /// (e.g. "w_200,h_200,c_fill") or a <see cref="Transformation"/> instance via implicit conversion.
    /// </summary>
    public sealed class TransformationParam
    {
        private TransformationParam(string value)
        {
            Value = value;
        }

        /// <summary>
        /// Gets the serialised transformation string.
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Implicitly converts a string into a <see cref="TransformationParam"/>.
        /// </summary>
        /// <param name="value">The raw transformation string.</param>
        public static implicit operator TransformationParam(string value) => FromString(value);

        /// <summary>
        /// Implicitly converts a <see cref="Transformation"/> into a <see cref="TransformationParam"/>.
        /// </summary>
        /// <param name="transformation">The transformation instance.</param>
        public static implicit operator TransformationParam(Transformation transformation) => FromTransformation(transformation);

        /// <summary>
        /// Creates a <see cref="TransformationParam"/> from a raw transformation string.
        /// </summary>
        /// <param name="value">The raw transformation string.</param>
        /// <returns>The wrapper, or null when <paramref name="value"/> is null.</returns>
        public static TransformationParam FromString(string value) =>
            value == null ? null : new TransformationParam(value);

        /// <summary>
        /// Creates a <see cref="TransformationParam"/> from a <see cref="Transformation"/> instance.
        /// </summary>
        /// <param name="transformation">The transformation instance.</param>
        /// <returns>The wrapper, or null when <paramref name="transformation"/> is null.</returns>
        public static TransformationParam FromTransformation(Transformation transformation) =>
            transformation == null ? null : new TransformationParam(transformation.Generate());

        /// <summary>
        /// Returns the serialised transformation string.
        /// </summary>
        /// <returns>The transformation string.</returns>
        public override string ToString() => Value;
    }
}
