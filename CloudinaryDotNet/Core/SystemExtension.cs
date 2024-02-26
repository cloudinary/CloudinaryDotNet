namespace CloudinaryDotNet.Core
{
    using System.Reflection;
    using Newtonsoft.Json;

    /// <summary>
    /// SystemExtension.
    /// </summary>
    public static class SystemExtension
    {
        /// <summary>
        /// Clones the object.
        /// </summary>
        /// <param name="source">The source to clone.</param>
        /// <typeparam name="T">Object type.</typeparam>
        /// <returns>New instance of the cloned object.</returns>
        public static T Clone<T>(this T source)
        {
            var serialized = JsonConvert.SerializeObject(source);
            return JsonConvert.DeserializeObject<T>(serialized);
        }

        /// <summary>
        /// Copies the writable properties from the source object to the destination object.
        /// </summary>
        /// <param name="source">The source object whose properties will be copied.</param>
        /// <param name="destination">The destination object where the properties will be copied.</param>
        public static void CopyPropertiesTo(this object source, object destination)
        {
            var properties = typeof(FileDescription).GetRuntimeProperties();

            foreach (var property in properties)
            {
                if (!property.CanWrite)
                {
                    continue;
                }

                var value = property.GetValue(source);

                property.SetValue(destination, value);
            }
        }
    }
}
