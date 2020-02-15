using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arctron.Gltf;

namespace test.Processing
{
    public class ThreedxmlTogltf
    {
        public List<int> indexes=new List<int>();
        public List<float> positions=new List<float>();
        public string nameofscene;

        public void getfromprimitivedata(List<int> i,List<double> p,string n)
        {
            this.indexes = i;
            foreach(double e in p)
            {
                float f = (float)e;
                this.positions.Add(f);
            }
            this.nameofscene = n;
        }



        public GltfModel writeToGLtf()
        {
            GltfModel _model = new GltfModel();
            #region "asset"
            _model.Asset.Generator = "3dxmlToGLTF";
            _model.Asset.Version = "2.0";
            #endregion

            #region "scene" and "scenes"
            // 为 "scene" 添加 0
            _model.Scene = 0;
            // 为 "scenes" 添加 根节点 nodes
            Scene scene = new Scene();
            scene.Name = nameofscene;
            scene.Nodes.Add(0);
            _model.Scenes.Add(scene);
            #endregion

            #region "nodes"  有待添加
            Node node = new Node();
            node.Mesh = 0;
            _model.Nodes.Add(node);
            #endregion

            #region "meshes"
            Mesh mesh = new Mesh();
            Primitive primitive = new Primitive();
            var atts = new Dictionary<string, int>();
            atts.Add("POSITION", 1);
            //atts.Add("NORMAL", 2);
            primitive.Attributes = atts;
            primitive.Indices = 0;
            primitive.Material = 0;
            primitive.Mode = Mode.Triangles;
            mesh.Primitives.Add(primitive);
            
            _model.Meshes.Add(mesh);
            #endregion

            #region "materials"
            Material material = new Material();
            PbrMetallicRoughness pbrMetallicRoughness = new PbrMetallicRoughness();
            pbrMetallicRoughness.BaseColorFactor = new double[] { 0.1, 0.1, 0.1, 1 };
            pbrMetallicRoughness.MetallicFactor = 0.0;
            material.PbrMetallicRoughness = pbrMetallicRoughness;
            material.DoubleSided = true;
            material.Name = "color";
            material.AlphaMode = AlphaMode.OPAQUE;
            _model.Materials.Add(material);
            #endregion

            #region "buffers"           
            List<byte> bytes = new List<byte>();
            foreach (int e in this.indexes)
            {
                byte[] b = BitConverter.GetBytes(e);
                foreach (byte elementofb in b)
                {
                    bytes.Add(elementofb);
                }
            }
            //若索引为int,顶点位置为float，则不用去判断是否需要添加0字节以达到整除的目的，因为这种情况下必定整除
            foreach(float e in this.positions)
            {
                byte[] b = BitConverter.GetBytes(e);
                foreach (byte elementofb in b)
                {
                    bytes.Add(elementofb);
                }
            }

            byte[] byteflow = bytes.ToArray();

            Arctron.Gltf.Buffer buffer = new Arctron.Gltf.Buffer();
            buffer.ByteLength = byteflow.Length;
            buffer.Uri = "data:application/octet-stream;base64," + Convert.ToBase64String(byteflow); //byte数组转化为Base64
            _model.Buffers.Add(buffer);
            #endregion

            #region  "bufferViews"
            //索引段
            BufferView bufferView0 = new BufferView();
            bufferView0.Buffer = 0;
            bufferView0.ByteOffset = 0;
            bufferView0.ByteLength = indexes.Count*sizeof(int);
            bufferView0.Target = 34963;
            //数据段
            BufferView bufferView1 = new BufferView();
            bufferView1.Buffer = 0;
            bufferView1.ByteOffset = indexes.Count * sizeof(int);
            bufferView1.ByteLength = positions.Count * sizeof(float);         
            bufferView1.Target = 34962;
            //添加到gltf模型中
            _model.BufferViews.Add(bufferView0);
            _model.BufferViews.Add(bufferView1);
            #endregion

            #region "accessors"
            //索引段
            int index_num = indexes.Count;
            int vertex_num = positions.Count / 3;

            Accessor accessor0 = new Accessor();
            accessor0.BufferView = 0;
            accessor0.ByteOffset = 0;
            accessor0.ComponentType = 5125;
            accessor0.Count = index_num;
            accessor0.Type = AccessorType.SCALAR;
            double[] min = new double[1] { 0 };
            accessor0.Min = min;
            double[] max = new double[1] { vertex_num - 1 };
            accessor0.Max = max;
            //顶点位置
            Accessor accessor1 = new Accessor();
            accessor1.BufferView = 1;
            accessor1.ByteOffset = 0;
            accessor1.ComponentType = 5126;
            accessor1.Count = vertex_num;
            accessor1.Type = AccessorType.VEC3;
            accessor1.Min = accessor1.GetMinVec(positions);
            accessor1.Max = accessor1.GetMaxVec(positions);
            _model.Accessors.Add(accessor0);
            _model.Accessors.Add(accessor1);
            #endregion

            #region "textures"

            #endregion

            #region "images"

            #endregion

            #region "samplers"

            #endregion

            _model.Clean();




            return _model;


        }
    }
}
