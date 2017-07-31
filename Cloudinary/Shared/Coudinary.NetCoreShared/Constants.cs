using System;
using System.Collections.Generic;
using System.Text;

namespace Coudinary.NetCoreShared
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
    }
}
