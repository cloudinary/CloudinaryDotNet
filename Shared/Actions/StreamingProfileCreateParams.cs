﻿using System;
using System.Collections.Generic;

namespace CloudinaryDotNet.Actions
{
    /// <summary>
    /// Parameters of create streaming profile request.
    /// </summary>
    public class StreamingProfileCreateParams : StreamingProfileBaseParams
    {
        /// <summary>
        /// The identification name to assign to the new streaming profile. The name is 
        /// case-insensitive and can contain alphanumeric characters, underscores (_) and hyphens (-).
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Validate object model.
        /// </summary>
        public override void Check()
        {
            if (string.IsNullOrEmpty(Name))
                throw new ArgumentException("Must be specified", nameof(Name));

            base.Check();
        }

        /// <summary>
        /// Maps object model to dictionary of parameters in cloudinary notation.
        /// </summary>
        /// <returns>Sorted dictionary of parameters.</returns>
        public override SortedDictionary<string, object> ToParamsDictionary()
        {
            SortedDictionary<string, object> dict = base.ToParamsDictionary();
            dict.Add("name", Name);
            return dict;
        }
    }
}
