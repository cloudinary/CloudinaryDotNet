using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace CloudinaryDotNet.Actions
{
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
            if(Representations == null || !Representations.Any())
                throw new ArgumentException("Must be specified and not empty", nameof(Representations));
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
                dict.Add("representations",
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
        /// Specifies the transformation parameters for the representation.
        /// All video transformation parameters except video_sampling are supported.
        /// Common transformation parameters for representations include: width, height
        /// (or aspect_ratio), bit_rate, video_codec, audio_codec, sample_rate (or fps), etc.
        /// </summary>
        [DataMember(Name = "transformation")]
        [JsonConverter(typeof(RepresentationsConverter))]
        public Transformation Transformation;
    }

    internal class RepresentationsConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => true;

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

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(((Transformation)value).ToString());
        }
    }
}
