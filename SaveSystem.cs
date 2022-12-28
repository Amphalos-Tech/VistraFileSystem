using System.Xml;
using System.Runtime;
namespace SaveSystem
{
    public static class SaveSystem
    {
        public static string Path { get; set; } = null;
        private static XmlWriter writer;
        private static XmlReader reader;
        private static XmlWriterSettings settings;
        private static System.Xml.Serialization.XmlSerializer serializer;

        static SaveSystem()
        {
            settings = new XmlWriterSettings();
            settings.Indent = true;
            serializer = new System.Xml.Serialization.XmlSerializer(typeof(SaveFile));
        }

        public static SaveFile Read() //Returns SaveFile object from xml 
        {
            XmlEncryptionHelper.DecryptXmlFile(Path + "encrypted.xml", Path + "output.xml", Path + "key.xml");
            reader = XmlReader.Create(Path + "output.xml");
            #pragma warning disable
            SaveFile s = (SaveFile)serializer.Deserialize(reader);
            reader.Close();
            return s;
        }

        public static void Write(SaveFile o) //Serliazes SaveFile object to xml
        {
            writer = XmlWriter.Create(Path + "save.xml", settings);
            serializer.Serialize(writer, o);
            writer.Close();
            writer.Flush();
            XmlEncryptionHelper.EncryptXmlFile(Path + "save.xml", Path + "encrypted.xml", Path + "key.xml");
        }
    }
}