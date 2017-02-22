using System;
using System.Collections.Generic;

namespace CloudinaryDotNet.Actions
{
    public class DelResParams : BaseParams
    {
        List<string> m_publicIds = new List<string>();
        string m_prefix;
        string m_tag;
        bool m_all;
        SpecialImageType specialType = SpecialImageType.None;

        public DelResParams()
        {
            Type = "upload";
        }

        public ResourceType ResourceType { get; set; }

        public string Type { get; set; }

        public SpecialImageType SpecialType {
            get
            {
                return specialType; 
            }
            set
            {
                specialType = value;
            }
        }

        /// <summary>
        /// If true, delete only the derived images of the matching resources.
        /// </summary>
        public bool KeepOriginal { get; set; }

        /// <summary>
        /// Whether to invalidate CDN cache copies of a previously uploaded image that shares the same public ID. Default: false.
        /// </summary>
        public bool Invalidate { get; set; }

        /// <summary>
        /// Continue deletion from the given cursor. Notice that it doesn't have a lot of meaning unless the <see cref="KeepOriginal"/> flag is set to True.
        /// </summary>
        public String NextCursor { get; set; }

        /// <summary>
        /// Delete all resources with the given public IDs
        /// </summary>
        public List<string> PublicIds
        {
            get { return m_publicIds; }
            set { m_publicIds = value; m_prefix = String.Empty; m_tag = String.Empty; m_all = false; }
        }

        /// <summary>
        /// Delete all resources that their public ID starts with the given prefix.
        /// </summary>
        public string Prefix
        {
            get { return m_prefix; }
            set { m_publicIds = null; m_tag = String.Empty; m_prefix = value; m_all = false; }
        }

        public string Tag
        {
            get { return m_tag; }
            set { m_publicIds = null; m_prefix = String.Empty; m_tag = value; m_all = false; }
        }

        /// <summary>
        /// Delete all resources. Optional (default: false). 
        /// </summary>
        public bool All
        {
            get { return m_all; }
            set { m_publicIds = null; m_prefix = String.Empty; m_tag = String.Empty; m_all = true; }
        }

        /// <summary>
        /// Validate object model
        /// </summary>
        public override void Check()
        {
            if ((PublicIds == null || PublicIds.Count == 0) &&
                String.IsNullOrEmpty(Prefix) &&
                String.IsNullOrEmpty(Tag) &&
                !All)
            {
                throw new ArgumentException("Either PublicIds or Prefix or Tag must be specified!");
            }

            if (!String.IsNullOrEmpty(Tag) && !String.IsNullOrEmpty(Type))
                throw new ArgumentException("Type of resource cannot specified when tag is given!");
        }

        /// <summary>
        /// Maps object model to dictionary of parameters in cloudinary notation
        /// </summary>
        /// <returns>Sorted dictionary of parameters</returns>
        public override SortedDictionary<string, object> ToParamsDictionary()
        {
            SortedDictionary<string, object> dict = base.ToParamsDictionary();

            AddParam(dict, "keep_original", KeepOriginal);
            AddParam(dict, "invalidate", Invalidate);
            AddParam(dict, "next_cursor", NextCursor);

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
            if (m_all)
            {
                AddParam(dict, "all", true);
            }

            return dict;
        }
    }
}
