using System.Windows.Media.Media3D;
using System.Xml.Serialization;
using Vision.net3DXmlReader._3DRep;

namespace Vision.net3DXmlReader.Structure3dxml
{
    public class ReferenceRepType : Assembly3D
    {
        [XmlAttribute("format")]
        public string Format { get; set; }

        [XmlAttribute("version")]
        public string Version { get; set; }

        [XmlAttribute("associatedFile")]
        public string AssociatedFile { get; set; }


        [XmlElement("PLM_ExternalID")]
        public string PlmExternalId { get; set; }

        [XmlElement("V_discipline")]
        public string VDiscipline { get; set; }

        //[XmlElement("V_usage")]
        //public string VUsage { get; set; }

        [XmlElement("V_nature")]
        public string VNature { get; set; }

        [XmlIgnore]
        public Geometry3Dxml MeshGeometry { get; set; }

        //public ReferenceRepType()
        //{
        //    MeshGeometry = new Geometry3Dxml();
        //}
    }
}
