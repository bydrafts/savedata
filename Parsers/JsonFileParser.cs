using System;
using System.IO;
using UnityEngine;

namespace Drafts.SaveData
{
    public class JsonFileParser : ISaveDataParser
    {
        public void Save(string path, in object data)
        {
            path = Path.Combine(path, data.GetType().Name + ".json");
            var json = JsonUtility.ToJson(data, true);
            var dir = Path.GetDirectoryName(path);
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            File.WriteAllText(path, json);
        }

        public object Load(string path, Type type)
        {
            path = Path.Combine(path, type.Name + ".json");
            if (!File.Exists(path)) return Activator.CreateInstance(type);
            var txt = File.ReadAllText(path);
            return JsonUtility.FromJson(txt, type);
        }
    }
}