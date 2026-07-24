using System;
using System.Collections.Generic;

namespace Drafts.SaveData
{
    public class SaveManager
    {
        private ISaveDataParser Parser { get; }
        private Dictionary<string, object> pairs = new();

        public string Root { get; private set; }
        public string Folder { get; private set; }
        public event Action<SaveManager> OnLoaded;
        public event Action<SaveManager> OnSaving;

        public SaveManager(string root, string saveName = null, ISaveDataParser parser = null)
        {
            Root = root;
            Folder = saveName;
            Parser = parser ?? new JsonFileParser();
        }

        public T Get<T>(string path = null) => (T)Get(typeof(T), path);

        public object Get(Type type, string path = null)
        {
            var key = Parser.GetKey(Root, Folder, path ?? type.Name);
            if (Folder == null) throw new SaveNotLoadedException();
            if (pairs.TryGetValue(key, out var data)) return data;
            return pairs[key] = Parser.Load(key, type);
        }

        public void Set<T>(T data, string path = null)
        {
            var key = Parser.GetKey(Root, Folder, path ?? typeof(T).Name);
            pairs[key] = data;
        }

        public void New(string saveName)
        {
            Folder = saveName;
            pairs.Clear();
        }

        public void Save()
        {
            if (Folder == null) throw new SaveNotLoadedException();
            OnSaving?.Invoke(this);

            foreach (var pair in pairs)
                Parser.Save(pair.Key, pair.Value);
        }

        public void Load() => Load(Folder);

        public void Load(string saveName)
        {
            New(saveName);
            OnLoaded?.Invoke(this);
        }

        public void Clear()
        {
            if (Folder == null) throw new SaveNotLoadedException();
            pairs.Clear();
        }

        public IEnumerable<string> GetSaveNames() => Parser.GetSaveNames(Root);

        public IEnumerable<(string saveName, T data)> GetFromAll<T>(string path = null)
        {
            var type = typeof(T);
            path ??= type.Name;

            foreach (var name in GetSaveNames())
            {
                var key = Parser.GetKey(Root, name, path);
                var data = (T)Parser.Load(key, type);
                yield return (name, data);
            }
        }

        public object GetFrom<T>(string saveName, string path = null)
        {
            var key = Parser.GetKey(Root, saveName, path ?? typeof(T).Name);
            return Parser.Load(key, typeof(T));
        }

        public bool Delete(string saveName) => Parser.Delete(Root, saveName);
    }
}