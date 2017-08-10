using System;
using System.Collections.Generic;
using System.Linq;

namespace CloudinaryDotNet.Actions
{
    public class DelDerivedresByTransParam : BaseParams
    {

        List<Transformation> m_transformations;
        string m_public_id;
        string m_type = "upload";

        public DelDerivedresByTransParam()
        {
            m_transformations = new List<Transformation>();
        }

        public ResourceType ResourceType { get; set; }

        public string Type
        {
            get { return m_type; }
            set { m_type = value; }
        }

        /// <summary>
        /// Delete derivved resources by transformation list.
        /// </summary>
        public List<Transformation> Transformations
        {
            get { return m_transformations; }
            set { m_transformations = value; }
        }

        /// <summary>
        /// Delete derivved resources by public id.
        /// </summary>
        public string PublicId
        {
            get { return m_public_id; }
            set { m_public_id = value; }
        }

        /// <summary>
        /// Validate object model
        /// </summary>
        public override void Check()
        {
            if (string.IsNullOrWhiteSpace(m_public_id))
                throw new ArgumentException("PublicId must be specified!");

            if (m_transformations == null)
                throw new ArgumentException("Transformations cannot be null!");

            if (m_transformations.Count == 0)
                throw new ArgumentException("At least one transformation should be present!");
        }

        /// <summary>
        /// Maps object model to dictionary of parameters in cloudinary notation
        /// </summary>
        /// <returns>Sorted dictionary of parameters</returns>
        public override SortedDictionary<string, object> ToParamsDictionary()
        {
            SortedDictionary<string, object> dict = base.ToParamsDictionary();
            AddParam(dict, "keep_original", true);
            dict.Add("public_ids", new List<string> { m_public_id });
            dict.Add("transformations", string.Join(",", m_transformations.Select(t => t.ToString()).ToList()));

            return dict;
        }
    }
}
