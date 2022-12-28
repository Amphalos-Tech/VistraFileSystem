using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace VistraFileSystem
{
    public class FileSystem
    {
        public static string Path { get; set; } = null;
        internal static XmlWriter writer;
        internal static XmlReader reader;
        internal static XmlWriterSettings settings;
        internal static System.Xml.Serialization.XmlSerializer serializer;
    }
}
