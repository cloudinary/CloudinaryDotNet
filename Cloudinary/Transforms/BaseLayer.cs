using System;
using System.Collections.Generic;

namespace CloudinaryDotNet
{
    /// <summary>
    /// Base abstract non-generic class for creating Layers.
    /// </summary>
    public abstract class BaseLayer { }

    /// <summary>
    /// Base abstract generic class for creating Layers.
    /// </summary>
    public abstract class BaseLayer<T> : BaseLayer where T : BaseLayer<T>
    {
        protected string m_resourceType;
        protected string m_type;
        protected string m_publicId;
        protected string m_format;

        public T ResourceType(string resourceType)
        {
            m_resourceType = resourceType;
            return (T)this;
        }

        public T Type(string type)
        {
            this.m_type = type;
            return (T)this;
        }

        public T PublicId(string publicId)
        {
            this.m_publicId = publicId.Replace('/', ':');
            return (T)this;
        }

        public T Format(string format)
        {
            this.m_format = format;
            return (T)this;
        }

        public virtual string AdditionalParams()
        {
            return string.Empty;
        }

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
    }
}
