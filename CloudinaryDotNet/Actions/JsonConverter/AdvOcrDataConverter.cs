namespace CloudinaryDotNet.Actions
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Custom JSON converter to handle ADV_OCR data that can be either an array of objects or an error string.
    /// </summary>
    public class AdvOcrDataConverter : JsonConverter
    {
        /// <summary>
        /// Gets a value indicating whether this <see cref="AdvOcrDataConverter"/> can write JSON.
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
            var jObject = JObject.Load(reader);
            var advOcr = new AdvOcr();

            // Handle status field
            if (jObject["status"] != null)
            {
                advOcr.Status = jObject["status"].Value<string>();
            }

            // Handle data field - can be array or string
            if (jObject["data"] != null)
            {
                var dataToken = jObject["data"];

                if (dataToken.Type == JTokenType.Array)
                {
                    advOcr.Data = dataToken.ToObject<List<AdvOcrData>>(serializer);
                }
                else if (dataToken.Type == JTokenType.String)
                {
                    advOcr.ErrorMessage = dataToken.Value<string>();
                }
            }

            return advOcr;
        }

        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>True if this instance can convert the specified object type; otherwise, false.</returns>
        public override bool CanConvert(Type objectType) => objectType == typeof(AdvOcr);

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
