namespace CloudinaryDotNet.Actions
{
    using System;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Instructions on how to serialize the instances of <see cref="Transformation"/> class.
    /// </summary>
    internal class RepresentationsConverter : JsonConverter
    {
        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns> True if this instance can convert the specified object type; otherwise, false.</returns>
        public override bool CanConvert(Type objectType) => true;

        /// <summary>Reads the JSON representation of the object.</summary>
        /// <param name="reader">The <see cref="Newtonsoft.Json.JsonReader" /> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>The object value.</returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            var transformation = new Transformation();
            var transformationsResponse = JArray.Load(reader);
            if (transformationsResponse.Count > 0)
            {
                foreach (var jToken in transformationsResponse[0])
                {
                    var jTransformProperty = (JProperty)jToken;
                    transformation.Add(jTransformProperty.Name, jTransformProperty.Value);
                }
            }

            return transformation;
        }

        /// <summary>Writes the JSON representation of the object.</summary>
        /// <param name="writer">The <see cref="Newtonsoft.Json.JsonWriter" /> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(((Transformation)value).ToString());
        }
    }
}
