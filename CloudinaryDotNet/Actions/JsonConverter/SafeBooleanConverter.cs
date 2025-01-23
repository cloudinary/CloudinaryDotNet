namespace CloudinaryDotNet.Actions
{
    using System;
    using System.Globalization;
    using Newtonsoft.Json;

    /// <summary>
    /// Custom JSON converter to safely parse boolean values that might come as strings.
    /// </summary>
    public class SafeBooleanConverter : JsonConverter
    {
        /// <summary>
        /// Gets a value indicating whether this <see cref="SafeBooleanConverter"/> can write JSON.
        /// </summary>
        public override bool CanWrite => false;

        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>True if this instance can convert the specified object type; otherwise, false.</returns>
        public override bool CanConvert(Type objectType)
        {
            // Decide which types you want this converter to handle
            return objectType == typeof(bool) || objectType == typeof(bool?);
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
            // If the token is null or empty, return false
            if (reader.TokenType == JsonToken.Null ||
                (reader.Value is string strVal && string.IsNullOrWhiteSpace(strVal)))
            {
                return false;
            }

            // Handle Boolean token directly
            if (reader is { TokenType: JsonToken.Boolean, Value: bool boolValue })
            {
                return boolValue;
            }

            // Handle numeric "0" or "1"
            if (reader is { TokenType: JsonToken.Integer, Value: not null })
            {
                var intValue = Convert.ToInt64(reader.Value, CultureInfo.InvariantCulture);
                return intValue == 1;
            }

            // Handle string values: "0", "1", "true", "false", etc.
            if (reader is { TokenType: JsonToken.String, Value: string stringValue })
            {
                switch (stringValue)
                {
                    case "0":
                        return false;
                    case "1":
                        return true;
                }

                if (bool.TryParse(stringValue, out var parsedBool))
                {
                    return parsedBool;
                }

                throw new JsonSerializationException(
                    $"Unable to parse \"{stringValue}\" as a boolean value.");
            }

            throw new JsonSerializationException(
                $"Unexpected token {reader.TokenType} when parsing a boolean.");
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
    }
}
