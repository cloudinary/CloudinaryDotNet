namespace CloudinaryDotNet.Actions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Base parameters of update streaming profile request.
    /// </summary>
    public class StreamingProfileBaseParams : BaseParams
    {
        /// <summary>
        /// A descriptive name for the profile.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// A list of representations that defines a custom streaming profile.
        /// </summary>
        public List<Representation> Representations { get; set; }

        /// <summary>
        /// Validate object model.
        /// </summary>
        public override void Check()
        {
            if (Representations == null || !Representations.Any())
            {
                throw new ArgumentException($"{nameof(Representations)} field must be specified and not empty");
            }
        }

        /// <summary>
        /// Maps object model to dictionary of parameters in cloudinary notation.
        /// </summary>
        /// <returns>Sorted dictionary of parameters.</returns>
        public override SortedDictionary<string, object> ToParamsDictionary()
        {
            SortedDictionary<string, object> dict = base.ToParamsDictionary();

            if (!string.IsNullOrEmpty(DisplayName))
            {
                dict.Add("display_name", DisplayName);
            }

            if (Representations != null)
            {
                dict.Add(
                        "representations",
                        JsonConvert.SerializeObject(
                                Representations,
                                new JsonSerializerSettings
                                {
                                    NullValueHandling = NullValueHandling.Ignore,
                                }));
            }

            return dict;
        }
    }

    /// <summary>
    /// Details of the transformation parameters for the representation.
    /// </summary>
    [DataContract]
    public class Representation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Representation"/> class.
        /// </summary>
        public Representation()
        {
        }

        /// <summary>
        /// Specifies the transformation parameters for the representation.
        /// All video transformation parameters except video_sampling are supported.
        /// Common transformation parameters for representations include: width, height
        /// (or aspect_ratio), bit_rate, video_codec, audio_codec, sample_rate (or fps), etc.
        /// </summary>
        [DataMember(Name = "transformation")]
        [JsonConverter(typeof(RepresentationsConverter))]
        public Transformation Transformation;

        /// <summary>
        /// Initializes a new instance of the <see cref="Representation"/> class.
        /// </summary>
        /// <param name="source">JSON token.</param>
        internal Representation(JToken source)
        {
            this.Transformation = new Transformation();

            // TODO: need to test if Newtonsoft works fine deserializing Transformation.
            // throw new Exception(source["transformation"].ToString());
            var transformations = source.ReadValueAsSnakeCase<IEnumerable<Dictionary<string, object>>>(nameof(Transformation));

            if (transformations?.Any() ?? false)
            {
                this.Transformation = new Transformation(transformations.First());
            }
        }
    }

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
            if (transformationsResponse != null && transformationsResponse.Count > 0)
            {
                foreach (JProperty jTransformProperty in transformationsResponse[0])
                {
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
