using System.Collections.Generic;
using System.Xml.Serialization;

namespace Vision.net3DXmlReader.Structure3dxml
{
    public class ProductStructure
    {
        [XmlAttribute("root")]
        public int Root { get; set; }

        [XmlElement(ElementName = "Reference3D")]
        [XmlIgnore]
        public List<Reference3DType> Reference3DList { get; set; }

        [XmlElement(ElementName = "Instance3D")]
        public List<Instance3DType> Instance3DList { get; set; }

        [XmlElement(ElementName = "ReferenceRep")]
        public List<ReferenceRepType> ReferenceRepList { get; set; }

        [XmlElement(ElementName = "InstanceRep")]
        public List<InstanceRepType> InstanceRepList { get; set; }

        //[XmlElement()]
        //public List<Assembly3D> Assembly3D { get; set; }
    }
}
