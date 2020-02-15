using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Vision.net3DXmlReader
{
    public static class XMLHelper
    {
        public static T DeserializeXML<T>(string input)
            where T : class
        {
            if (string.IsNullOrEmpty(input)) return null;

            var serializer = new XmlSerializer(typeof(T));

            try
            {
                using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(input)))
                {
                    return serializer.Deserialize(stream) as T;
                }
            }
            catch (Exception ex)
            {
                //TODO: log...throw

                return null;
            }
        }
    }
}