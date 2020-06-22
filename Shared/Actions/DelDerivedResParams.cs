namespace CloudinaryDotNet.Actions
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Parameters to delete derived resources.
    /// </summary>
    public class DelDerivedResParams : BaseParams
    {
        private string m_publicId = string.Empty;
        private List<Transformation> m_tranformations = new List<Transformation>();

        /// <summary>
        /// Initializes a new instance of the <see cref="DelDerivedResParams"/> class.
        /// </summary>
        public DelDerivedResParams()
        {
            DerivedResources = new List<string>();
        }

        /// <summary>
        /// Gets or sets whether to delete all derived resources with the given IDs.
        /// </summary>
        public List<string> DerivedResources { get; set; }

        /// <summary>
        /// Gets or sets whether to delete all derived resources with the given transformation.
        /// </summary>
        public List<Transformation> Transformations
        {
            get { return m_tranformations; }
            set { m_tranformations = value; }
        }

        /// <summary>
        /// Gets or sets whether to delete all derived resources with the given public id.
        /// </summary>
        public string PublicId
        {
            get { return m_publicId; }
            set { m_publicId = value; }
        }

        /// <summary>
        /// Validate object model.
        /// </summary>
        public override void Check()
        {
            if ((DerivedResources == null || DerivedResources.Count == 0) && (m_tranformations == null || m_tranformations.Count == 0))
            {
                throw new ArgumentException("At least one derived resource or transformation must be specified!");
            }

            if (m_tranformations != null && (m_tranformations.Count > 0 && string.IsNullOrWhiteSpace(m_publicId)))
            {
                throw new ArgumentException("PublicId must be specified!");
            }
        }

        /// <summary>
        /// Maps object model to dictionary of parameters in cloudinary notation.
        /// </summary>
        /// <returns>Sorted dictionary of parameters.</returns>
        public override SortedDictionary<string, object> ToParamsDictionary()
        {
            SortedDictionary<string, object> dict = base.ToParamsDictionary();

            if (DerivedResources != null && DerivedResources.Count > 0)
            {
                dict.Add("derived_resource_ids", DerivedResources);
            }

            if (m_tranformations != null && m_tranformations.Count > 0)
            {
                List<string> transformations = new List<string>();
                foreach (Transformation t in m_tranformations)
                {
                    transformations.Add(t.Generate());
                }

                dict.Add("transformations", transformations);
            }

            return dict;
        }
    }
}
