using System.Xml.Serialization;
using Vision.net3DXmlReader._3DRep;

namespace Vision.net3DXmlReader.Structure3dxml
{ 
     public class Instance3DType : InstanceRepType
    {
        [XmlElement("IsAggregatedBy")]
        public int IsAggregatedBy { get; set; }

        [XmlElement("IsInstanceOf")]
        public string IsInstanceOf { get; set; }

        [XmlElement("RelativeMatrix")]

        public string RelativeMatrix { get; set; }

        //public string RelativeMatrix { get; set; }
        [XmlIgnore]
        public Geometry3Dxml MeshGeometry { get; set; }
    }
}
