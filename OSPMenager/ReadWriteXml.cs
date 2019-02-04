using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
//using CsvHelper;
using Microsoft.Win32;
using System.Xml;

namespace OSPMenager
{
    class ReadWriteXml
    {

        public static void savedata(object obj, string filename)
        {
            XmlSerializer serializer = new XmlSerializer(obj.GetType());
            TextWriter writer = new StreamWriter(filename);
            serializer.Serialize(writer, obj);
            writer.Close();
        }

        public static object readdata(string path, string filename)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(List<Druh>));
            StreamReader reader = new StreamReader("supernazwa.xml");
            List<Druh> input = (List<Druh>)deserializer.Deserialize(reader);
            return input;
        }
    }
}
