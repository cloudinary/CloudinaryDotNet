﻿using System;
using System.Collections.Generic;
using System.Text;
using CloudinaryDotNet.Core;
using Newtonsoft.Json;

namespace CloudinaryDotNet.Actions
{
    /// <summary>
    /// Parameters to update details of an existing resource.
    /// </summary>
    public class UpdateParams : BaseParams
    {
        /// <summary>
        /// Instantiates the <see cref="UpdateParams"/> object with public ID.
        /// </summary>
        /// <param name="publicId">The public ID of the resource to update.</param>
        public UpdateParams(string publicId)
        {
            PublicId = publicId;
            Type = "upload";
        }

        /// <summary>
        /// The public ID of the resource to update.
        /// </summary>
        public string PublicId { get; set; }

        /// <summary>
        /// The type of file. Possible values: image, raw, video. Default: image.
        /// </summary>
        public ResourceType ResourceType { get; set; }

        /// <summary>
        /// The storage type: upload, private, authenticated, facebook, twitter, gplus, instagram_name, gravatar,
        /// youtube, hulu, vimeo, animoto, worldstarhiphop or dailymotion. Default: upload.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// An HTTP header or a list of headers lines for returning as response HTTP headers when delivering the
        /// uploaded image to your users. Supported headers: 'Link', 'X-Robots-Tag'.
        /// For example 'X-Robots-Tag: noindex'.
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
        /// Set to "aspose" to automatically convert Office documents to PDF files and other image formats using the
        /// Aspose Document Conversion add-on.
        /// </summary>
        public string RawConvert { get; set; }

        /// <summary>
        /// Sets the face coordinates. Use plain string (x,y,w,h|x,y,w,h) or <see cref="Rectangle"/>
        /// or <see cref="List{Rectangle}"/>.
        /// </summary>
        public object FaceCoordinates { get; set; }

        /// <summary>
        /// Coordinates of an interesting region contained in an uploaded image. The given coordinates are used for
        /// cropping uploaded images using the custom gravity mode. The region is specified by the X and Y coordinates
        /// of the top left corner and the width and height of the region. For example: "85,120,220,310".
        /// Otherwise, one can use <see cref="Rectangle"/> structure.
        /// </summary>
        public object CustomCoordinates { get; set; }

        /// <summary>
        /// Set to "rekognition_scene" to automatically detect scene categories of photos using the ReKognition Scene
        /// Categorization add-on.
        /// </summary>
        public string Categorization { get; set; }

        /// <summary>
        /// Set to "remove_the_background" to remove the background from the image.
        /// </summary>
        public string BackgroundRemoval { get; set; }

        /// <summary>
        /// Set to "rekognition_scene" to automatically detect scene categories of photos using the ReKognition Scene
        /// Categorization add-on.
        /// </summary>
        public float? AutoTagging { get; set; }

        /// <summary>
        /// Set to "rekognition_face" to automatically extract advanced face attributes of photos using the ReKognition
        /// Detect Face Attributes add-on.
        /// </summary>
        public string Detection { get; set; }

        /// <summary>
        /// Set to "tineye" to use the TinEye add-on.
        /// </summary>
        public string SimilaritySearch { get; set; }

        /// <summary>
        /// Optional. Set to 'adv_ocr' to extract all text elements in an image as well as the bounding box coordinates
        /// of each detected element using the OCR Text Detection and Extraction Add-on add-on.
        /// </summary>
        public string Ocr { get; set; }

        /// <summary>
        /// An HTTP or HTTPS URL to notify your application (a webhook) when the process has completed.
        /// </summary>
        public string NotificationUrl { get; set; }

        /// <summary>
        /// Override the default quality defined in the account level for a specific resource.
        /// </summary>
        [Obsolete("Property QualityOveride is deprecated, please use QualityOverride instead")]
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
        public ModerationStatus ModerationStatus { get; set; }

        /// <summary>
        /// Optional. Pass a list of AccessControlRule parameters.
        /// </summary>
        public List<AccessControlRule> AccessControl { get; set; }

        /// <summary>
        /// Validate object model.
        /// </summary>
        public override void Check()
        {
            if (String.IsNullOrEmpty(PublicId))
                throw new ArgumentException("PublicId must be set!");
        }

        /// <summary>
        /// Maps object model to dictionary of parameters in cloudinary notation.
        /// </summary>
        /// <returns>Sorted dictionary of parameters.</returns>
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
