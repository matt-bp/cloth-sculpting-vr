using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Wright.Library.File
{
    public static class DictionaryWriter
    {
        /// <summary>
        /// Serializes a dictionary to a file with the specified filename in the persistent data path provided by Unity.
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="filename"></param>
        public static void WriteToFile(Dictionary<string, object> dict, string filename)
        {
            var fullFilename = Path.Combine(Application.persistentDataPath, filename);
            Debug.Log($"Saving file to {fullFilename}");

            using var stream = System.IO.File.Create(fullFilename);
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, dict);
        }
    }
}