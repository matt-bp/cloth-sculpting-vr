using System.IO;
using UnityEngine;
using Newtonsoft.Json;

namespace Wright.Library.File
{
    public static class DictionaryFileHelper
    {
        /// <summary>
        /// Serializes a dictionary to a file with the specified filename in the persistent data path provided by Unity.
        /// </summary>
        /// <param name="thingToSave"></param>
        /// <param name="filename"></param>
        public static void WriteToFile<T>(T thingToSave, string filename)
        {
            Debug.Assert(filename.Contains(".json"));
            
            var fullFilename = Path.Combine(Application.persistentDataPath, filename);
            Debug.Log($"Saving file to {fullFilename}");

            using var stream = System.IO.File.Create(fullFilename);
            using var writer = new StreamWriter(stream);

            var result = JsonConvert.SerializeObject(thingToSave);

            writer.WriteLine(result);
        }

        public static bool LoadFromDisk<T>(string filename, out T fromDisk)
        {
            Debug.Assert(filename.Contains(".json"));
            
            var fullFilename = Path.Combine(Application.persistentDataPath, filename);

            fromDisk = default;

            if (!System.IO.File.Exists(fullFilename))
            {
                Debug.Log($"No saved game at {fullFilename}");
                return false;
            }

            var json = string.Concat(System.IO.File.ReadLines(fullFilename));
            Debug.Log($"READ JSON: {json}");
            fromDisk = JsonConvert.DeserializeObject<T>(json);
            
            return true;
        }
    }
}