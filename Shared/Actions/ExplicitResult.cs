namespace CloudinaryDotNet.Actions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Parsed response after a call of Explicit method.
    /// </summary>
    [DataContract]
    public class ExplicitResult : RawUploadResult
    {
        /// <summary>
        /// The specific type of asset.
        /// </summary>
        /// <summary>
        /// The derived images generated as per the requested eager transformations of the method call.
        /// </summary>
        [DataMember(Name = "eager")]
        public Eager[] Eager { get; protected set; }

        /// <summary>
        /// List of responsive breakpoint settings of the asset.
        /// </summary>
        [DataMember(Name = "responsive_breakpoints")]
        public List<ResponsiveBreakpointList> ResponsiveBreakpoints { get; set; }

        /// <summary>
        /// Status is returned when passing 'Async' argument set to 'true' to the server.
        /// </summary>
        [DataMember(Name = "status")]
        public string Status { get; protected set; }

        /// <summary>
        /// Any requested information from executing one of the Cloudinary Add-ons on the media asset.
        /// </summary>
        [DataMember(Name = "info")]
        public Info Info { get; protected set; }

        /// <summary>
        /// Details of the quality analysis.
        /// </summary>
        [DataMember(Name = "quality_analysis")]
        public QualityAnalysis QualityAnalysis { get; protected set; }

        /// <summary>
        /// Overrides corresponding method of <see cref="BaseResult"/> class.
        /// Populates additional token fields.
        /// </summary>
        /// <param name="source">JSON token received from the server.</param>
        internal override void SetValues(JToken source)
        {
            base.SetValues(source);
            Status = source.ReadValueAsSnakeCase<string>(nameof(Status));
            ResponsiveBreakpoints = source.ReadObject(nameof(ResponsiveBreakpoints).ToSnakeCase(), _ => _.ToObject<List<ResponsiveBreakpointList>>());
            QualityAnalysis = source.ReadObject(nameof(QualityAnalysis).ToSnakeCase(), _ => new QualityAnalysis(_));
            Info = source.ReadObject(nameof(Info).ToSnakeCase(), _ => new Info(_));
            Eager = source.ReadList(nameof(Eager).ToSnakeCase(), _ => new Eager(_)).ToArray();
        }
    }

    /// <summary>
    /// Details of the derived image generated as per the requested eager transformation applied.
    /// </summary>
    [DataContract]
    public class Eager
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Eager"/> class.
        /// </summary>
        public Eager()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Eager"/> class.
        /// </summary>
        /// <param name="source">JSON token.</param>
        internal Eager(JToken source)
        {
            Uri = source.ReadValueAsSnakeCase<Uri>("Url");
            SecureUri = source.ReadValueAsSnakeCase<Uri>("SecureUrl");
        }

        /// <summary>
        /// URL for accessing the asset.
        /// </summary>
        [DataMember(Name = "url")]
        public Uri Uri { get; protected set; }

        /// <summary>
        /// The HTTPS URL for securely accessing the asset.
        /// </summary>
        [DataMember(Name = "secure_url")]
        public Uri SecureUri { get; protected set; }
    }
}
