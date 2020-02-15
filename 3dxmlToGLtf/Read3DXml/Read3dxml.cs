using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vision.net3DXmlReader;
using Vision.net3DXmlReader.Structure3dxml;

namespace Read3DXml
{
    public static class Read3dxml
    {
        //该方法是将Net3DXmlReader中的ReadAsync方法再次封装
        public static async Task<Model3Dxml> Read3Dxml(string fileName)
        {
            Net3DXmlReader net3DXmlReader = new Net3DXmlReader();
            Model3Dxml model3Dxml = await net3DXmlReader.ReadAsync(fileName);
            return model3Dxml;
            //ReadModel(model3Dxml);
        }
        //public void ReadModel(Model3Dxml model3Dxml)
        //{
        //    int root = model3Dxml.ProductStructure.Root;
        //    List<Assembly3D> assembly3DList = new List<Assembly3D>();
        //    assembly3DList.AddRange(model3Dxml.ProductStructure.Instance3DList);
        //    assembly3DList.AddRange(model3Dxml.ProductStructure.InstanceRepList);
        //    //assembly3DList.AddRange(model3Dxml.ProductStructure.Reference3DList);
        //    assembly3DList.AddRange(model3Dxml.ProductStructure.ReferenceRepList);
        //    Assembly3D rootAssembly3D = assembly3DList.Find(x => x.Id == root);
        //}
    }
}
