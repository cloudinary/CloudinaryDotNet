using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;

using System.Text;

namespace CloudinaryDotNet.Actions
{
    [DataContract]
    public class TransformResult : BaseResult
    {
        [DataMember(Name = "message")]
        public string Message { get; protected set; }

    }
}
