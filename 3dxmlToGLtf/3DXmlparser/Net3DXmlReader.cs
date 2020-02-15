using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Vision.net3DXmlReader.Manifest3dxml;
using Vision.net3DXmlReader.Structure3dxml;
using Vision.net3DXmlReader._3DRep;


namespace Vision.net3DXmlReader
{
   public class Net3DXmlReader
   {
       public string _cacheDirPath;
       public Manifest _manifest;
       public Model3Dxml _model3Dxml;

       //该方法貌似没用到
        public Model3Dxml Read(string path)
       {
           try
           {
               _cacheDirPath = ReadPackage(path);
               
               _manifest = ReadManifestXml(_cacheDirPath + "\\Manifest.xml");
               _model3Dxml = Read3DXmlStructure(Path.Combine(_cacheDirPath, _manifest.RootXmlStruPath));
               foreach (ReferenceRepType referenceRepType in _model3Dxml.ProductStructure.ReferenceRepList)
               {
                   string model3DRepXmlFile = Path.Combine(_cacheDirPath, referenceRepType.AssociatedFile.Split(':')[2]);
                   referenceRepType.MeshGeometry = Read3Drep(model3DRepXmlFile);
               }
               DeleteDirectory(_cacheDirPath);
               return _model3Dxml;
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public async Task<Model3Dxml> ReadAsync(string path)
       {
           try
           { 
               var result = await Task.Run(async () =>
               {
                   _cacheDirPath = ReadPackage(path);
                   _manifest = ReadManifestXml(_cacheDirPath + "Manifest.xml");//获得Manifest.xml中根节点文件的信息
                   string filePath = Path.Combine(_cacheDirPath, _manifest.RootXmlStruPath);//找到根节点文件的绝对路径
                   //string filePath = Path.Combine(_cacheDirPath+ "AssembleFile.3dxml");

                   _model3Dxml = Read3DXmlStructure(filePath);
              
                   List<Task> tasks = new List<Task>();
                   //_model3Dxml.ProductStructure.Instance3DList.ForEach((referenceRepType) =>
                   //{
                   //    //urn: 3DXML: Reference: ext: AB01C--1FA - PLATE1.3dxml#1
                   //    string fileName = referenceRepType.IsInstanceOf.Split(':')[4];
                   //    string file = fileName.Split('#')[0];
                   //    var model3DRepXmlFile = Path.Combine(_cacheDirPath, referenceRepType.IsInstanceOf.Split(':')[2]);
                   //    var stas = Read3DrepAsync2(file);
                   //});

                   //instance3DType是ProductStructure中，以待解决
                   //如果ProductStructure中包含的是<Instance3D>，以下代码可以运行并将数据放入Instance3DList中
                   _model3Dxml.ProductStructure.Instance3DList.ForEach((instance3DType) =>
                   {
                       var task = Task.Run(async () =>
                       {
                           string fileName = instance3DType.IsInstanceOf.Split(':')[4].Split('#')[0];
                           string file = Path.Combine(_cacheDirPath, fileName);
                           instance3DType.MeshGeometry = await Read3DrepAsync(file);
                       });
                       tasks.Add(task);
                   });
                   await Task.WhenAll(tasks);


                   //referenceRepType是ProductStructure中的<ReferenceRep>  （xsi:type="ReferenceRepType"）
                   //如果ProductStructure中包含的是<ReferenceRep>，以下代码可以运行并将数据放入ReferenceRepList中
                   _model3Dxml.ProductStructure.ReferenceRepList.ForEach((referenceRepType) =>
                   {
                       var task = Task.Run(async () =>
                       {
                           //获得<ReferenceRep>中的associatedFile所指向的文件，该文件可能包括一个零件的几何数据信息
                           var model3DRepXmlFile = Path.Combine(_cacheDirPath, referenceRepType.AssociatedFile.Split(':')[2]);
                           referenceRepType.MeshGeometry = await Read3DrepAsync(model3DRepXmlFile);
                       });
                       tasks.Add(task);
                   });
                   await Task.WhenAll(tasks);

                   DeleteDirectory(_cacheDirPath);
                   return _model3Dxml;
               });
               return result;
           }
           catch (Exception ex)
           {               
               throw ex;
           }
       }

        //获取压缩包的路径？？相当于对3dxml的解压，为之后获得其中的Manifest.xml作铺垫？？
        private string ReadPackage(string path)
       {
           //Create temp archive for store
           //string folderFullPath = Path.GetFullPath(path);
           string archivePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tmp\\");

           if (System.IO.Directory.Exists(archivePath))
           {
               DeleteDirectory(archivePath);
           }

           System.IO.Directory.CreateDirectory(archivePath);

           using (ZipArchive zipArchive = ZipFile.Open(path, ZipArchiveMode.Read))
           {
               foreach (ZipArchiveEntry entry in zipArchive.Entries)
               {
                   try
                   {
                       entry.ExtractToFile(archivePath + entry.Name);
                   }
                   catch (Exception ex)
                   {
                       Console.WriteLine(ex.Message);
                       //throw;
                   }
               }
           }
           return archivePath;
       }

       /// <summary>
       /// Depth-first recursive delete, with handling for descendant 
       /// directories open in Windows Explorer.
       /// </summary>
       private void DeleteDirectory(string path)
       {
           foreach (string directory in System.IO.Directory.GetDirectories(path))
           {
               DeleteDirectory(directory);
           }

           try
           {
               System.IO.Directory.Delete(path, true);
           }
           catch (IOException)
           {
               System.IO.Directory.Delete(path, true);
           }
           catch (UnauthorizedAccessException)
           {
               System.IO.Directory.Delete(path, true);
           }
       }

       /// <summary>
       /// Manifest.xml中包括了要解析的3dxml文件名称
       /// </summary>
       private Manifest ReadManifestXml(string manifestFile)
       {
           try
           {
               XmlReader xmlReader = XmlReader.Create(manifestFile);
               XmlSerializer xmlSerializer = new XmlSerializer(typeof(Manifest));
               var result = xmlSerializer.Deserialize(xmlReader);
               xmlReader.Close();
               if (result is Manifest)
               {
                   return result as Manifest;                 
               }
               throw (new Exception("Parase manifest.Xml Error!"));
           }
           catch (Exception ex)
           {
               throw(new Exception("Parase manifest.Xml Error!" + ex));
           }
       }

       private Model3Dxml Read3DXmlStructure(string fileName)
       {
           try
           {
               DeleteStr(fileName);
               XmlReader xmlReader = XmlReader.Create(fileName);
                //FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                //var input = ReadXml("./PRODUCT.3dxml");
                //var course = XMLHelper.DeserializeXML<Model3Dxml>(input);
                //XmlSerializer xmlSerializer2 = new XmlSerializer(typeof(ModelBy3Dxml), "http://www.3ds.com/xsd/3DXML");
                //var result2 = xmlSerializer2.Deserialize(xmlReader) as Model3Dxml;
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(Model3Dxml), "http://www.3ds.com/xsd/3DXML");
                var result = xmlSerializer.Deserialize(xmlReader) as Model3Dxml;


                xmlReader.Close();
               if (result is Model3Dxml)
               {
                   return result ;
               }
               throw (new Exception("Parase Model3DxmlStru Error!"));
           }
           catch (Exception ex)
           {
               throw (new Exception("Parase Model3DxmlStru Error!" + ex));
            }
       }

       public void DeleteStr(string filePath)
       {
           string strContent = File.ReadAllText(filePath);
           strContent = Regex.Replace(strContent, "xsi:", "xsi");
           File.WriteAllText(filePath, strContent);
        }

        private string ReadXml(string path)
       {
           try
           {
               using (StreamReader stream = new StreamReader(path))
               {
                   return stream.ReadToEnd();
               }
           }
           catch (Exception e)
           {
               Console.WriteLine("The file could not be read:");
               Console.WriteLine(e.Message);
           }

           return string.Empty;
       }

        public Geometry3Dxml Read3Drep(string fileName)
       {
           try
           {
               Geometry3Dxml geometry3Dxml = new Geometry3Dxml();

               XmlReaderSettings xmlReaderSettings = new XmlReaderSettings()
               {
                   IgnoreComments = true,
                   IgnoreWhitespace = true
               };
               XmlReader xmlReader = XmlReader.Create(fileName, xmlReaderSettings);

               while (xmlReader.Read())
               {
                   if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name.Equals("Rep") &&
                       xmlReader.GetAttribute("xsi:type").Equals("PolygonalRepType")) //错误 
                   {
                       MeshGeometry3DXml meshGeometry3DXml = new MeshGeometry3DXml();
                       xmlReader.ReadStartElement();
                       while (xmlReader.NodeType != XmlNodeType.EndElement || !xmlReader.Name.Equals("Rep"))
                       {
                           if (xmlReader.Name.Equals("PolygonalLOD"))
                           {
                           }
                           else if (xmlReader.Name.Equals("Faces"))
                           {
                               xmlReader.ReadStartElement();
                               while (xmlReader.NodeType != XmlNodeType.EndElement || !xmlReader.Name.Equals("Faces"))
                               {
                                   if (xmlReader.Name.Equals("Face"))
                                   {
                                       string faceTrianglesString = xmlReader.GetAttribute("triangles");
                                       string faceStripsString = xmlReader.GetAttribute("strips");
                                       string faceFansString = xmlReader.GetAttribute("fans");
                                       if (faceTrianglesString != null)
                                       {
                                           string[] array =
                                               new Regex("[\\s]+").Replace(faceTrianglesString, " ").Trim().Split(' ');
                                           for (int i = 0; i < array.Length; i++)
                                           {
                                               //if (i % 3 == 0)
                                               //    faces.Add(3);
                                               meshGeometry3DXml.TriangleIndices.Add(int.Parse(array[i]));
                                           }
                                       }

                                       if (faceStripsString != null)
                                       {
                                           string[] stripsArray = faceStripsString.Split(',');
                                           foreach (string stripString in stripsArray)
                                           {
                                               string[] array =
                                                   new Regex("[\\s]+").Replace(stripString, " ").Trim().Split(' ');
                                               //faces.Add(3);
                                               meshGeometry3DXml.TriangleIndices.Add(int.Parse(array[0]));
                                               meshGeometry3DXml.TriangleIndices.Add(int.Parse(array[1]));
                                               meshGeometry3DXml.TriangleIndices.Add(int.Parse(array[2]));

                                               for (int i = 3; i < array.Length; i++)
                                               {
                                                   //faces.Add(3);
                                                   meshGeometry3DXml.TriangleIndices.Add(int.Parse(array[i - 2]));
                                                   if (i%2 == 0) //保证面的法向正确
                                                   {
                                                       meshGeometry3DXml.TriangleIndices.Add(int.Parse(array[i - 1]));
                                                       meshGeometry3DXml.TriangleIndices.Add(int.Parse(array[i - 0]));
                                                   }
                                                   else
                                                   {
                                                       meshGeometry3DXml.TriangleIndices.Add(int.Parse(array[i - 0]));
                                                       meshGeometry3DXml.TriangleIndices.Add(int.Parse(array[i - 1]));
                                                   }
                                               }
                                           }

                                           if (faceFansString != null)
                                           {
                                               //处理 trianglsFans
                                               string[] fansArray = faceFansString.Split(',');
                                               foreach (string fanString in fansArray)
                                               {
                                                   string[] array =
                                                       new Regex("[\\s]+").Replace(fanString, " ").Trim().Split(' ');
                                                   //faces.Add(3);
                                                   meshGeometry3DXml.TriangleIndices.Add(int.Parse(array[0]));
                                                   meshGeometry3DXml.TriangleIndices.Add(int.Parse(array[1]));
                                                   meshGeometry3DXml.TriangleIndices.Add(int.Parse(array[2]));
                                                   for (int i = 3; i < array.Length; i++)
                                                   {
                                                       //faces.Add(3);
                                                       meshGeometry3DXml.TriangleIndices.Add(int.Parse(array[0]));
                                                       meshGeometry3DXml.TriangleIndices.Add(int.Parse(array[i - 1]));
                                                       meshGeometry3DXml.TriangleIndices.Add(int.Parse(array[i]));
                                                   }
                                               }
                                           }
                                       }
                                   }
                                   xmlReader.Skip();
                               }
                           }
                           else if (xmlReader.Name.Equals("VertexBuffer"))
                           {
                               xmlReader.ReadStartElement();
                               while (xmlReader.NodeType != XmlNodeType.EndElement ||
                                      !xmlReader.Name.Equals("VertexBuffer"))
                               {
                                   if (xmlReader.Name.Equals("Positions"))
                                   {
                                       string vertexString = xmlReader.ReadInnerXml();
                                       string[] array = vertexString.Replace(",", " ").Trim().Split(' ');
                                       for (int i = 0; i < array.Length; i++)
                                       {
                                           meshGeometry3DXml.Points.Add(float.Parse(array[i]));
                                       }
                                   }

                                   xmlReader.Skip();
                               }
                           }
                           xmlReader.Skip();
                       }

                       geometry3Dxml.MeshGeometry3DXmls.Add(meshGeometry3DXml);
                   }
                   //xmlReader.Skip();
               }
               xmlReader.Close();
               return geometry3Dxml;
           }
           catch (Exception ex)
           {
               throw (new Exception("Parase Model3DxmlRep Error!" + ex));
           }     
       }

       public async Task<Geometry3Dxml> Read3DrepAsync(string file)
       {
           var result = await Task.Run(() => Read3Drep(file));
           return result;
       }

       public  Geometry3Dxml Read3DrepAsync2(string file)
       {
           var result = Read3Drep(file);
           return result;
       }
    }
}
