using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CloudinaryDotNet.Actions
{
    [DataContract]
    public class DeleteFolderResult : BaseResult
    {
        [DataMember(Name = "deleted")]
        public List<string> Deleted { get; protected set; }
    }
}
