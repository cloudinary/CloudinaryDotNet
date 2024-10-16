namespace CloudinaryDotNet.Actions
{
    using System.Collections.Generic;

    /// <summary>
    /// Parameters for getting the Cloudinary account configuration.
    /// </summary>
    public class ConfigParams : BaseParams
    {
        /// <summary>
        /// Gets or sets a value indicating whether to include settings in the response.
        /// </summary>
        public bool Settings { get; set; }

        /// <summary>
        /// Validates the object model.
        /// </summary>
        /// <exception cref="System.ArgumentException">Throw an exception if parameters are invalid.</exception>
        public override void Check()
        {
            // Currently, no validation required
        }

        /// <summary>
        /// Maps object model to a dictionary of parameters in Cloudinary notation.
        /// </summary>
        /// <returns>Sorted dictionary of parameters.</returns>
        public override SortedDictionary<string, object> ToParamsDictionary()
        {
            var dict = new SortedDictionary<string, object>();

            if (Settings)
            {
                dict.Add("settings", "true");
            }

            return dict;
        }
    }
}
