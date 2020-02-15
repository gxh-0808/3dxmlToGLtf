using Read3DXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using Read3DXml;
using Vision.net3DXmlReader.Structure3dxml;
using Vision.net3DXmlReader._3DRep;
using test.Processing;
using Arctron.Gltf;
using System.IO;

namespace test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            var openfiledlg = new OpenFileDialog
            {
                Multiselect = true,
                Title = @"打开",
                Filter = @"3DXml DXF  files (*.3dxml,*.dxf)|*.3dxml;*.dxf;"
                         + @"|DXF files (*.dxf)|*.dxf;"
                         + @"|HSF files (*.hsf)|*.hsf;"
                         + @"|HMF files (*.hmf)|*.hmf;"
                         + @"|SAMIN Block files (*.ebg2)|*.ebg2;"
            };

            if (openfiledlg.ShowDialog() == DialogResult.OK)
            {
                foreach (string fileName in openfiledlg.FileNames)
                {
                    if (fileName.EndsWith(".3dxml", true, CultureInfo.InvariantCulture))
                    {
                        //Read3Dxml(fileName);
                        //writetoglTF(fileName);
                        label1.Text = fileName;
                    }

                }
            }
        }


        //private async void Read3Dxml(string fileName)
        //{
        //    Model3Dxml model = await Read3dxml.Read3Dxml(fileName);
        //}


        //private async void writetoglTF(string fileName)
        //{
        //    Model3Dxml model = await Read3dxml.Read3Dxml(fileName);

            

        //    ToVertex modeldata = new ToVertex(model.ProductStructure.Instance3DList, model.ProductStructure.ReferenceRepList);

        //    modeldata.getprimitivelist();
        //    ThreedxmlTogltf changefile = new ThreedxmlTogltf();
        //    changefile.getfromprimitivedata(modeldata.allvertexindex, modeldata.allvertexpostion, model.Header.Title);
        //    GltfModel _model = changefile.writeToGLtf();
        //    var json = _model.ToJson(_model);

        //    //  文件保存路径
           
        //    var file = @"C:\Users\gxh-PC\Desktop\001.gltf";    // 保存的文件路径

        //    File.WriteAllText(file, json);

        //}

        private async void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog sd = new SaveFileDialog();
            sd.Filter = "3D Object (*.gltf)|*.gltf";
            if (sd.ShowDialog() == DialogResult.OK)
            {
                label2.Text = sd.FileName;
            }

            Model3Dxml model = await Read3dxml.Read3Dxml(label1.Text);
            textBox1.AppendText("已读取3dxml文件数据！  \r\n\n");

            ToVertex modeldata = new ToVertex(model.ProductStructure.Instance3DList, model.ProductStructure.ReferenceRepList);
            modeldata.getprimitivelist();

            ThreedxmlTogltf changefile = new ThreedxmlTogltf();
            changefile.getfromprimitivedata(modeldata.allvertexindex, modeldata.allvertexpostion, model.Header.Title);

            GltfModel _model = changefile.writeToGLtf();
            var json = _model.ToJson(_model);

            //  文件保存路径

            //var file = @"C:\Users\gxh-PC\Desktop\001.gltf";    // 保存的文件路径
            var file = label2.Text;
            File.WriteAllText(file, json);
            textBox1.AppendText("写入gltf文档成功！  \r\n\n");
        }
    }
}
