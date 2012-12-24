using System;
using System.Collections.Generic;

namespace CloudinaryDotNet.Actions
{
    public class DelResParams : BaseParams
    {
        List<string> m_publicIds = new List<string>();
        string m_prefix;
        string m_tag;

        public DelResParams()
        {
            Type = "upload";
        }

        public ResourceType ResourceType { get; set; }

        public string Type { get; set; }

        /// <summary>
        /// If true, delete only the derived images of the matching resources.
        /// </summary>
        public bool KeepOriginal { get; set; }

        /// <summary>
        /// Delete all resources with the given public IDs
        /// </summary>
        public List<string> PublicIds
        {
            get { return m_publicIds; }
            set { m_publicIds = value; m_prefix = String.Empty; m_tag = String.Empty; }
        }

        /// <summary>
        /// Delete all resources that their public ID starts with the given prefix.
        /// </summary>
        public string Prefix
        {
            get { return m_prefix; }
            set { m_publicIds = null; m_tag = String.Empty; m_prefix = value; }
        }

        public string Tag
        {
            get { return m_tag; }
            set { m_publicIds = null; m_prefix = String.Empty; m_tag = value; }
        }

        /// <summary>
        /// Validate object model
        /// </summary>
        public override void Check()
        {
            if ((PublicIds == null || PublicIds.Count == 0) &&
                String.IsNullOrEmpty(Prefix) &&
                String.IsNullOrEmpty(Tag))
            {
                throw new ArgumentException("Either PublicIds or Prefix or Tag must be specified!");
            }

            if (String.IsNullOrEmpty(Tag) && String.IsNullOrEmpty(Type))
                throw new ArgumentException("Type of resource must be specified!");
        }

        /// <summary>
        /// Maps object model to dictionary of parameters in cloudinary notation
        /// </summary>
        /// <returns>Sorted dictionary of parameters</returns>
        public override SortedDictionary<string, object> ToParamsDictionary()
        {
            SortedDictionary<string, object> dict = new SortedDictionary<string, object>();

            dict.Add("keep_original", KeepOriginal ? "true" : "false");

            if (!String.IsNullOrEmpty(Tag))
            {
                return dict;
            }
            if (!String.IsNullOrEmpty(Prefix))
            {
                dict.Add("prefix", Prefix);
            }
            else if (PublicIds != null && PublicIds.Count > 0)
            {
                dict.Add("public_ids", PublicIds);
            }

            return dict;
        }
    }
}
