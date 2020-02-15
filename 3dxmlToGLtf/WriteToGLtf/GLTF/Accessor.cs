using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Linq;

namespace Arctron.Gltf
{
    /// <summary>
    /// A typed view into a buffer view.
    /// </summary>
    public class Accessor
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        /*  数据类型  */
        [JsonProperty("componentType")]
        public int ComponentType { get; set; }
        //public ComponentType ComponentType { get; set; }

        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("min")]
        [JsonConverter(typeof(DoubleArrayJsonConverter))]   // ？？？
        public double[] Min { get; set; }

        [JsonProperty("max")]
        [JsonConverter(typeof(DoubleArrayJsonConverter))]
        public double[] Max { get; set; }

        /// <summary>
        /// Specifies if the attribute is a scalar, vector, or matrix.
        /// </summary>
        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        //public string Type { get; set; }
        public AccessorType Type { get;set; }

        /// <summary>
        /// The parent buffer view this accessor reads from.
        /// </summary>
        [JsonProperty("bufferView")]
        public int BufferView { get; set; }
        /// <summary>
        /// The offset relative to the start of the parent `BufferView` in bytes.
        /// </summary>
        [JsonProperty("byteOffset")]
        public int ByteOffset { get; set; }

        public double[] GetMinVec(List<float> vertexes)
        {
            double[] minVec = new double[3] { 0 ,0, 0 };
            double min1 = vertexes[0], min2 = vertexes[1], min3 = vertexes[2];
            for (int j = 0; j < vertexes.Count(); j = j + 3) { if (vertexes[j] < min1) { min1 = vertexes[j]; } }
            for (int j = 1; j < vertexes.Count(); j = j + 3) { if (vertexes[j] < min2) { min2 = vertexes[j]; } }
            for (int j = 2; j < vertexes.Count(); j = j + 3) { if (vertexes[j] < min3) { min3 = vertexes[j]; } }
            minVec[0] = min1; minVec[1] = min2; minVec[2] = min3;
            return minVec;
        }

        public double[] GetMaxVec(List<float> vertexes)
        {
            double[] maxVec = new double[3] { 0, 0, 0 };
            double max1 = vertexes[0], max2 = vertexes[1], max3 = vertexes[2];
            for (int j = 0; j < vertexes.Count(); j = j + 3) { if (vertexes[j] > max1) { max1 = vertexes[j]; } }
            for (int j = 1; j < vertexes.Count(); j = j + 3) { if (vertexes[j] > max2) { max2 = vertexes[j]; } }
            for (int j = 2; j < vertexes.Count(); j = j + 3) { if (vertexes[j] > max3) { max3 = vertexes[j]; } }
            maxVec[0] = max1; maxVec[1] = max2; maxVec[2] = max3;
            return maxVec;
        }
    }


}
