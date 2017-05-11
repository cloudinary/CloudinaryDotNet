﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Net;

namespace CloudinaryDotNet.Actions
{
    [DataContract]
    public class ExplicitResult : RawUploadResult
    {
        [DataMember(Name = "format")]
        public string Format { get; protected set; }

        [DataMember(Name = "type")]
        public string Type { get; protected set; }

        [DataMember(Name = "eager")]
        public Eager[] Eager { get; protected set; }

        /// <summary>
        /// List of responsive image breakpoints
        /// </summary>
        public List<ResponsiveBreakpointList> ResponsiveBreakpoints { get; set; }

        protected override void OnParse()
        {
            if (JsonObj != null)
            {
                var responsiveBreakpoints = JsonObj["responsive_breakpoints"];
                if (responsiveBreakpoints != null)
                {
                    ResponsiveBreakpoints = responsiveBreakpoints.ToObject<List<ResponsiveBreakpointList>>();
                }
            }
        }
    }

    [DataContract]
    public class Eager
    {
        [DataMember(Name = "url")]
        public Uri Uri { get; protected set; }

        [DataMember(Name = "secure_url")]
        public Uri SecureUri { get; protected set; }
    }
}
