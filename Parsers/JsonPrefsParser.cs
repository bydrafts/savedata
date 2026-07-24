using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Drafts.SaveData
{
    public class JsonPrefsParser : ISaveDataParser
    {
        public string GetKey(params string[] path) => Path.Combine(path);
        public bool KeyExists(string key) => PlayerPrefs.HasKey(key);

        public void Save(string key, in object data)
        {
            key = Path.Combine(key, data.GetType().Name);
            var json = JsonUtility.ToJson(data, false);
            PlayerPrefs.SetString(key, json);
        }

        public object Load(string key, Type type)
        {
            key = Path.Combine(key, type.Name);
            if (!PlayerPrefs.HasKey(key))
                return Activator.CreateInstance(type);

            var txt = PlayerPrefs.GetString(key);
            return JsonUtility.FromJson(txt, type);
        }

        public bool Delete(string[] path)
        {
            Debug.LogError("JsonPrefsParser dont support save deletion. Delete keys individually.");
            return false;
        }

        public IEnumerable<string> GetSaveNames(string root)
        {
            Debug.LogError("JsonPrefsParser dont support listing saves.");
            yield break;
        }
    }
}