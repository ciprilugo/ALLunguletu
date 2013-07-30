using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;

namespace ALLunguletu.Helpers
{
    [DataContract]
    public class Filter
    {
        [DataMember]
        public string groupOp { get; set; }
        [DataMember]
        public Rule[] rules { get; set; }

        public static Filter Create(string jsonData)
        {
            try
            {
                var serializer =
                  new DataContractJsonSerializer(typeof(Filter));
                System.IO.StringReader reader =
                  new System.IO.StringReader(jsonData);
                System.IO.MemoryStream ms =
                  new System.IO.MemoryStream(
                  Encoding.Default.GetBytes(jsonData));
                return serializer.ReadObject(ms) as Filter;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }


    [DataContract]
    public class Rule
    {
        [DataMember]
        public string field { get; set; }
        [DataMember]
        public string op { get; set; }
        [DataMember]
        public string data { get; set; }
    }
}