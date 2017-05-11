using System.IO;
using System.Net;
using System.Runtime.Serialization;

using System;
using System.Collections.Generic;

namespace CloudinaryDotNet.Actions
{
    /// <summary>
    /// Results of tags management
    /// </summary>
    [DataContract]
    public class ExplodeResult : BaseResult
    {
        [DataMember(Name = "status")]
        public string Status { get; protected set; }

        [DataMember(Name = "batch_id")]
        public string BatchId { get; protected set; }

    }
}
