using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace Core
{
    public class FileLoader<T> : ILoader<T>
    {
        private readonly JsonSerializerSettings _settings = new JsonSerializerSettings()
            { TypeNameHandling = TypeNameHandling.Auto };
        
        public T Load(string fileName)
        {
            T result = default(T);
            string filePath = Path.Combine(Application.persistentDataPath, fileName);

            if (File.Exists(filePath))
            {
                string jsonString = File.ReadAllText(filePath);
                if (!string.IsNullOrEmpty(jsonString))
                {
                    result = LoadFromJson(jsonString);
                }
                Debug.Log($"FileLoader: Loaded {typeof(T)} from {filePath}");
            }
            else
            {
                Debug.Log($"FileLoader: File doesn't exists {typeof(T)} in {filePath}");
            }
            
            return result;
        }

        public bool Save(string fileName,T objectToSave)
        {
            string filePath = Path.Combine(Application.persistentDataPath, fileName);

            string jsonString = SaveToJson(objectToSave);
            if (!string.IsNullOrEmpty(jsonString))
            {
                File.WriteAllText(filePath, jsonString);
                Debug.Log($"FileLoader: Saved {typeof(T)} to {filePath}");
                return true;
            }

            return false;
        }

        private T LoadFromJson(string json)
        {
            T result;
            try
            {
                result = JsonConvert.DeserializeObject<T>(json, _settings);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                throw;
            }

            return result;
        }

        private string SaveToJson(T objectToSave)
        {
            string result;

            try
            {
                result = JsonConvert.SerializeObject(objectToSave, _settings);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                throw;
            }

            return result;
        }
    }
}