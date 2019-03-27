using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace CloudinaryDotNet.Actions
{
    public class UpdateParams : BaseParams
    {
        public UpdateParams(string publicId)
        {
            PublicId = publicId;
            Type = "upload";
        }

        public string PublicId { get; set; }

        public ResourceType ResourceType { get; set; }

        public string Type { get; set; }

        /// <summary>
        /// An HTTP header or a list of headers lines for returning as response HTTP headers when delivering the uploaded image to your users. Supported headers: 'Link', 'X-Robots-Tag'. For example 'X-Robots-Tag: noindex'.
        /// </summary>
        public Dictionary<string, string> Headers { get; set; }

        /// <summary>
        /// A comma-separated list of tag names to assign to the uploaded image for later group reference.
        /// </summary>
        public string Tags { get; set; }

        /// <summary>
        /// Allows to store a set of key-value pairs together with resource.
        /// </summary>
        public StringDictionary Context { get; set; }

        /// <summary>
        /// Set to "aspose" to automatically convert Office documents to PDF files and other image formats using the Aspose Document Conversion add-on.
        /// </summary>
        public string RawConvert { get; set; }

        /// <summary>
        /// Sets the face coordinates. Use plain string (x,y,w,h|x,y,w,h) or <see cref="Rectangle"/> or <see cref="List{Rectangle}"/>.
        /// </summary>
        public object FaceCoordinates { get; set; }

        /// <summary>
        /// Coordinates of an interesting region contained in an uploaded image. The given coordinates are used for cropping uploaded images using the custom gravity mode. The region is specified by the X and Y coordinates of the top left corner and the width and height of the region. For example: "85,120,220,310". Otherwise, one can use <see cref="Rectangle"/> structure.
        /// </summary>
        public object CustomCoordinates { get; set; }

        /// <summary>
        /// Set to "rekognition_scene" to automatically detect scene categories of photos using the ReKognition Scene Categorization add-on.
        /// </summary>
        public string Categorization { get; set; }

        /// <summary>
        /// Set to "remove_the_background" to remove the background from the image.
        /// </summary>
        public string BackgroundRemoval { get; set; }

        /// <summary>
        /// Set to "rekognition_scene" to automatically detect scene categories of photos using the ReKognition Scene Categorization add-on.
        /// </summary>
        public float? AutoTagging { get; set; }

        /// <summary>
        /// Set to "rekognition_face" to automatically extract advanced face attributes of photos using the ReKognition Detect Face Attributes add-on.
        /// </summary>
        public string Detection { get; set; }

        public string SimilaritySearch { get; set; }

        public string Ocr { get; set; }

        public string NotificationUrl { get; set; }

        /// <summary>
        /// Override the default quality defined in the account level for a specific resource.
        /// </summary>
        [Obsolete]
        public string QualityOveride
        {
            get { return QualityOverride; }
            set { QualityOverride = value; }
        }

        /// <summary>
        /// Override the default quality defined in the account level for a specific resource.
        /// </summary>
        public string QualityOverride { get; set; }

        /// <summary>
        /// Gets or sets the moderation status.
        /// </summary>
        /// <value>
        /// The moderation status.
        /// </value>
        public ModerationStatus ModerationStatus { get; set; }

        /// <summary>
        /// Optional. Pass a list of AccessControlRule parameters
        /// </summary>
        public List<AccessControlRule> AccessControl { get; set; }

        /// <summary>
        /// Validate object model
        /// </summary>
        public override void Check()
        {
            if (String.IsNullOrEmpty(PublicId))
                throw new ArgumentException("PublicId must be set!");
        }

        /// <summary>
        /// Maps object model to dictionary of parameters in cloudinary notation
        /// </summary>
        /// <returns>Sorted dictionary of parameters</returns>
        public override SortedDictionary<string, object> ToParamsDictionary()
        {
            SortedDictionary<string, object> dict = base.ToParamsDictionary();

            AddParam(dict, "public_id", PublicId);
            AddParam(dict, "tags", Tags);
            AddParam(dict, "type", Type);
            AddParam(dict, "categorization", Categorization);
            AddParam(dict, "detection", Detection);
            AddParam(dict, "ocr", Ocr);
            AddParam(dict, "similarity_search", SimilaritySearch);
            AddParam(dict, "background_removal", BackgroundRemoval);

            if(!string.IsNullOrWhiteSpace(NotificationUrl))
                AddParam(dict, "notification_url", NotificationUrl);

            if (ModerationStatus != Actions.ModerationStatus.Pending)
                AddParam(dict, "moderation_status", ApiShared.GetCloudinaryParam(ModerationStatus));

            if (AutoTagging.HasValue)
                AddParam(dict, "auto_tagging", AutoTagging.Value);

            AddParam(dict, "raw_convert", RawConvert);

            if (Context != null && Context.Count > 0)
            {
                AddParam(dict, Constants.CONTEXT_PARAM_NAME, Utils.SafeJoin("|", Context.SafePairs));
            }

            AddCoordinates(dict, "face_coordinates", FaceCoordinates);
            AddCoordinates(dict, "custom_coordinates", CustomCoordinates);

            if (!string.IsNullOrWhiteSpace(QualityOverride))
                AddParam(dict, "quality_override", QualityOverride);

            if (Headers != null && Headers.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var item in Headers)
                {
                    sb.AppendFormat("{0}: {1}\n", item.Key, item.Value);
                }

                dict.Add("headers", sb.ToString());
            }

            if (AccessControl != null && AccessControl.Count > 0)
            {
                AddParam(dict, "access_control", JsonConvert.SerializeObject(AccessControl));
            }

            return dict;
        }
    }
}
