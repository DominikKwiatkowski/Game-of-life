using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GameOfLife.Common
{
    /// <summary>
    /// Common static utils unconnected with project
    /// </summary>
    static class CommonUtils
    {
        /// <summary>
        /// Dump object to xml file.
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="filePath">Path of save file</param>
        /// <param name="objectToWrite">Object to be saved</param>
        public static void WriteToXmlFile<T>(string filePath, T objectToWrite) where T : new()
        {
            TextWriter writer = null;
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                writer = new StreamWriter(filePath);
                serializer.Serialize(writer, objectToWrite);
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }

        /// <summary>
        /// Read object from xml file.
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="filePath">Path of load file</param>
        /// <returns>Object loaded from file</returns>
        public static T ReadFromXmlFile<T>(string filePath) where T : new()
        {
            TextReader reader = null;
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                reader = new StreamReader(filePath);
                return (T)serializer.Deserialize(reader);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }
    }
}
