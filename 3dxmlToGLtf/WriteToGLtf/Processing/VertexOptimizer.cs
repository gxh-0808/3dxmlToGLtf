using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test
{
    public class VertexOptimizer
    {
        public List<int> initialIndex;
        public List<Vertex> initialVertex;
        public List<int> newIndex=new List<int>();
        public List<Vertex> newVertex=new List<Vertex>();


        public VertexOptimizer(List<int> i,List<Vertex> v)
        {
            initialIndex = i;
            initialVertex=v;
        }

        public void simplifyByRemovalDuplication()
        {
            //List<Vertex> initialVertex = new List<Vertex>();
            Vertex eachvertex;

            //for(int i=0,j=0;i<initialPosition.Count;i=i+3,j=j+1)//将初始的位置列表中的数据，以三个为一组放入一个Vertex中，再将Vertex放入列表中，此时列表中应有重复的Vertex
            //{
            //    eachvertex = new Vertex(j, initialPosition[i], initialPosition[i + 1], initialPosition[i + 2]);
            //    initialVertex.Add(eachvertex);
            //}

            eachvertex = new Vertex(initialVertex[0].id, initialVertex[0].x, initialVertex[0].y, initialVertex[0].z);
            newVertex.Add(eachvertex);//第一个点可以直接放入

            //-----------------------------------------------------
            //将剩余点中重复的点剔除，构建一个新的存储列表
            int newid = 1;
            for(int i=1;i<initialVertex.Count;i++)
            {
                int k = 0;
                for(int j =0;j<newVertex.Count;j++)
                {
                    if(initialVertex[i].judgeEqual(newVertex[j]))
                    {
                        k = k + 1;
                        break;
                    }
                }

                if(k==0)
                {
                    eachvertex = new Vertex(newid, initialVertex[i].x, initialVertex[i].y, initialVertex[i].z);
                    
                    newVertex.Add(eachvertex);
                    newid = newid + 1;
                }
            }
            //--------------------------------------------------

            //---------------------------------------------
            //根据新构建的不含重复点的顶点列表去创建相应的索引列表
            for (int i = 0; i < initialIndex.Count; i++)
            {
                for(int j=0;j<newVertex.Count;j++)
                {
                    if(initialVertex[initialIndex[i]].judgeEqual(newVertex[j]))
                    {
                        newIndex.Add(newVertex[j].id);
                        break;
                    }                                                                 
                }
            }
            //---------------------------------------------------


        }
    }
}
