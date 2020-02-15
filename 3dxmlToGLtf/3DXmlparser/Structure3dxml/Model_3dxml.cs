using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Vision.net3DXmlReader.Structure3dxml
{ 

    [XmlRoot("Model_3dxml",Namespace = "http://www.3ds.com/xsd/3DXML")]
    [Serializable]
    public class Model3Dxml
    {
        [XmlElement("Header")]
        public Header Header { get; set; }

        [XmlElement("ProductStructure")]
        //[XmlIgnore]
        public ProductStructure ProductStructure { get; set; }

        [XmlElement("DefaultView")]
        public DefaultView DefaultView { get; set; }
    }
    [XmlRoot("Model_3dxml", Namespace = "http://www.3ds.com/xsd/3DXML")]
    //[Serializable]
    public class ModelBy3Dxml
    {
        [XmlElement("Header")]
        public Header Header { get; set; }

        [XmlElement("ProductStructure")]
        public ProductByStructure ProductStructure { get; set; }
    }
    public class ProductByStructure
    {
        [XmlAttribute("root")]
        public int Root { get; set; }

        [XmlElement(ElementName = "Reference3D")]
        public List<Reference3DType> Reference3DList { get; set; }

        [XmlElement(ElementName = "Instance3D")]
        public List<InstanceBy3DType> Instance3DList { get; set; }
    }

    public class InstanceBy3DType
    {
        [XmlElement("IsAggregatedBy")]
        public int IsAggregatedBy { get; set; }

        [XmlElement("IsInstanceOf")]
        public string IsInstanceOf { get; set; }
        [XmlElement("RelativeMatrix")]
        public string RelativeMatrix { get; set; }
        //[XmlElement("RelativeMatrix")]
        //public int RelativeMatrix { get; set; }
    }
}
