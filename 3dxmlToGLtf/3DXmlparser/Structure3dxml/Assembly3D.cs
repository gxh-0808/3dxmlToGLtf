using System.Xml.Serialization;

namespace Vision.net3DXmlReader.Structure3dxml
{
    [XmlInclude(typeof(Reference3DType))]
    [XmlInclude(typeof(Instance3DType))]
    [XmlInclude(typeof(ReferenceRepType))]
    [XmlInclude(typeof(InstanceRepType))]
    public class Assembly3D
    {
        [XmlAttribute("type", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string Type { get; set; }

        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }
    }
}
