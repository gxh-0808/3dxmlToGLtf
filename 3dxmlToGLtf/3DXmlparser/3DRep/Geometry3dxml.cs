using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Vision.net3DXmlReader._3DRep
{
    public class Geometry3Dxml
    {
        public List<MeshGeometry3DXml> MeshGeometry3DXmls;

        public Geometry3Dxml()
        {
            MeshGeometry3DXmls = new List<MeshGeometry3DXml>();
        }

    }

    public class MeshGeometry3DXml
    {
        public List<double> Points { get; set; }

        public List<double> Normals { get; set; }

        public List<int> TriangleIndices { get; set; }

        public Color SurfaceColor { get; set; }

        public List<List<double>> Edges { get; set; }

        public MeshGeometry3DXml()
        {
            Points = new List<double>();
            Normals = new List<double>();
            TriangleIndices = new List<int>();
            SurfaceColor = new Color();
            Edges = new List<List<double>>();
        }
    }

}
