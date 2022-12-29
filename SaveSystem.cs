using System.Xml;
using System.IO;

namespace VistraFileSystem
{
    public class SaveSystem : FileSystem
    {
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
            File.Delete(Path + "output.xml");
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