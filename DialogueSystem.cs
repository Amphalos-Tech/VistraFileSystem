﻿using System.Xml;
using System.Collections.Generic;

namespace VistraFileSystem
{
    public class DialogueSystem : FileSystem
    {
        static DialogueSystem()
        {
            settings = new XmlWriterSettings();
            settings.Indent = true;
            serializer = new System.Xml.Serialization.XmlSerializer(typeof(List<DialogueBlock>));
        }

        public static List<DialogueBlock> Read(SaveFile.GameStage stage) //Returns dialogue list for appropriate stage
        {
            var outputPath = Path + "output" + stage + ".xml";
            var encryptedPath = Path + "encrypted" + stage + ".xml";
            var keyPath = Path + "key" + stage + ".xml";

            XmlEncryptionHelper.DecryptXmlFile(encryptedPath, outputPath, keyPath);
            reader = XmlReader.Create(outputPath);
            #pragma warning disable
            List<DialogueBlock> list = (List<DialogueBlock>)serializer.Deserialize(reader);
            reader.Close();
            return list;
        }

        public static void Write(List<DialogueBlock> dialogueChunk, SaveFile.GameStage stage) //Serliazes dialogue blocks, only for developement use, DO NOT CALL IN BUILD
        {
            var rawPath = Path + "rawdialogue" + stage + ".xml";
            var encryptedPath = Path + "encrypted" + stage + ".xml";
            var keyPath = Path + "key" + stage + ".xml";
            writer = XmlWriter.Create(rawPath, settings);
            serializer.Serialize(writer, dialogueChunk);
            writer.Close();
            writer.Flush();
            XmlEncryptionHelper.EncryptXmlFile(rawPath, encryptedPath, keyPath);
        }

    }
}