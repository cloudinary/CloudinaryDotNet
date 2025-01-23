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
            // We'll parse everything into a 'bool?' and decide how to return
            // based on whether the target is bool or bool?.
            bool? parsedValue = null;

            if (reader.TokenType == JsonToken.Null ||
                (reader.Value is string strVal && string.IsNullOrWhiteSpace(strVal)))
            {
                parsedValue = null;
            }

            // Handle Boolean token directly: e.g., true or false
            else if (reader is { TokenType: JsonToken.Boolean, Value: bool boolValue })
            {
                parsedValue = boolValue;
            }

            // Handle integer "0" or "1"
            else if (reader is { TokenType: JsonToken.Integer, Value: not null })
            {
                var intValue = Convert.ToInt64(reader.Value, CultureInfo.InvariantCulture);
                parsedValue = intValue == 1;
            }

            // Handle string values: "0", "1", "true", "false", etc.
            else if (reader is { TokenType: JsonToken.String, Value: string stringValue })
            {
                switch (stringValue)
                {
                    case "0":
                        parsedValue = false;
                        break;
                    case "1":
                        parsedValue = true;
                        break;
                    default:
                        if (bool.TryParse(stringValue, out var boolFromString))
                        {
                            parsedValue = boolFromString;
                        }
                        else
                        {
                            throw new JsonSerializationException(
                                $"Unable to parse \"{stringValue}\" as a boolean value.");
                        }

                        break;
                }
            }
            else
            {
                // Unexpected token => throw
                throw new JsonSerializationException(
                    $"Unexpected token {reader.TokenType} when parsing a boolean.");
            }

            // If we're targeting bool? => return the parsedValue (can be null or bool).
            // If we're targeting bool => return false if parsedValue is null, else the bool value.
            if (objectType == typeof(bool?))
            {
                return parsedValue;
            }

            // objectType == typeof(bool)
            return parsedValue ?? false;
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
