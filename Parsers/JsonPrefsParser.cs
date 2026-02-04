using System;
using System.IO;
using UnityEngine;

namespace Drafts.SaveData
{
    public class JsonPrefsParser : ISaveDataParser
    {
        public void Save(string path, in object data)
        {
            path = Path.Combine(path, data.GetType().Name);
            var json = JsonUtility.ToJson(data, false);
            PlayerPrefs.SetString(path, json);
        }

        public object Load(string path, Type type)
        {
            path = Path.Combine(path, type.Name);
            if (!PlayerPrefs.HasKey(path)) 
                return Activator.CreateInstance(type);
            
            var txt = PlayerPrefs.GetString(path);
            return JsonUtility.FromJson(txt, type);
        }
    }
}