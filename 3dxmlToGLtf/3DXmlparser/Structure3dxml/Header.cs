using System.Xml.Serialization;

namespace Vision.net3DXmlReader.Structure3dxml
{
    public class Header
    {
        [XmlElement("SchemaVersion")]
        public string SchemaVersion { get; set; }

        [XmlElement("Title")]
        public string Title { get; set; }

        [XmlElement("Author")]
        public string Author { get; set; }

        [XmlElement("Generator")]
        public string Generator { get; set; }

        [XmlElement("Created")]
        public string Created { get; set; }
    }
}
