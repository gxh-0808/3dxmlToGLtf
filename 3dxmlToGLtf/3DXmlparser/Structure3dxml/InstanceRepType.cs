using System.Xml.Serialization;

namespace Vision.net3DXmlReader.Structure3dxml
{
    [XmlInclude(typeof(Instance3DType))]
    public class InstanceRepType : Assembly3D
    {

        [XmlElement("IsAggregatedBy")]
        public int IsAggregatedBy { get; set; }

        [XmlElement("IsInstanceOf")]
        public string IsInstanceOf { get; set; }
    }
}
