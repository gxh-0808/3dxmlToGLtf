using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vision.net3DXmlReader.Structure3dxml;
using Vision.net3DXmlReader._3DRep;

namespace test
{
    public class ToVertex
    {
        public List<Instance3DType> instance3dlist;
        public List<ReferenceRepType> referencereplist;
        public Geometry3Dxml allgeometry = new Geometry3Dxml();
        public List<int> allvertexindex = new List<int>();
        public List<double> allvertexpostion = new List<double>();


        public ToVertex(List<Instance3DType> Inst, List<ReferenceRepType> refe)
        {
            this.instance3dlist = Inst;
            this.referencereplist = refe;
        }


        public void getallgeometry()
        {
            Geometry3Dxml geometryFromInstance3dlist = getFromInstance3dlist();
            Geometry3Dxml geometryFromReferenceRepList = getFromReferenceRepList();
           
            this.allgeometry.MeshGeometry3DXmls = geometryFromReferenceRepList.MeshGeometry3DXmls.Concat(geometryFromInstance3dlist.MeshGeometry3DXmls).ToList<MeshGeometry3DXml>();
           
        }


        public Geometry3Dxml getFromInstance3dlist()
        {
            Geometry3Dxml result = new Geometry3Dxml();

            foreach (Instance3DType eachinstance3d in instance3dlist)
            {
                foreach (MeshGeometry3DXml triangleMesh in eachinstance3d.MeshGeometry.MeshGeometry3DXmls)
                {
                    if (triangleMesh.Points.Count / 3 >= 3)//判断MeshGeometry3DXml中的点是否有三个，是否可以至少组成一个三角形，一堆三角形就可组成一个复杂形状的面
                    {
                        result.MeshGeometry3DXmls.Add(triangleMesh);
                    }

                }
            }


            return result;
        }

        //读取ReferenceRepList（相当于装配体）中所有零部件的面片信息，将他们存放在Geometry3Dxml中的MeshGeometry3DXmls里
        public Geometry3Dxml getFromReferenceRepList()
        {
            Geometry3Dxml result = new Geometry3Dxml();

            foreach (ReferenceRepType eachReferenceRep in referencereplist)
            {
                foreach (MeshGeometry3DXml triangleMesh in eachReferenceRep.MeshGeometry.MeshGeometry3DXmls)
                {
                    if (triangleMesh.Points.Count / 3 >= 3)//判断MeshGeometry3DXml中的点是否有三个，是否可以至少组成一个三角形，一堆三角形就可组成一个复杂形状的面
                    {
                        result.MeshGeometry3DXmls.Add(triangleMesh);
                    }

                }
            }
            return result;
        }


        public void getprimitivelist()//获得两个初始的list,其中包含点位置的list和包含点索引的list,需要注意的是这个列表中可能会包含重复的点！！
        {
            getallgeometry();
            int offset = 0;//顶点索引的偏移量，每遍历一个mesh后都会更新一次

            foreach(MeshGeometry3DXml e in this.allgeometry.MeshGeometry3DXmls)
            {
                foreach(int i in e.TriangleIndices)
                {
                    this.allvertexindex.Add(i + offset);
                }

                offset = offset + e.Points.Count / 3;

                foreach(double p in e.Points)
                {
                    this.allvertexpostion.Add(p);
                }
            }
        }

        public List<Vertex> archieveConversion()
        {
            getprimitivelist();
            List<Vertex> allVertex=new List<Vertex>();
            Vertex eachvertex;

            for (int i = 0, j = 0; i < allvertexpostion.Count; i = i + 3, j = j + 1)//将初始的位置列表中的数据，以三个为一组放入一个Vertex中，再将Vertex放入列表中，此时列表中应有重复的Vertex
            {
                eachvertex = new Vertex(j, allvertexpostion[i], allvertexpostion[i + 1], allvertexpostion[i + 2]);
                allVertex.Add(eachvertex);
            }

            return allVertex;

        }
      

    }
}
