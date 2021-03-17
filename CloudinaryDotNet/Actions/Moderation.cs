namespace CloudinaryDotNet.Actions
{
    using System;
    using System.Runtime.Serialization;
    using Newtonsoft.Json;

    /// <summary>
    /// The data received from image moderation service.
    /// </summary>
    [DataContract]
    public class Moderation
    {
        /// <summary>
        /// Moderation status of assets.
        /// </summary>
        [DataMember(Name = "status")]
        public ModerationStatus Status;

        /// <summary>
        /// Type of image moderation service: "manual", "webpurify", "aws_rek", or "metascan".
        /// </summary>
        [DataMember(Name = "kind")]
        public string Kind;

        /// <summary>
        /// Result of the request for moderation.
        /// </summary>
        [DataMember(Name = "response")]
        [JsonConverter(typeof(ModerationResponseConverter))]
        public ModerationResponse Response;

        /// <summary>
        /// Date of the moderation status update.
        /// </summary>
        [DataMember(Name = "updated_at")]
        public DateTime UpdatedAt;
    }
}