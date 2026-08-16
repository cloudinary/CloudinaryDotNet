namespace CloudinaryDotNet.Actions
{
    using System;
    using System.Globalization;
    using System.Numerics;
    using Newtonsoft.Json;

    /// <summary>
    /// Custom JSON converter that maps the unsigned 32-bit representation of -1 to <see cref="UnknownValue"/>.
    /// </summary>
    /// <remarks>
    /// Some upstream services report an unknown numeric value as an unsigned 32-bit -1, which reaches the API as
    /// 4294967295. Binding that to <see cref="int"/> overflows, and because a single field failure aborts the whole
    /// bind, the entire response is discarded. Binding it to <see cref="long"/> does not overflow but silently
    /// records a nonsensical count. This converter translates that one sentinel to <see cref="UnknownValue"/> in
    /// both cases, so the rest of the response still deserializes and the affected field is recognisable. Any other
    /// value outside the range of the target is genuinely unexpected and is still reported as an error rather than
    /// silently reinterpreted.
    /// <para>
    /// Every input other than the sentinel is converted exactly as <c>JsonReader.ReadAsInt32</c> would, including
    /// its rounding of fractional values, so that adding this converter changes nothing else. Two divergences are
    /// deliberate: a null or empty value yields the target's default rather than failing the whole response, and an
    /// oversized value is reported as a <see cref="JsonException"/> rather than letting a raw
    /// <see cref="OverflowException"/> escape the serializer.
    /// </para>
    /// </remarks>
    public class SafeIntConverter : JsonConverter
    {
        /// <summary>
        /// The value reported when the source encodes an unknown count as an unsigned 32-bit -1.
        /// </summary>
        public const int UnknownValue = -1;

        /// <summary>
        /// The unsigned 32-bit representation of -1, as produced by an upstream integer wrap-around.
        /// </summary>
        private const long UnsignedNegativeOne = 4294967295L;

        /// <summary>
        /// Gets a value indicating whether this <see cref="SafeIntConverter"/> can write JSON.
        /// </summary>
        public override bool CanWrite => false;

        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>True if this instance can convert the specified object type; otherwise, false.</returns>
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(int) || objectType == typeof(int?) ||
                   objectType == typeof(long) || objectType == typeof(long?);
        }

        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The <see cref="JsonReader"/> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of the object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>The object value.</returns>
        public override object ReadJson(
            JsonReader reader,
            Type objectType,
            object existingValue,
            JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return Empty(objectType);
            }

            // JsonReader.ReadAsInt32 accepts only these tokens; anything else is an
            // error there, so reject it here rather than letting System.Convert
            // quietly coerce values such as booleans.
            if (reader.TokenType != JsonToken.Integer &&
                reader.TokenType != JsonToken.Float &&
                reader.TokenType != JsonToken.String)
            {
                throw new JsonSerializationException(
                    $"Error reading integer. Unexpected token: {reader.TokenType}.");
            }

            if (IsWrapAround(reader.Value))
            {
                return IsInt64(objectType) ? (object)(long)UnknownValue : UnknownValue;
            }

            return Convert(reader, objectType);
        }

        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="JsonWriter"/> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException("Unnecessary because this converter is used primarily for deserialization.");
        }

        /// <summary>
        /// Determines whether the target property holds a 64-bit integer.
        /// </summary>
        /// <param name="objectType">Type of the target property.</param>
        /// <returns>True when the target is <see cref="long"/> or <see cref="Nullable{Int64}"/>.</returns>
        private static bool IsInt64(Type objectType)
        {
            return objectType == typeof(long) || objectType == typeof(long?);
        }

        /// <summary>
        /// Produces the value used when the JSON holds null or an empty string.
        /// </summary>
        /// <param name="objectType">Type of the target property.</param>
        /// <returns>Null for nullable targets, otherwise the target's default value.</returns>
        private static object Empty(Type objectType)
        {
            if (objectType == typeof(int?) || objectType == typeof(long?))
            {
                return null;
            }

            return IsInt64(objectType) ? (object)default(long) : default(int);
        }

        /// <summary>
        /// Determines whether the raw JSON value is the upstream wrap-around sentinel.
        /// </summary>
        /// <param name="value">The value carried by the reader.</param>
        /// <returns>True when the value is the unsigned 32-bit representation of -1.</returns>
        /// <remarks>
        /// A value of 4294967295 reaches the reader boxed as <see cref="long"/>, as <see cref="double"/> when
        /// written in float or exponent form, or as <see cref="string"/> when quoted. Narrower types cannot hold
        /// it, and wider ones are not produced for a value of this magnitude, so no other case can match.
        /// </remarks>
        private static bool IsWrapAround(object value)
        {
            switch (value)
            {
                case long longValue:
                    return longValue == UnsignedNegativeOne;

                case double doubleValue:
                    return doubleValue.Equals((double)UnsignedNegativeOne);

                case string stringValue:
                    return long.TryParse(
                               stringValue,
                               NumberStyles.Integer,
                               CultureInfo.InvariantCulture,
                               out var parsed) &&
                           parsed == UnsignedNegativeOne;

                default:
                    return false;
            }
        }

        /// <summary>
        /// Converts the value exactly as Newtonsoft's own reader would, so that every
        /// input other than the sentinel keeps its established behaviour.
        /// </summary>
        /// <param name="reader">The reader positioned on the value.</param>
        /// <param name="objectType">Type of the target property.</param>
        /// <returns>The converted value, boxed as the target's underlying type.</returns>
        private static object Convert(JsonReader reader, Type objectType)
        {
            var value = reader.Value;

            if (value is string text)
            {
                if (string.IsNullOrWhiteSpace(text))
                {
                    return Empty(objectType);
                }

                // Mirrors JsonReader.ReadInt32String, which parses with the reader's culture.
                var culture = reader.Culture ?? CultureInfo.InvariantCulture;
                if (IsInt64(objectType))
                {
                    return long.TryParse(text, NumberStyles.Integer, culture, out var parsedLong)
                        ? parsedLong
                        : throw new JsonSerializationException(
                            $"Could not convert string to integer: {text}.");
                }

                return int.TryParse(text, NumberStyles.Integer, culture, out var parsedInt)
                    ? parsedInt
                    : throw new JsonSerializationException(
                        $"Could not convert string to integer: {text}.");
            }

            // Mirrors JsonReader.ReadAsInt32: BigInteger is cast, everything else goes
            // through System.Convert, which rounds rather than truncates.
            try
            {
                if (value is BigInteger bigValue)
                {
                    return IsInt64(objectType) ? (object)(long)bigValue : (int)bigValue;
                }

                return IsInt64(objectType)
                    ? (object)System.Convert.ToInt64(value, CultureInfo.InvariantCulture)
                    : System.Convert.ToInt32(value, CultureInfo.InvariantCulture);
            }
            catch (Exception ex) when (ex is OverflowException || ex is FormatException || ex is InvalidCastException)
            {
                throw new JsonSerializationException(
                    $"Could not convert to integer: {value}.", ex);
            }
        }
    }
}
