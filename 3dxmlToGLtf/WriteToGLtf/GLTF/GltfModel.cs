using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Arctron.Gltf
{
    /// <summary>
    /// gltf json model  实例化一个gltf文件
    /// </summary>
    public class GltfModel
    {
        [JsonProperty("asset")]
        public Asset Asset { get; set; } = new Asset { Generator = "arctron", Version = "2.0" };
        [JsonProperty("scene")]
        public int Scene { get; set; }
        [JsonProperty("scenes")]
        public List<Scene> Scenes { get; set; } = new List<Scene>();
        [JsonProperty("nodes")]
        public List<Node> Nodes { get; set; } = new List<Node>();
        [JsonProperty("meshes")]
        public List<Mesh> Meshes { get; set; } = new List<Mesh>();
        [JsonProperty("accessors")]
        public List<Accessor> Accessors { get; set; } = new List<Accessor>();
        [JsonProperty("materials")]
        public List<Material> Materials { get; set; } = new List<Material>();
        [JsonProperty("textures")]
        public List<Texture> Textures { get; set; } = new List<Texture>();
        [JsonProperty("images")]
        public List<Image> Images { get; set; } = new List<Image>();
        [JsonProperty("samplers")]
        public List<Sampler> Samplers { get; set; } = new List<Sampler>();
        [JsonProperty("bufferViews")]
        public List<BufferView> BufferViews { get; set; } = new List<BufferView>();
        [JsonProperty("buffers")]
        public List<Buffer> Buffers { get; set; } = new List<Buffer>();



        /// <summary>
        /// Load gltf json file
        /// </summary>
        /// <param name="filePath">gltf file path</param>
        /// <returns></returns>
        public static GltfModel LoadFromJsonFile(string filePath)
        {
            var s = System.IO.File.ReadAllText(filePath, Encoding.UTF8);
            return JsonConvert.DeserializeObject<GltfModel>(s);
        }

        public void Clean()
        {
            if (Images != null && Images.Count == 0)
            {
                Images = null;
            }
            if (Textures != null && Textures.Count == 0)
            {
                Textures = null;
            }
            if (Samplers != null && Samplers.Count == 0)
            {
                Samplers = null;
            }
        }

        public string ToJson(object model)
        {
            return JsonConvert.SerializeObject(model,
                    new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        Formatting = Formatting.Indented
                    });
        }
    }
}
