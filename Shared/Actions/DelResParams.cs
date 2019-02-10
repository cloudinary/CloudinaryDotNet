using System;
using System.Collections.Generic;

namespace CloudinaryDotNet.Actions
{
    /// <summary>
    /// Parameters for deletion of resources.
    /// </summary>
    public class DelResParams : BaseParams
    {
        List<string> m_publicIds = new List<string>();
        string m_prefix;
        string m_tag;
        bool m_all;

        /// <summary>
        /// Instantiates the <see cref="DelResParams"/> object.
        /// </summary>
        public DelResParams()
        {
            Type = "upload";
        }

        /// <summary>
        /// Optional. The type of file to delete. Default: image.
        /// </summary>
        public ResourceType ResourceType { get; set; }

        /// <summary>
        /// Optional. The storage type: upload, private, authenticated, facebook, twitter, gplus, instagram_name,
        /// gravatar, youtube, hulu, vimeo, animoto, worldstarhiphop or dailymotion. Default: upload.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// If true, delete only the derived images of the matching resources.
        /// </summary>
        public bool KeepOriginal { get; set; }

        /// <summary>
        /// Whether to invalidate CDN cache copies of a previously uploaded image that shares the same public ID.
        /// Default: false.
        /// </summary>
        public bool Invalidate { get; set; }

        /// <summary>
        /// Continue deletion from the given cursor. Notice that it doesn't have a lot of meaning unless the 
        /// <see cref="KeepOriginal"/> flag is set to True.
        /// </summary>
        public String NextCursor { get; set; }

        /// <summary>
        /// Delete all resources with the given public IDs (array of up to 100 public_ids).
        /// </summary>
        public List<string> PublicIds
        {
            get { return m_publicIds; }
            set { m_publicIds = value; m_prefix = String.Empty; m_tag = String.Empty; m_all = false; }
        }

        /// <summary>
        /// Delete all resources, including derived resources, where the public ID starts with the given prefix
        /// (up to a maximum of 1000 original resources).
        /// </summary>
        public string Prefix
        {
            get { return m_prefix; }
            set { m_publicIds = null; m_tag = String.Empty; m_prefix = value; m_all = false; }
        }

        /// <summary>
        /// Delete all resources (and their derivatives) with the given tag name (up to a maximum of 1000 original
        /// resources).
        /// </summary>
        public string Tag
        {
            get { return m_tag; }
            set { m_publicIds = null; m_prefix = String.Empty; m_tag = value; m_all = false; }
        }

        /// <summary>
        /// Optional. Get or set whether to delete all resources (of the relevant resource type and type), including
        /// derived resources (up to a maximum of 1000 original resources). Default: false.
        /// </summary>
        public bool All
        {
            get { return m_all; }
            set
            {
                if (value)
                {
                    m_publicIds = null; m_prefix = String.Empty; m_tag = String.Empty; m_all = value;
                }
                else
                    m_all = value;
            }
        }

        /// <summary>
        /// Get or set a list of transformations. When provided, only derived resources matched by the transformations
        /// would be deleted.
        /// </summary>
        public List<Transformation> Transformations { get; set; }

        /// <summary>
        /// Validate object model.
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
        }

        /// <summary>
        /// Maps object model to dictionary of parameters in cloudinary notation.
        /// </summary>
        /// <returns>Sorted dictionary of parameters.</returns>
        public override SortedDictionary<string, object> ToParamsDictionary()
        {
            SortedDictionary<string, object> dict = base.ToParamsDictionary();

            AddParam(dict, "invalidate", Invalidate);
            AddParam(dict, "next_cursor", NextCursor);

            if (Transformations != null && Transformations.Count > 0)
            {
                AddParam(dict, "transformations", string.Join("|", Transformations));
            }

            AddParam(dict, "keep_original", KeepOriginal);

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
