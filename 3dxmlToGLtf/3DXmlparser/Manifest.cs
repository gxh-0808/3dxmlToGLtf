using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Vision.net3DXmlReader.Manifest3dxml
{
    [XmlRoot("Manifest")]  
    public class Manifest
    {
        [XmlAttribute("noNamespaceSchemaLocation", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string NoNamespaceSchemaLocation { get; set; }

        [XmlElement("Root")]
        public string RootXmlStruPath { get; set; }

        [XmlElement("Date")]
        public string Date { get; set; }

        [XmlElement("WithAuthoringData")]
        public bool IsWithAuthoringData { get; set; }
    }
}
