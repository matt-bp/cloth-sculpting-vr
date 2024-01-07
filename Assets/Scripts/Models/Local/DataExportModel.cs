using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Models.Global;
using UnityEngine;
using Wright.Library.File;

namespace Models.Local
{
    [RequireComponent(typeof(TaskResultModel))]
    public class DataExportModel : MonoBehaviour
    {
        private TaskResultModel _taskResultModel;

        private void Start()
        {
            _taskResultModel = GetComponent<TaskResultModel>();
        }

        public void SaveGameStatus()
        {
            var filename = "game"; // needs to change
            var datedFilename = FilenameSanitizer.Sanitize($"{filename}_{DateTime.Now}.dat");
            var fullFilename = Path.Combine(Application.persistentDataPath, datedFilename);
            Debug.Log($"Saving file to {fullFilename}");
            
            var gameState = new Dictionary<string, object> { { "time", 1.0 } };

            using var stream = File.Create(fullFilename);
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, gameState);
        }
    }
}