using System.IO;
using System.Xml.Serialization;
using UnityEngine;

namespace Util
{
    public static class XmlUtil
    {
        public static T Deserialize<T>(TextAsset xmlAsset)
        {
            return Deserialize<T>(xmlAsset.text);
        }

        public static T Deserialize<T>(string xmlText)
        {
            using (var reader = new StringReader(xmlText))
            {
                var serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(reader);
            }
        }
    }
}
