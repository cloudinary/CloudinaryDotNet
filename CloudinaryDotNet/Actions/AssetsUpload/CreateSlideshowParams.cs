namespace CloudinaryDotNet.Actions
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// Parameters of generating a slideshow.
    /// </summary>
    public class CreateSlideshowParams : BaseParams
    {
        /// <summary>
        /// Gets or sets the manifest transformation for slideshow creation.
        /// </summary>
        public Transformation ManifestTransformation { get; set; }

        /// <summary>
        /// Gets or sets the manifest json for slideshow creation.
        /// </summary>
        public SlideshowManifest ManifestJson { get; set; }

        /// <summary>
        /// Gets or sets the identifier that is used for accessing the generated video.
        /// </summary>
        public string PublicId { get; set; }

        /// <summary>
        /// Gets or sets an additional transformation to run on the created slideshow before saving it in the cloud.
        /// For example: limit the dimensions of the uploaded image to 512x512 pixels.
        /// </summary>
        public Transformation Transformation { get; set; }

        /// <summary>
        /// Gets or sets a list of tag names to assign to the generated slideshow.
        /// </summary>
        /// <returns>A list of strings where each element represents a tag name.</returns>
        public List<string> Tags { get; set; }

        /// <summary>
        /// Gets or sets whether to overwrite existing resources with the same public ID.
        /// </summary>
        public bool? Overwrite { get; set; }

        /// <summary>
        /// Gets or sets an HTTP URL to send notification to (a webhook) when the operation or any additional
        /// requested asynchronous action is completed. If not specified,
        /// the response is sent to the global Notification URL (if defined)
        /// in the Upload settings of your account console.
        /// </summary>
        public string NotificationUrl { get; set; }

        /// <summary>
        /// Gets or sets whether it is allowed to use an upload preset for setting parameters of this upload (optional).
        /// </summary>
        public string UploadPreset { get; set; }

        /// <summary>
        /// Validate object model.
        /// </summary>
        public override void Check()
        {
            if (ManifestTransformation == null && ManifestJson == null)
            {
                throw new ArgumentException("Please specify ManifestTransformation or ManifestJson");
            }
        }

        /// <summary>
        /// Maps object model to dictionary of parameters in cloudinary notation.
        /// </summary>
        /// <returns>Sorted dictionary of parameters.</returns>
        public override SortedDictionary<string, object> ToParamsDictionary()
        {
            SortedDictionary<string, object> dict = base.ToParamsDictionary();

            AddParam(dict, "manifest_json", JsonConvert.SerializeObject(ManifestJson, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore,
            }));
            AddParam(dict, "manifest_transformation", ManifestTransformation?.Generate());
            AddParam(dict, "public_id", PublicId);
            AddParam(dict, "transformation", Transformation?.Generate());
            AddParam(dict, "tags", Tags);
            AddParam(dict, "overwrite", Overwrite);
            AddParam(dict, "notification_url", NotificationUrl);
            AddParam(dict, "upload_preset", UploadPreset);

            return dict;
        }
    }
}
