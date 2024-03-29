﻿using System.Xml;
using System.IO;

namespace VistraFileSystem
{
    public class SaveSystem : FileSystem
    {
        static SaveSystem()
        {
            settings = new XmlWriterSettings();
            settings.Indent = true;
        }

        public static SaveFile Read() //Returns SaveFile object from xml 
        {
            serializer = new System.Xml.Serialization.XmlSerializer(typeof(SaveFile));
            var outputPath = Path + "output.xml";
            XmlEncryptionHelper.DecryptXmlFile(Path + "encrypted.xml", outputPath, Path + "key.xml");
            reader = XmlReader.Create(outputPath);
#pragma warning disable
            SaveFile s = (SaveFile)serializer.Deserialize(reader);
            reader.Close();
            File.Delete(outputPath);
            return s;
        }

        public static void Write(SaveFile o) //Serliazes SaveFile object to xml
        {
            serializer = new System.Xml.Serialization.XmlSerializer(typeof(SaveFile));
            writer = XmlWriter.Create(Path + "save.xml", settings);
            serializer.Serialize(writer, o);
            writer.Close();
            writer.Flush();
            XmlEncryptionHelper.EncryptXmlFile(Path + "save.xml", Path + "encrypted.xml", Path + "key.xml");
        }
    }
}