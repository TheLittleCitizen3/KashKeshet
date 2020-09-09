using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace Common
{
    public static class Serializator
    {
        public static  byte[] Serialize<T>(T response)
        {
            byte[] bytes;
            using (var stream = new MemoryStream())
            {
                var serivalizer = new DataContractSerializer(typeof(T));
                serivalizer.WriteObject(stream, response);
                bytes = new byte[stream.Length];
                stream.Position = 0;
                stream.Read(bytes, 0, (int)stream.Length);
            }
            return bytes;

        }

        public static T Deserialize<T>(byte[] data)
        {
            T deserializedObject = default(T);

            using (var stream = new MemoryStream(data))
            using (var reader = XmlDictionaryReader.CreateTextReader(stream, new XmlDictionaryReaderQuotas()))
            {
                var serializer = new DataContractSerializer(typeof(T));

                deserializedObject = (T)serializer.ReadObject(reader, true);
            }
            return deserializedObject;
        }
    }
}
