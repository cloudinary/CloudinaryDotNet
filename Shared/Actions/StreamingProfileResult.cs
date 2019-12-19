namespace CloudinaryDotNet.Actions
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Detailed information about streaming profile.
    /// </summary>
    [DataContract]
    public class StreamingProfileResult : BaseResult
    {
        /// <summary>
        /// An API message.
        /// </summary>
        [DataMember(Name = "message")]
        public string Message { get; protected set; }

        /// <summary>
        /// Details of the streaming profile.
        /// </summary>
        [DataMember(Name = "data")]
        public StreamingProfileData Data { get; protected set; }

        /// <inheritdoc/>
        internal override void SetValues(JToken source)
        {
            base.SetValues(source);
            this.Message = source.ReadValueAsSnakeCase<string>(nameof(Message));
            Data = source.ReadObject(nameof(Data).ToSnakeCase(), _ => new StreamingProfileData(_));
        }
    }

    /// <summary>
    /// Details of the streaming profile.
    /// </summary>
    [DataContract]
    public class StreamingProfileData : StreamingProfileBaseData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StreamingProfileData"/> class.
        /// </summary>
        public StreamingProfileData()
            : base()
        {
        }

        /// <summary>
        /// A collection of Representations that defines a custom streaming profile.
        /// </summary>
        [DataMember(Name = "representations")]
        public List<Representation> Representations { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StreamingProfileData"/> class.
        /// </summary>
        /// <param name="source">JSON token.</param>
        internal StreamingProfileData(JToken source)
            : base(source)
        {
            Representations = source.ReadValueAsSnakeCase<List<JObject>>(nameof(Representations))
                .Select(_ => new Representation(_))
                .ToList();
        }
    }

    /// <summary>
    /// Result of listing of streaming profiles.
    /// </summary>
    [DataContract]
    public class StreamingProfileListResult : BaseResult
    {
        /// <summary>
        /// List of basic details of the streaming profiles.
        /// </summary>
        [DataMember(Name = "data")]
        public IEnumerable<StreamingProfileBaseData> Data { get; protected set; }

        /// <inheritdoc/>
        internal override void SetValues(JToken source)
        {
            base.SetValues(source);
            Data = source.ReadList(nameof(Data).ToSnakeCase(), _ => new StreamingProfileBaseData(_));
        }
    }

    /// <summary>
    /// Basic details of the streaming profile.
    /// </summary>
    [DataContract]
    public class StreamingProfileBaseData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StreamingProfileBaseData"/> class.
        /// </summary>
        public StreamingProfileBaseData()
        {
        }

        /// <summary>
        /// The identification name of the new streaming profile.
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; protected set; }

        /// <summary>
        /// A descriptive name for the profile.
        /// </summary>
        [DataMember(Name = "display_name")]
        public string DisplayName { get; protected set; }

        /// <summary>
        /// True if streaming profile is defined.
        /// </summary>
        [DataMember(Name = "predefined")]
        public bool Predefined { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StreamingProfileBaseData"/> class.
        /// </summary>
        /// <param name="source">The source JSON token.</param>
        internal StreamingProfileBaseData(JToken source)
        {
            Name = source.ReadValueAsSnakeCase<string>(nameof(Name));
            DisplayName = source.ReadValueAsSnakeCase<string>(nameof(DisplayName));
            Predefined = source.ReadValueAsSnakeCase<bool>(nameof(Predefined));
        }
    }
}
