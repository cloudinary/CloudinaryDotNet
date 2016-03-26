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
        protected string resourceType = null;
        protected string type = null;
        protected string publicId = null;
        protected string format = null;

        public T ResourceType(string resourceType)
        {
            this.resourceType = resourceType;
            return (T)this;
        }

        public T Type(string type)
        {
            this.type = type;
            return (T)this;
        }

        public T PublicId(string publicId)
        {
            this.publicId = publicId.Replace('/', ':');
            return (T)this;
        }

        public T Format(string format)
        {
            this.format = format;
            return (T)this;
        }

        public override string ToString()
        {
            List<string> components = new List<string>();
            if (!string.IsNullOrEmpty(resourceType) && !resourceType.Equals("image"))
            {
                components.Add(resourceType);
            }

            if (!string.IsNullOrEmpty(type) && !type.Equals("upload"))
            {
                components.Add(type);
            }

            if (string.IsNullOrEmpty(publicId))
            {
                throw new ArgumentException("Must supply publicId");
            }

            components.Add(FormattedPublicId());

            return string.Join(":", components);
        }

        protected string FormattedPublicId()
        {
            var transientPublicId = publicId;

            if (!string.IsNullOrEmpty(format))
            {
                transientPublicId = string.Format("{0}.{1}", transientPublicId, format);
            }

            return transientPublicId;
        }
    }
}
