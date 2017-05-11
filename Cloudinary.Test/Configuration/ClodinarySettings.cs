using System;
using System.Configuration;

namespace Cloudinary.Test.Configuration
{
    public class ClodinarySettings : ConfigurationSection
    {
        private static ClodinarySettings settings = ConfigurationManager.GetSection("ClodinarySettings") as ClodinarySettings;

        public static ClodinarySettings Settings
        {
            get
            {
                return settings;
            }
        }

        [ConfigurationProperty("CloudName", DefaultValue = "", IsRequired = false)]
        public string CloudName
        {
            get { return (string)this["CloudName"]; }
            set { this["CloudName"] = value; }
        }

        [ConfigurationProperty("ApiKey", DefaultValue = "", IsRequired = false)]
        public string ApiKey
        {
            get { return (string)this["ApiKey"]; }
            set { this["ApiKey"] = value; }
        }

        [ConfigurationProperty("ApiSecret", DefaultValue = "", IsRequired = false)]
        public string ApiSecret
        {
            get { return (string)this["ApiSecret"]; }
            set { this["ApiSecret"] = value; }
        }

        [ConfigurationProperty("ApiBaseAddress", DefaultValue = "", IsRequired = false)]
        public string ApiBaseAddress
        {
            get { return (string)this["ApiBaseAddress"]; }
            set { this["ApiBaseAddress"] = value; }
        }
    }
}
