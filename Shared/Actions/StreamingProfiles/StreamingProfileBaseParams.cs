namespace CloudinaryDotNet.Actions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Newtonsoft.Json;

    /// <summary>
    /// Base parameters of update streaming profile request.
    /// </summary>
    public class StreamingProfileBaseParams : BaseParams
    {
        /// <summary>
        /// Gets or sets a descriptive name for the profile.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets a list of representations that defines a custom streaming profile.
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
}
