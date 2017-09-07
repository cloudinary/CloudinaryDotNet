using CloudinaryShared.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;


namespace Coudinary.NetCoreShared
{
    public static  class CloudinaryConfiguration
    {
        public static string CloudName = string.Empty;
        public static string ApiKey = string.Empty;
        public static string ApiSecret = string.Empty;
        public static AuthTokenBase AuthToken = null;
    }
}
