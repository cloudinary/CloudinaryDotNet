using CloudinaryDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Coudinary.NetCoreShared
{
    public class UrlShared
    {
        protected const string CL_BLANK = "data:image/gif;base64,R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7";
        protected static readonly string[] DEFAULT_VIDEO_SOURCE_TYPES = { "webm", "mp4", "ogv" };
        protected static readonly Regex VIDEO_EXTENSION_RE = new Regex("\\.(" + String.Join("|", DEFAULT_VIDEO_SOURCE_TYPES) + ")$", RegexOptions.Compiled);

        protected ISignProvider m_signProvider;

        protected string m_cloudName;
        protected string m_cloudinaryAddr = Api.ADDR_RES;
        protected string m_apiVersion;

        protected bool m_shorten;
        protected bool m_secure;
        protected bool m_usePrivateCdn;
        protected bool m_signed;
        protected bool m_useRootPath;
        protected string m_suffix;
        protected string m_privateCdn;
        protected string m_version;
        protected string m_cName;
        protected string m_source;
        protected string m_fallbackContent;
        protected bool m_useSubDomain;
        protected Dictionary<string, Transformation> m_sourceTransforms;
        protected List<string> m_customParts = new List<string>();
        protected Transformation m_posterTransformation;
        protected string m_posterSource;
        protected Url m_posterUrl;

        protected string[] m_sourceTypes;

        protected string m_action = String.Empty;
        protected string m_resourceType = String.Empty;

        protected Transformation m_transformation;

        public UrlShared(string cloudName)
        {
            m_cloudName = cloudName;
        }

        public UrlShared(string cloudName, ISignProvider signProvider)
            : this(cloudName)
        {
            m_signProvider = signProvider;
        }

        public string FormatValue { get; set; }

        public Transformation Transformation
        {
            get
            {
                if (m_transformation == null) m_transformation = new Transformation();
                return m_transformation;
            }
        }

        public static class Crc32
        {
            static uint[] table;

            public static uint ComputeChecksum(byte[] bytes)
            {
                uint crc = 0xffffffff;
                for (int i = 0; i < bytes.Length; ++i)
                {
                    byte index = (byte)(((crc) & 0xff) ^ bytes[i]);
                    crc = (uint)((crc >> 8) ^ table[index]);
                }
                return ~crc;
            }

            public static byte[] ComputeChecksumBytes(byte[] bytes)
            {
                return BitConverter.GetBytes(ComputeChecksum(bytes));
            }

            static Crc32()
            {
                uint poly = 0xedb88320;
                table = new uint[256];
                uint temp = 0;
                for (uint i = 0; i < table.Length; ++i)
                {
                    temp = i;
                    for (int j = 8; j > 0; --j)
                    {
                        if ((temp & 1) == 1)
                        {
                            temp = (uint)((temp >> 1) ^ poly);
                        }
                        else
                        {
                            temp >>= 1;
                        }
                    }
                    table[i] = temp;
                }
            }
        }

        protected class CSource
        {
            public CSource(string source)
            {
                SourceToSign = Source = source;
            }

            public static CSource operator +(CSource src, string value)
            {
                src.Source += value;
                src.SourceToSign += value;

                return src;
            }

            public string Source;
            public string SourceToSign;
        }

    }
}
