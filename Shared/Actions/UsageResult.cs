namespace CloudinaryDotNet.Actions
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// The report on the status of your Cloudinary account usage details. Note that numbers are updated periodically.
    /// </summary>
    [DataContract]
    public class UsageResult : BaseResult
    {
        /// <summary>
        /// Gets or sets your current plan.
        /// </summary>
        [DataMember(Name = "plan")]
        public string Plan { get; protected set; }

        /// <summary>
        /// Gets or sets date of the last report update.
        /// </summary>
        [DataMember(Name = "last_updated")]
        public DateTime LastUpdated { get; protected set; }

        /// <summary>
        /// Gets or sets information about the objects in your account.
        /// </summary>
        [DataMember(Name = "objects")]
        public Usage Objects { get; protected set; }

        /// <summary>
        /// Gets or sets information about usage of bandwith in your account.
        /// </summary>
        [DataMember(Name = "bandwidth")]
        public Usage Bandwidth { get; protected set; }

        /// <summary>
        /// Gets or sets information about usage of storage in your account.
        /// </summary>
        [DataMember(Name = "storage")]
        public Usage Storage { get; protected set; }

        /// <summary>
        /// Gets or sets count of requests used.
        /// </summary>
        [DataMember(Name = "requests")]
        public long Requests { get; protected set; }

        /// <summary>
        /// Gets or sets count of resources in your account.
        /// </summary>
        [DataMember(Name = "resources")]
        public int Resources { get; protected set; }

        /// <summary>
        /// Gets or sets count of derived resources.
        /// </summary>
        [DataMember(Name = "derived_resources")]
        public int DerivedResources { get; protected set; }

        /// <summary>
        /// Information about usage of transformations in your account.
        /// </summary>
        [DataMember(Name = "transformations")]
        public Usage Transformations { get; protected set; }

        /// <summary>
        /// Information about usage of webpurify in your account.
        /// </summary>
        [DataMember(Name = "webpurify")]
        public Usage Webpurify { get; protected set; }

        /// <summary>
        /// Information about usage of adv ocr in your account.
        /// </summary>
        [DataMember(Name = "adv_ocr")]
        public Usage AdvOcr { get; protected set; }

        /// <summary>
        /// Information about usage of aws rek moderation in your account.
        /// </summary>
        [DataMember(Name = "aws_rek_moderation")]
        public Usage AwsRekModeration { get; protected set; }

        /// <summary>
        /// Information about usage of search api in your account.
        /// </summary>
        [DataMember(Name = "search_api")]
        public Usage SearchApi { get; protected set; }

        /// <summary>
        /// Information about usage of url2png in your account.
        /// </summary>
        [DataMember(Name = "url2png")]
        public Usage Url2png { get; protected set; }

        /// <summary>
        /// Information about usage of aspose in your account.
        /// </summary>
        [DataMember(Name = "aspose")]
        public Usage Aspose { get; protected set; }

        /// <summary>
        /// Information about usage of style transfers in your account.
        /// </summary>
        [DataMember(Name = "style_transfer")]
        public Usage StyleTransfer { get; protected set; }

        /// <summary>
        /// Information about usage of azure video indexer in your account.
        /// </summary>
        [DataMember(Name = "azure_video_indexer")]
        public Usage AzureVideoIndexer { get; protected set; }

        /// <summary>
        /// Information about usage of object detection in your account.
        /// </summary>
        [DataMember(Name = "object_detection")]
        public Usage ObjectDetection { get; protected set; }

        /// <summary>
        /// Information about usage of credits in your account.
        /// </summary>
        [DataMember(Name = "credits")]
        public Usage Credits { get; protected set; }

        /// <summary>
        /// Information about file size limits per resource type.
        /// </summary>
        [DataMember(Name = "media_limits")]
        public Dictionary<string, long> MediaLimits { get; protected set; }
    }

    /// <summary>
    /// Usage information about the objects in account.
    /// </summary>
    [DataContract]
    public class Usage
    {
        /// <summary>
        /// Gets or sets a number of objects in your account.
        /// </summary>
        [DataMember(Name = "usage")]
        public long Used { get; protected set; }

        /// <summary>
        /// Gets or sets current limit of objects for account.
        /// </summary>
        [DataMember(Name = "limit")]
        public long Limit { get; protected set; }

        /// <summary>
        /// Gets or sets current usage percentage.
        /// </summary>
        [DataMember(Name = "used_percent")]
        public float UsedPercent { get; protected set; }

        /// <summary>
        /// Current usage of credits.
        /// </summary>
        [DataMember(Name = "credits_usage")]
        public float CreditsUsage { get; protected set; }
    }
}
