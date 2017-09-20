using System;
using System.IO;
using System.Text;
using System.Xml;

namespace Cloudinary.Test
{
    public static class SettingsReader
    {
        private const string SETTINGS_XPATH = "/configuration/CloudinarySettings";

        public static string ReadSetting(string settingName)
        {
            string configuration_file_path = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
            if (string.IsNullOrWhiteSpace(configuration_file_path))
                throw new Exception("Configurtion file does not exists");

            string xmlString = File.ReadAllText(configuration_file_path);
            string _byteOrderMarkUtf8 = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());
            byte[] bytes = Encoding.Default.GetBytes(xmlString);
            xmlString = Encoding.UTF8.GetString(bytes);

            XmlDocument doc = new XmlDocument();
            try
            {
                doc.LoadXml(xmlString);
            }
            catch (Exception exc)
            {
                throw new Exception(string.Concat("Configurtion file read error:", exc.Message));
            }

            XmlNode node = doc.DocumentElement.SelectSingleNode(SETTINGS_XPATH);
            if (node == null)
                throw new Exception(string.Concat("Cannot find setting:", settingName));

            XmlAttribute attr = node.Attributes[settingName];

            if(attr == null)
                throw new Exception(string.Concat("Cannot find setting:", settingName));

            return attr.InnerText;
        }
    }
}