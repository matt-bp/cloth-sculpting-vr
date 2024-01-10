using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Wright.Library.File
{
    public static class DictionaryFileHelper
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

        public static bool LoadFromDisk(string filename, out Dictionary<string, object> dict)
        {
            var fullFilename = Path.Combine(Application.persistentDataPath, filename);
            
            dict = new Dictionary<string, object>();
            
            if (!System.IO.File.Exists(fullFilename))
            {
                Debug.Log($"No saved game at {fullFilename}");
                return false;
            }

            using var stream = System.IO.File.Open(fullFilename, FileMode.Open);
            var formatter = new BinaryFormatter();
            dict = formatter.Deserialize(stream) as Dictionary<string, object>;
            return true;
        }
        
    }
}