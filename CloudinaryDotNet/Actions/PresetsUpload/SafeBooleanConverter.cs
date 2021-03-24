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
        /// Gets a value indicating whether this <see cref="CloudinaryDotNet.Actions.SafeBooleanConverter"/> can write JSON.
        /// </summary>
        public override bool CanWrite => false;

        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The <see cref="JsonReader"/> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>The object value.</returns>
        public override object ReadJson(
            JsonReader reader,
            Type objectType,
            object existingValue,
            JsonSerializer serializer)
        {
            var value = reader.Value;

            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return false;
            }

            var isBinaryString = value.ToString() == "0" || value.ToString() == "1";
            return isBinaryString ?
                Convert.ToBoolean(Convert.ToInt16(value, CultureInfo.InvariantCulture)) :
                Convert.ToBoolean(value, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>True if this instance can convert the specified object type; otherwise, false.</returns>
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string) || objectType == typeof(bool);
        }

        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="JsonWriter"/> to write to.</param>
        /// <param name="existingValue">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException("Unnecessary because of using just for Deserialization");
        }
    }
}