using System;
using System.IO;
using UnityEngine;

namespace Drafts.SaveData
{
    public class JsonFileParser : FileParserBase
    {
        protected override string Extension => ".json";
        private bool Beautify { get; }

        public JsonFileParser(bool beautify = true) => Beautify = beautify;

        public override void Save(string key, in object data)
        {
            var json = JsonUtility.ToJson(data, Beautify);
            AssurePath(key);
            File.WriteAllText(key, json);
        }

        public override object Load(string key, Type type)
        {
            if (!File.Exists(key))
                return Activator.CreateInstance(type);
            var txt = File.ReadAllText(key);
            return JsonUtility.FromJson(txt, type);
        }
    }
}