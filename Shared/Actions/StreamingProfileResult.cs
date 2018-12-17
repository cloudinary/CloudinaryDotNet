﻿using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CloudinaryDotNet.Actions
{
    [DataContract]
    public class StreamingProfileResult : BaseResult
    {
        [DataMember(Name = "message")]
        public string Message { get; protected set; }

        [DataMember(Name = "data")]
        public StreamingProfileData Data { get; protected set; }
    }

    [DataContract]
    public class StreamingProfileData : StreamingProfileBaseData
    {
        /// <summary>
        /// A collection of Representations that defines a custom streaming profile
        /// </summary>
        [DataMember(Name = "representations")]
        public List<Representation> Representations { get; set; }
    }

    [DataContract]
    public class StreamingProfileListResult : BaseResult
    {
        [DataMember(Name = "data")]
        public IEnumerable<StreamingProfileBaseData> Data { get; protected set; }
    }

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
        /// True if streaming profile is defined
        /// </summary>
        [DataMember(Name = "predefined")]
        public bool Predefined { get; protected set; }
    }
}
