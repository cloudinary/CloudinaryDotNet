using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;

namespace Coudinary.NetCoreShared
{
    public class FileDescription : FileDescriptionBase
    {
        /// <summary>
        /// Constructor to upload file from stream
        /// </summary>
        /// <param name="name">Resource name</param>
        /// <param name="stream">Stream to read from (will be disposed with this object)</param>
        public FileDescription(string name, Stream stream) : base(name, stream)
        { }

        /// <summary>
        /// Constructor to upload file by path
        /// </summary>
        /// <param name="filePath">Either URL (http/https/s3/data) or local path to file</param>
        public FileDescription(string filePath) : base(filePath)
        { }

        protected override Stream GetStream(string url)
        {
            HttpRequestMessage msg = new HttpRequestMessage();
            msg.RequestUri = new Uri(url);
            var memStream = new MemoryStream();
            msg.Content.ReadAsStreamAsync().Result.CopyTo(memStream);
            memStream.Position = 0;

            return memStream;
        }
    }
}
