using System;
using System.Collections.Generic;

namespace CloudinaryDotNet.Actions
{
    public class ListResourcesParams : BaseParams
    {
        string m_tag;
        string m_prefix;
        string m_type;

        public ListResourcesParams()
        {
            NextCursor = String.Empty;
            Type = String.Empty;
            Prefix = String.Empty;
            Tags = false;
        }

        public ResourceType ResourceType { get; set; }

        /// <summary>
        /// Find all resources that their public ID starts with the given prefix.
        /// When Prefix is set Tag will be cleared.
        /// </summary>
        public string Prefix
        {
            get { return m_prefix; }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    Tag = String.Empty;
                    m_prefix = value;
                }
            }
        }

        public string Type
        {
            get { return m_type; }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    Tag = String.Empty;
                    m_type = value;
                }
            }
        }

        /// <summary>
        /// Max number of resources to return. Default=10. Maximum=500.
        /// When Tag is set Type and Prefix will be cleared.
        /// </summary>
        public string Tag
        {
            get { return m_tag; }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    Type = String.Empty;
                    Prefix = String.Empty;
                    m_tag = value;
                }
            }
        }

        /// <summary>
        /// Optional. Max number of resources to return. Default=10. Maximum=500.
        /// </summary>
        public int MaxResults { get; set; }

        /// <summary>
        /// Optional. Return tag information for every resources
        /// </summary>
        public bool Tags { get; set; }

        /// <summary>
        /// Optional.
        /// </summary>
        public string NextCursor { get; set; }

        /// <summary>
        /// Validate object model
        /// </summary>
        public override void Check()
        {
            // ok
        }

        /// <summary>
        /// Maps object model to dictionary of parameters in cloudinary notation
        /// </summary>
        /// <returns>Sorted dictionary of parameters</returns>
        public override SortedDictionary<string, object> ToParamsDictionary()
        {
            SortedDictionary<string, object> dict = new SortedDictionary<string, object>();

            if (MaxResults > 0)
                AddParam(dict, "max_results", MaxResults.ToString());

            AddParam(dict, "next_cursor", NextCursor);
            AddParam(dict, "type", Type);
            AddParam(dict, "prefix", Prefix);
            AddParam(dict, "tags", Tags);

            return dict;
        }
    }
}
