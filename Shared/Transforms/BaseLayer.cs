using System;
using System.Collections.Generic;

namespace CloudinaryDotNet
{
    /// <summary>
    /// Base abstract non-generic class for creating Layers.
    /// </summary>
    public abstract class BaseLayer : Core.ICloneable
    {
        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public abstract object Clone();
    }

    /// <summary>
    /// Base abstract generic class for creating Layers.
    /// </summary>
    public abstract class BaseLayer<T> : BaseLayer where T : BaseLayer<T>
    {
        /// <summary>
        /// The type of the resource.
        /// </summary>
        protected string m_resourceType;

        /// <summary>
        /// The specific type of the asset. Valid values: upload, private and authenticated. Default: upload.
        /// </summary>
        protected string m_type;

        /// <summary>
        /// The identifier of the uploaded asset. 
        /// </summary>
        protected string m_publicId;

        /// <summary>
        /// The resource format.
        /// </summary>
        protected string m_format;

        /// <summary>
        /// Sets the type of resource. Valid values: image, raw, and video.
        /// </summary>
        /// <returns>The instance of Layer object with set parameter.</returns>
        public T ResourceType(string resourceType)
        {
            m_resourceType = resourceType;
            return (T)this;
        }

        /// <summary>
        /// Sets the specific type of asset. Valid values: upload, private and authenticated. Default: upload.
        /// </summary>
        /// <returns>The instance of Layer object with set parameter.</returns>
        public T Type(string type)
        {
            this.m_type = type;
            return (T)this;
        }

        /// <summary>
        /// Sets the public ID of previously uploaded PNG image to add as overlay.
        /// </summary>
        /// <param name="publicId">Public ID.</param>
        /// <returns>The instance of Layer object with set parameter.</returns>
        public T PublicId(string publicId)
        {
            this.m_publicId = publicId.Replace('/', ':');
            return (T)this;
        }

        /// <summary>
        /// Sets a format of asset.
        /// </summary>
        /// <param name="format">Asset format.</param>
        /// <returns>The instance of Layer object with set parameter.</returns>
        public T Format(string format)
        {
            this.m_format = format;
            return (T)this;
        }

        /// <summary>
        /// Gets an additional parameters for the layer.
        /// </summary>
        public virtual string AdditionalParams()
        {
            return string.Empty;
        }

        /// <summary>
        /// Gets this layer represented as string.
        /// </summary>
        /// <returns>The Layer represented as string.</returns>
        public override string ToString()
        {
            List<string> components = new List<string>();
            if (!string.IsNullOrEmpty(m_resourceType) && !m_resourceType.Equals("image"))
            {
                components.Add(m_resourceType);
            }

            if (!string.IsNullOrEmpty(m_type) && !m_type.Equals("upload"))
            {
                components.Add(m_type);
            }

            string additionalParams = this.AdditionalParams();

            if (!string.IsNullOrEmpty(additionalParams))
            {
                components.Add(additionalParams);
            }

            if (!string.IsNullOrEmpty(m_publicId))
            {
                components.Add(FormattedPublicId());
            }

            return string.Join(":", components.ToArray());
        }

        private string FormattedPublicId()
        {
            var transientPublicId = m_publicId;

            if (!string.IsNullOrEmpty(m_format))
            {
                transientPublicId = string.Format("{0}.{1}", transientPublicId, m_format);
            }

            return transientPublicId;
        }

        #region ICloneable

        /// <summary>
        /// Creates a shallow copy of the current object.
        /// </summary>
        /// <returns>A new instance of the current object.</returns>
        public override object Clone() => MemberwiseClone();

        #endregion
    }
}
