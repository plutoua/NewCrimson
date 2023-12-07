using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using JetBrains.Annotations;
using TimmyFramework.Surrogates;
using UnityEngine;

namespace TimmyFramework
{
    public class StoreSystem
    {
        private string _filePath;
        private BinaryFormatter _formatter;

        public StoreSystem(string directoryName, string fileName)
        {
            var directory = Application.persistentDataPath + "/" + directoryName;
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            _filePath = directory + "/" + fileName;

            InitializeBinaryFormater();
        }

        public object Load(object saveDataByDefault)
        {
            if (!File.Exists(_filePath))
            {
                if (saveDataByDefault != null)
                {
                    Save(saveDataByDefault);
                }

                return saveDataByDefault;
            }

            var file = File.Open(_filePath, FileMode.Open);
            var savedData = _formatter.Deserialize(file);
            file.Close();
            return savedData;
        }

        public void Save(object saveData)
        {
            var file = File.Create(_filePath);
            _formatter.Serialize(file, saveData);
            file.Close();
        }

        private void InitializeBinaryFormater()
        {
            _formatter = new BinaryFormatter();
            var selector = new SurrogateSelector();

            var vector3Surrogate = new Vector3SerializationSurrogate();
            var quaternionSurrogate = new QuaternionSerializationSurrogate();
            
            selector.AddSurrogate(typeof(Vector3), new StreamingContext(StreamingContextStates.All), vector3Surrogate);
            selector.AddSurrogate(typeof(Quaternion), new StreamingContext(StreamingContextStates.All), quaternionSurrogate);

            _formatter.SurrogateSelector = selector;
        }
    }
}