using System;
using System.IO;
using System.Security.Cryptography;
using System.Xml;

namespace VistraFileSystem
{
    public static class XmlEncryptionHelper
    {
        private static readonly Aes Aes = Aes.Create();

        public static void EncryptXmlFile(string inputXmlFilePath, string outputXmlFilePath, string keyFilePath)
        {
            Aes.GenerateKey();
            Aes.GenerateIV();

            using (var keyFileStream = new FileStream(keyFilePath, FileMode.Create, FileAccess.Write))
            using (var keyWriter = new BinaryWriter(keyFileStream)) //write key file
            {
                keyWriter.Write(Aes.Key);
                keyWriter.Write(Aes.IV);
            }

            using (var inputXmlStream = new FileStream(inputXmlFilePath, FileMode.Open, FileAccess.Read))
            using (var outputXmlStream = new FileStream(outputXmlFilePath, FileMode.Create, FileAccess.Write))
            {
                var encryptedXmlDocument = new XmlDocument();
                var encryptedElement = encryptedXmlDocument.CreateElement("EncryptedData"); //create root so it doesnt entirely collapse on decryption
                encryptedXmlDocument.AppendChild(encryptedElement);

                using (var memoryStream = new MemoryStream())
                using (var cryptoStream = new CryptoStream(memoryStream, Aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    inputXmlStream.CopyTo(cryptoStream);
                    cryptoStream.FlushFinalBlock();
                    memoryStream.Position = 0;

                    var encryptedData = Convert.ToBase64String(memoryStream.ToArray());

                    encryptedElement.InnerText = encryptedData; //to be decrypted data of element
                }

                encryptedXmlDocument.Save(outputXmlStream);
            }
        }


        public static void DecryptXmlFile(string inputXmlFilePath, string outputXmlFilePath, string keyFilePath)
        {
            byte[] key, iv;
            using (var keyFileStream = new FileStream(keyFilePath, FileMode.Open, FileAccess.Read))
            using (var keyReader = new BinaryReader(keyFileStream))
            {
                key = keyReader.ReadBytes(Aes.KeySize / 8);
                iv = keyReader.ReadBytes(Aes.BlockSize / 8);
            }

            Aes.Key = key;
            Aes.IV = iv;

            using (var inputXmlStream = new FileStream(inputXmlFilePath, FileMode.Open, FileAccess.Read))
            {

                var encryptedXmlDocument = new XmlDocument();
                encryptedXmlDocument.Load(inputXmlStream);

                var encryptedElement = encryptedXmlDocument.SelectSingleNode("EncryptedData") as XmlElement; //needed for fully formed xml output
                #pragma warning disable
                var encryptedData = encryptedElement.InnerText;

                var encryptedBytes = Convert.FromBase64String(encryptedData);

                using (var outputXmlStream = new FileStream(outputXmlFilePath, FileMode.Create, FileAccess.Write))
                using (var cryptoStream = new CryptoStream(outputXmlStream, Aes.CreateDecryptor(), CryptoStreamMode.Write))//decrypt bytes then write to outputstream
                {
                    cryptoStream.Write(encryptedBytes, 0, encryptedBytes.Length);
                    cryptoStream.FlushFinalBlock();
                }
            }
        }

    }
}
