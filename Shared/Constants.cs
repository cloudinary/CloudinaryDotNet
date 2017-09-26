using System;
using System.Collections.Generic;
using System.Text;

namespace CloudinaryDotNet
{
    static class Constants
    {
        //API url patterns
        public const string RESOURCES_API_URL = "resources";
        public const string TAG_API_URL = "tag";

        //Api URL action names
        public const string UPDATE_ACESS_MODE = "update_access_mode";
        public const string TAGS_MANGMENT = "tags";
        public const string CONTEXT_MANAGMENT = "context";

        //Param names
        public const string TAG_PARAM_NAME = "tag";
        public const string CONTEXT_PARAM_NAME = "context";
        public const string PREFIX_PARAM_NAME = "prefix";
        public const string PUBLIC_IDS = "public_ids";
        public const string COMMAND = "command";

        //Resource types
        public const string RESOURCE_TYPE_FETCH = "fetch";
        public const string RESOURCE_TYPE_IMAGE = "image";
        public const string RESOURCE_TYPE_VIDEO = "video";

        //Action names
        public const string ACTION_NAME_UPLOAD = "upload";
        public const string ACTION_NAME_FETCH = "fetch";

        //URL constants
        public const string CF_SHARED_CDN = "d3jpl91pxevbkh.cloudfront.net";
        public const string OLD_AKAMAI_SHARED_CDN = "cloudinary-a.akamaihd.net";
        public const string AKAMAI_SHARED_CDN = "res.cloudinary.com";
        public const string SHARED_CDN = AKAMAI_SHARED_CDN;
    }
}
