using System.Xml.Serialization;

namespace Vision.net3DXmlReader.Structure3dxml
{

    
    public class DefaultView
    {
        
        [XmlElement("Viewpoint")]
        public Viewpoint Viewpoint { get; set; }

    }


    public class ParallelViewpointType : Viewpoint
    {
        
    }


    public class PerspectiveViewpointType : Viewpoint
    {
        
    }

    [XmlInclude(typeof(ParallelViewpointType))]
    [XmlInclude(typeof(PerspectiveViewpointType))]
    //[XmlType(Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
    public class Viewpoint
    {
      
        [XmlAttribute("type", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string Type { get; set; }

        [XmlAttribute("visualizedHeight")]
        public string VisualizedHeight { get; set; }

        [XmlAttribute("targetDistance")]
        public string TargetDistance { get; set; }

        [XmlAttribute("nearPlaneDistance")]
        public string NearPlaneDistance { get; set; }

        [XmlAttribute("farPlaneDistance")]
        public string FarPlaneDistance { get; set; }

        [XmlElement("Position")]
        public string Position { get; set; }

        [XmlElement("Sight")]
        public string Sight { get; set; }

        [XmlElement("Right")]
        public string Right { get; set; }

        [XmlElement("Up")]
        public string Up { get; set; }
    }


    //public class Base { public string Field; }
    //public class Derived { public string AnotherField; }
    //public class Container { public Base MyField; }
    //Container obj = new Container(); obj.MyField = new Derived(); // legal assignment in the                              //.NET type system// ...XmlSerializer serializer = new XmlSerializer( typeof( Container ) );serializer.Serialize( writer, obj ); // Kaboom!
}
