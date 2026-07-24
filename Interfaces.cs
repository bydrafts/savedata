using System;
using System.Collections.Generic;

namespace Drafts.SaveData
{
    public class SaveNotLoadedException : Exception { }

    public interface ISaveDataParser
    {
        bool KeyExists(string key);
        public void Save(string key, in object data);
        object Load(string key, Type type);
        bool Delete(params string[] path);
        string GetKey(params string[] path);
        IEnumerable<string> GetSaveNames(string root);
    }
}