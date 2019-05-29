using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CloudinaryDotNet.Actions
{
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
        /// 
        /// </summary>
        [DataMember(Name = "data")]
        public StreamingProfileData Data { get; protected set; }
    }

    /// <summary>
    /// Details of the streaming profile.
    /// </summary>
    [DataContract]
    public class StreamingProfileData : StreamingProfileBaseData
    {
        /// <summary>
        /// A collection of Representations that defines a custom streaming profile
        /// </summary>
        [DataMember(Name = "representations")]
        public List<Representation> Representations { get; set; }
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
    }

    /// <summary>
    /// Basic details of the streaming profile.
    /// </summary>
    [DataContract]
    public class StreamingProfileBaseData
    {
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
    }
}
